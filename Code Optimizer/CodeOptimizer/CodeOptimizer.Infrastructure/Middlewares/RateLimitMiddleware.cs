using CodeOptimizer.Infrastructure.Attributes;
using CodeOptimizer.Infrastructure.Cache;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace CodeOptimizer.Infrastructure.Middlewares
{
    public class RateLimitMiddleware
    {
        private readonly RequestDelegate _next;

        public RateLimitMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var cache = context.RequestServices.GetRequiredService<ICacheService>();
            var attr = context.GetEndpoint()?.Metadata.GetMetadata<RateLimitAttribute>();

            if (attr == null)
            {
                await _next(context);
                return;
            }

            string userId = context.User.Identity?.IsAuthenticated == true
                ? context.User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value
                : context.Connection.RemoteIpAddress?.ToString() ?? "guest";

            string endpoint = context.Request.Path.ToString().ToLower();
            string rateKey = $"rl:{userId}:{endpoint}:{attr.PerSeconds}";

            string? cacheKey = TryBuildCacheKeyFromRequest(context);

            bool isCacheHit = false;
            if (cacheKey != null)
            {
                var cachedObj = await cache.GetAsync<object>(cacheKey);
                isCacheHit = cachedObj != null;
            }

            if (isCacheHit)
            {
                await _next(context);
                return;
            }

            int count = await cache.GetAsync<int>(rateKey);
            if (count >= attr.MaxRequests)
            {
                context.Response.StatusCode = 429;
                await context.Response.WriteAsJsonAsync(new
                {
                    error = $"Rate limit exceeded: {attr.MaxRequests} per {attr.PerSeconds}s"
                });
                return;
            }

            await _next(context);

            await cache.SetAsync(rateKey, count + 1, TimeSpan.FromSeconds(attr.PerSeconds));
        }



        private string? TryBuildCacheKeyFromRequest(HttpContext context)
        {
            try
            {
                context.Request.EnableBuffering();

                using var reader = new StreamReader(context.Request.Body, Encoding.UTF8, leaveOpen: true);
                var body = reader.ReadToEndAsync().Result;
                context.Request.Body.Position = 0;

                if (string.IsNullOrWhiteSpace(body)) return null;

                var json = JsonDocument.Parse(body);

                if (!json.RootElement.TryGetProperty("code", out var codeProp))
                    return null;

                var code = codeProp.GetString();
                if (string.IsNullOrWhiteSpace(code)) return null;

                var normalized = Normalize(code);
                return Hash(normalized);
            }
            catch
            {
                return null;
            }
        }

        private string Normalize(string code)
        {
            var lines = code.Replace("\r\n", "\n")
                .Split("\n")
                .Select(l => l.Trim())
                .Where(l => l.Length > 0);

            var flat = string.Join(" ", lines);
            while (flat.Contains("  "))
                flat = flat.Replace("  ", " ");

            return flat;
        }

        private string Hash(string input)
        {
            using var sha = SHA256.Create();
            return Convert.ToHexString(sha.ComputeHash(Encoding.UTF8.GetBytes(input)));
        }

    }
}
