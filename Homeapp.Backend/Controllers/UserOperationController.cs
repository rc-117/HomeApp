namespace Homeapp.Backend.Controllers
{
    using Homeapp.Backend.Db;
    using Homeapp.Backend.Identity;
    using Homeapp.Backend.Identity.Requests;
    using Homeapp.Backend.Managers;
    using Homeapp.Backend.Tools;
    using Homeapp.Test;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Options;
    using Microsoft.IdentityModel.Tokens;
    using Newtonsoft.Json.Linq;
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
        private IUserDataManager userDataManager;

        /// <summary>
        /// Initializes the UserOperationController
        /// </summary>
        public UserOperationController
            (IOptions<JWTSettings> jwtSettings,
            AppDbContext appDbContext,
            IUserDataManager userDataManager)
        {
            this.jwtSettings = jwtSettings.Value;
            this.appDbContext = appDbContext;
            this.userDataManager = userDataManager;
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

            if (!Validation.EmailIsAlreadyInUse(email, this.appDbContext))
            {
                return NotFound("Account not found.");
            }

            if (!Validation.UserEmailPasswordComboIsValid
                (email, passwordHash, this.appDbContext))
            {
                return Unauthorized("Invalid username or password.");
            }

            var user = this.userDataManager.GetUserWithEmailAndPassword(email, passwordHash);

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(this.jwtSettings.SecretKey); ;
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Email, user.EmailAddress),
                    new Claim(ClaimTypes.GivenName, user.FirstName),
                    new Claim(ClaimTypes.Surname, user.LastName),
                    new Claim(ClaimTypes.Gender, Enum.GetName(typeof(Gender), user.Gender))
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
        /// Registers a new user into the application database and assigns them to an existing household.
        /// </summary>
        /// <param name="request">The incomg request.</param>
        [AllowAnonymous]
        [HttpPut]
        [Route("api/Users/register")]
        public async Task<IActionResult> RegisterUser([FromBody]CreateUserRequest request)
        {
            if (NoUsersOrHouseHoldsExist())
            {
                return BadRequest
                    ("No households exist to register a user into. Please use method 'RegisterUserAndHousehold' to register both a new user and household.");
            }

            if (Validation.EmailIsAlreadyInUse(request.EmailAddress, this.appDbContext))
            {
                return BadRequest($"Email '{request.EmailAddress}' is already in use.");
            }

            if (!Validation.HouseholdExists(request.RequestedHouseholdId, this.appDbContext))
            {
                return NotFound($"Household with id '{request.RequestedHouseholdId}' was not found.");
            }

            if (!Validation.RequestedHouseholdPasswordIsValid
                (request.RequestedHouseholdId,
                request.RequestedHousholdPasswordHash,
                this.appDbContext))
            {
                return Unauthorized($"Invalid household password.");
            }

            if (!Validation.DateIntArrayIsValid(request.Birthday))
            {
                return BadRequest("Invalid date value. Integer array must be in m/d/yyyy format.");
            }

            if (!Validation.BirthdayIsValid(this.GetDateFromIntArray(request.Birthday)))
            {
                return BadRequest("Invalid birthday received.");
            }

            var result = await this.userDataManager.SaveUserToDb(request);

            if (result == null)
            {
                return StatusCode(500, "An error occured while saving the user to the database.");
            }
            
            return Ok(result);
        }

        /// <summary>
        /// Registers a new user into the application database.
        /// </summary>
        /// <param name="request">The incoming request.</param>
        [AllowAnonymous]
        [HttpPut]
        [Route("api/Users/registerUserAndHousehold")]
        public async Task<IActionResult> RegisterUserAndHousehold([FromBody]CreateUserAndHouseholdRequest request)
        {
            if(Validation.EmailIsAlreadyInUse(request.UserRequest.EmailAddress, this.appDbContext))
            {
                return BadRequest($"Email '{request.UserRequest.EmailAddress}' is already in use.");
            }

            if (!Validation.DateIntArrayIsValid(request.UserRequest.Birthday))
            {
                return BadRequest("Invalid date value. Integer array must be in m/d/yyyy format.");
            }

            if (!Validation.BirthdayIsValid(this.GetDateFromIntArray(request.UserRequest.Birthday)))
            {
                return BadRequest("Invalid birthday received.");
            }

            var result = await this.userDataManager.SaveUserAndHouseholdToDb(request);

            if (result == null)
            {
                return StatusCode(500, "Error saving new user and household to database.");
            }

            return Ok(result.ToString());
        }

        /// <summary>
        /// Gets all users and groups from within a household.
        /// </summary>
        /// <param name="householdId">The household id, passed in from the route.</param>
        /// <param name="householdGroupId">The household group id, passed in from the route.</param>
        [HttpGet]
        [Route("api/Households/householdId/{householdId}/householdGroupId/{householdGroupId}/GetUsersAndGroups")]
        public IActionResult GetUsersAndGroupsFromHousehold(string householdId, string householdGroupId)
        {
            var householdGuid = Guid.TryParse(householdId, out Guid guid) == true ? guid : Guid.Empty;
            var householdGroupGuid = Guid.TryParse(householdGroupId, out Guid groupGuid) == true ? groupGuid : Guid.Empty;

            if (householdGuid == Guid.Empty)
            {
                return BadRequest("Invalid household Id.");
            }
            else if (householdGroupGuid == Guid.Empty)
            {
                return BadRequest("Invalid household group Id.");
            }

            if (this.userDataManager.GetHouseholdWithId(householdGuid) == null)
            {
                return NotFound($"Household with id '{householdGuid}' was not found.");
            }//write code in the user data manager to check if household group exists
            

            var groups = this.userDataManager.GetGroupsFromHousehold(householdGuid);

            var userJArray = new JArray();
            var groupJArray = new JArray();

            foreach (var group in groups)
            {
                var userIds = "";

                if (this.userDataManager.GetHouseholdGroupUsers(group.Id) != null)
                {
                    foreach (var user in this.userDataManager.GetHouseholdGroupUsers(group.Id))
                    {
                        userIds += $"{user.Id};";
                    }
                }                

                groupJArray.Add(new JObject()
                {
                    { "Id", group.Id },
                    { "Name", group.Name },
                    { "Users", userIds },
                });
            }

            foreach (var user in this.userDataManager.GetUsersFromHousehold(householdGuid))
            {
                userJArray.Add(new JObject()
                {
                    { "Id", user.Id },
                    { "FirstName", user.FirstName },
                    { "LastName", user.LastName },
                    { "Email", user.EmailAddress },
                });
            }

            return Ok(new JObject()
            {
                { "Groups", groupJArray },
                { "Users", userJArray }
            }.ToString());
        }

        /// <summary>
        /// Adds a specified user to a specified household group.
        /// </summary>
        /// <param name="householdId">The household id, passed in from the route.</param>
        /// <param name="householdGroupId">The household group id, passed in from the route.</param>
        /// <param name="userId">The id of the user to add to the household group.</param>
        [HttpPut]
        [Route("api/Households/householdId/{householdId}/householdGroupId/{householdGroupId}/userId/{userId}/AddUser")]
        public async Task<IActionResult> AddUserToHouseholdGroup
            (string householdId, 
            string householdGroupId,
            string userId)
        {
            var householdGuid = Guid.TryParse(householdId, out Guid guid) == true ? guid : Guid.Empty;
            var householdGroupGuid = Guid.TryParse(householdGroupId, out Guid groupGuid) == true ? groupGuid : Guid.Empty;
            var userGuid = Guid.TryParse(userId, out Guid userIdGuid) == true ? userIdGuid : Guid.Empty;

            if (householdGuid == Guid.Empty)
            {
                return BadRequest("Invalid household Id.");
            }
            else if (householdGroupGuid == Guid.Empty)
            {
                return BadRequest("Invalid household group Id.");
            }
            else if (userGuid == Guid.Empty)
            {
                return BadRequest("Invalid user Id.");
            }

            if (!Validation.HouseholdExists(householdGuid, this.appDbContext))
            {
                return NotFound($"Houshold with id '{householdGuid}' was not found.");
            }
            else if (!Validation.HouseholdGroupExists(householdGroupGuid, this.appDbContext))
            {
                return NotFound($"Houshold group with id '{householdGroupGuid}' was not found.");
            }
            else if (!Validation.UserExists(userGuid, this.appDbContext))
            {
                return NotFound($"User with id '{userGuid}' was not found.");
            }

            if (!Validation.GroupIsInHousehold(householdGroupGuid, householdGuid, this.appDbContext))
            {
                return NotFound($"Group with id '{householdGroupGuid}' does not exist in household with id '{householdGuid}'");
            }

            if (!Validation.UserIsInHousehold(this.GetUserId(), householdGuid, this.appDbContext))
            {
                return Unauthorized($"Requesting user is unauthorized to make changes to household: '{householdGuid}'");
            }
            else if (!Validation.UserIsInHousehold(userGuid, householdGuid, this.appDbContext))
            {
                return Unauthorized($"User with id '{userGuid}' is not in in household: '{householdGuid}'. Request denied.");
            }

            var result = await this.userDataManager.AddUserToHouseholdGroup(householdGuid, householdGroupGuid, userGuid);

            return Ok();
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