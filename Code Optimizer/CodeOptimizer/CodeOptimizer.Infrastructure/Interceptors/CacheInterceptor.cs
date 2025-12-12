using CodeOptimizer.Infrastructure.Cache;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using Castle.DynamicProxy;
using System.Text.Json;
using System.Linq;

namespace CodeOptimizer.Infrastructure.Interceptors
{
    public class AutoCacheInterceptor : IInterceptor
    {
        private readonly ICacheService _cache;
        private readonly IHttpContextAccessor _http;
        private readonly ILogger<AutoCacheInterceptor> _logger;

        public AutoCacheInterceptor(
            ICacheService cache,
            IHttpContextAccessor contextAccessor,
            ILogger<AutoCacheInterceptor> logger)
        {
            _cache = cache;
            _http = contextAccessor;
            _logger = logger;
        }

        public void Intercept(IInvocation invocation)
        {
            if (!IsAsyncMethod(invocation.Method))
            {
                invocation.Proceed();
                return;
            }

            var genericType = invocation.Method.ReturnType.GenericTypeArguments[0];

            var method = typeof(AutoCacheInterceptor)
                .GetMethod(nameof(InterceptAsync), BindingFlags.Instance | BindingFlags.NonPublic)!
                .MakeGenericMethod(genericType);

            invocation.ReturnValue = method.Invoke(this, new object[] { invocation });
        }

        private async Task<T> InterceptAsync<T>(IInvocation invocation)
        {
            string cacheKey = BuildCacheKey(invocation);

            var cached = await _cache.GetAsync<T>(cacheKey);
            if (cached is not null)
            {
                MarkCacheHit();

                _logger.LogInformation("[CACHE HIT] {CacheKey}", cacheKey);

                return cached;
            }

            invocation.Proceed();
            var task = (Task<T>)invocation.ReturnValue;
            var result = await task.ConfigureAwait(false);

            await _cache.SetAsync(cacheKey, result, TimeSpan.FromMinutes(10));

            return result;
        }

        private bool IsAsyncMethod(MethodInfo method)
        {
            return method.ReturnType.IsGenericType &&
                   method.ReturnType.GetGenericTypeDefinition() == typeof(Task<>);
        }
        private string BuildCacheKey(IInvocation invocation)
        {
            if (invocation.Arguments.Length > 0 && invocation.Arguments[0] != null)
            {
                var arg = invocation.Arguments[0];
                var codeProp = arg.GetType().GetProperty("Code");

                if (codeProp != null)
                {
                    var codeValue = codeProp.GetValue(arg) as string;

                    if (!string.IsNullOrWhiteSpace(codeValue))
                    {
                        var normalized = NormalizeCode(codeValue);
                        return Hash(normalized);
                    }
                }
            }

            return "EMPTY_CODE_KEY";
        }

        private string NormalizeCode(string code)
        {
            var lines = code
                .Replace("\r\n", "\n")
                .Split('\n')
                .Select(l => l.Trim())
                .Where(l => !string.IsNullOrWhiteSpace(l));

            var flattened = string.Join(" ", lines);

            while (flattened.Contains("  "))
                flattened = flattened.Replace("  ", " ");

            return flattened;
        }

        private string Hash(string input)
        {
            using var sha = SHA256.Create();
            return Convert.ToHexString(sha.ComputeHash(Encoding.UTF8.GetBytes(input)));
        }

        private void MarkCacheHit()
        {
            var ctx = _http.HttpContext;
            if (ctx == null) return;

            ctx.Items["CACHE_HIT"] = true;

            if (!ctx.Response.HasStarted &&
                !ctx.Response.Headers.ContainsKey("x-cache-status"))
            {
                ctx.Response.Headers.Add("x-cache-status", "HIT");
            }
        }
    }
}
