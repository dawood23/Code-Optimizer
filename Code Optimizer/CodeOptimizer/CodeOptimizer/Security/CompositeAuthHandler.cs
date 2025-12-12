using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace CodeOptimizer.API.Security
{
    public class CompositeAuthHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        public CompositeAuthHandler(
            IOptionsMonitor<AuthenticationSchemeOptions> options,
            ILoggerFactory logger,
            System.Text.Encodings.Web.UrlEncoder encoder,
            ISystemClock clock)
            : base(options, logger, encoder, clock)
        { }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (!Request.Headers.ContainsKey("Authorization"))
                return AuthenticateResult.NoResult();

            var primary = await Context.AuthenticateAsync("Bearer");
            if (primary?.Succeeded == true) return primary;

            var owin = await Context.AuthenticateAsync("OwinJwt");
            if (owin?.Succeeded == true) return owin;
            return AuthenticateResult.Fail("No valid token");
        }

        protected override Task HandleChallengeAsync(AuthenticationProperties properties)
        {
            Response.StatusCode = 401;
            return Task.CompletedTask;
        }
    }
}
