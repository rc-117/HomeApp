using DummyRepo;
using DummyRepo.Entities;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Linq;
using System.Net.Http.Headers;
using System.Security.Claims;
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
            string[] credentials = Encoding.UTF8.GetString(bytes).Split(":");
            string email = credentials[0];
            string passwordHash = credentials[1];

            //Test user from dummy repo
            User user = Repo.Users.Where(u => 
                u.EmailAddress == email && u.PasswordHash == passwordHash)
                .FirstOrDefault();

            if (user == null)
            {
                return Task.FromResult(AuthenticateResult.Fail("Invalid username or password."));
            }
            else
            {
                var claims = new[] { new Claim(ClaimTypes.Name, user.EmailAddress) };
                var identity = new ClaimsIdentity(claims, Scheme.Name);
                var principal = new ClaimsPrincipal(identity);
                var ticket = new AuthenticationTicket(principal, Scheme.Name);

                return Task.FromResult(AuthenticateResult.Success(ticket));
            }
        }
    }
}
