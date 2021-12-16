namespace Homeapp.Backend.Controllers
{
    using Homeapp.Backend.Db;
    using Homeapp.Backend.Identity;
    using Homeapp.Backend.Identity.Requests;
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
    using System.Threading.Tasks;

    /// <summary>
    /// The user operation controller.
    /// </summary>
    [ApiController]
    public class UserOperationController : HomeappControllerBase
    {
        private JWTSettings jwtSettings;
        private AppDbContext appDbContext;

        /// <summary>
        /// Initializes the UserOperationController
        /// </summary>
        public UserOperationController
            (IOptions<JWTSettings> jwtSettings,
            AppDbContext appDbContext)
        {
            this.jwtSettings = jwtSettings.Value;
            this.appDbContext = appDbContext;
        }

        /// <summary>
        /// Gets a JWT token for the user.
        /// </summary>
        [AllowAnonymous]
        [HttpGet]
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

        /// <summary>
        /// Registers a new user into the application database.
        /// </summary>
        //[HttpPut]
        //[Route("api/Users/register")]
        //public Task<IActionResult> RegisterUser()
        //{
        // TO DO after making registerUserAndNewHouseHold action          
        //}

        /// <summary>
        /// Registers a new user into the application database.
        /// </summary>
        [HttpPut]
        [Route("api/Users/registerUserAndHousehold")]
        public Task<IActionResult> RegisterUserAndHousehold([FromBody]CreateUserAndHouseholdRequest request)
        {
            
        }

        #region Private helper methods
        private bool NoUsersOrHouseHoldsExist()
        {
            return this.appDbContext.Users.Count() < 1 || 
                this.appDbContext.Households.Count() < 1; 
        }
        #endregion
    }
}