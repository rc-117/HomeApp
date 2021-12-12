namespace Homeapp.Backend.Controllers
{
    using Homeapp.Backend.Identity;
    using Homeapp.Backend.Tools;
    using Homeapp.Test;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Options;
    using Microsoft.IdentityModel.Tokens;
    using System;
    using System.IdentityModel.Tokens.Jwt;
    using System.Linq;
    using System.Net.Http.Headers;
    using System.Security.Claims;
    using System.Text;

    /// <summary>
    /// The user operation controller.
    /// </summary>
    [ApiController]
    public class UserOperationController : HomeappControllerBase
    {
        private JWTSettings jwtSettings;

        /// <summary>
        /// Initializes the UserOperationController
        /// </summary>
        public UserOperationController(IOptions<JWTSettings> jwtSettings)
        {
            this.jwtSettings = jwtSettings.Value;
        }

        /// <summary>
        /// Gets a JWT token for the user.
        /// </summary>
        [AllowAnonymous]
        [Route("api/Users/login")]
        public IActionResult LoginUser()
        {
            if (!Request.Headers.ContainsKey("Authorization"))
            {
                return BadRequest("Authorization header was not found.");
            }



            var authenticationHeaderValue = AuthenticationHeaderValue.Parse(Request.Headers["Authorization"]);
            var bytes = new byte[] { };
            if (Validation.StringIsBase64Compatible
                (authenticationHeaderValue.Parameter, 
                out byte[] byteArray))
            {
                bytes = byteArray;
            }
            else
            {
                return BadRequest("Invalid request.");
            }

            string[] credentials = Encoding.UTF8.GetString(bytes).Split(":");
            string email = credentials[0];
            string passwordHash = credentials[1];

            //Test user from dummy repo
            User user = TestRepo.Users.Where(u =>
                u.EmailAddress == email && u.PasswordHash == passwordHash)
                .FirstOrDefault();

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(this.jwtSettings.SecretKey); ;
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.EmailAddress),
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
                }),
                Expires = DateTime.UtcNow.AddMinutes(5),
                SigningCredentials = new SigningCredentials
                    (new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);

            var userWithToken = new UserWithToken(user);
            userWithToken.Token = tokenHandler.WriteToken(token);

            return Ok(userWithToken);
        }

    }
}
