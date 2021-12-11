using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace Homeapp.Backend.Managers
{
    public class AuthenticationManager : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        public AuthenticationManager(IOptionsMonitor<AuthenticationSchemeOptions> options,
            ILoggerFactory logger, 
            UrlEncoder encoder, 
            ISystemClock clock) 
            : base(options, logger, encoder, clock)
        {

        }

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (!Request.Headers.ContainsKey("Authorization"))
            {
                return Task.FromResult(AuthenticateResult.Fail("Authorization header was not found."));
            }

            var authenticationHeaderValue = AuthenticationHeaderValue.Parse(Request.Headers["Authorization"]);
            var bytes = Convert.FromBase64String(authenticationHeaderValue.Parameter);
            string credentials = Encoding.UTF8.GetString(bytes);

            return Task.FromResult(AuthenticateResult.Fail("Authorization not implemented"));
        }
    }
}
