namespace Homeapp.Backend.Managers
{
    using Homeapp.Backend.Db;
    using Homeapp.Backend.Entities;
    using Homeapp.Backend.Identity;
    using Homeapp.Backend.Identity.Requests;
    using Homeapp.Backend.Tools;
    using Homeapp.Test;
    using Microsoft.Extensions.Options;
    using Microsoft.IdentityModel.Tokens;
    using Newtonsoft.Json.Linq;
    using System;
    using System.Collections.Generic;
    using System.IdentityModel.Tokens.Jwt;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Security.Claims;
    using System.Text;
    using System.Threading.Tasks;
    using System.Web.Http;

    /// <summary>
    /// The user data manager.
    /// </summary>
    public class UserDataManager : HomeappDataManagerBase, IUserDataManager
    {
        private AppDbContext appDbContext;
        private ICommonDataManager commonDataManager;
        private JWTSettings jwtSettings;

        /// <summary>
        /// Initializes the user data manager class.
        /// </summary>
        /// <param name="commonDataManager">The common data manager.</param>
        /// <param name="appDbContext">The application database context.</param>
        public UserDataManager(
            ICommonDataManager commonDataManager, 
            AppDbContext appDbContext, 
            IOptions<JWTSettings> jwtSettings)
        {
            this.appDbContext = appDbContext;
            this.commonDataManager = commonDataManager;
            this.jwtSettings = jwtSettings.Value;
        }

        #region Get
        /// <summary>
        /// Gets a user from the database using a given email and password combination.
        /// </summary>
        /// <param name="email">The email address.</param>
        /// <param name="password">The password hash.</param>
        /// <returns></returns>
        public User GetUserWithEmailAndPassword(string email, string password)
        {
            return this.appDbContext.Users.Where(u =>
                u.EmailAddress == email && u.PasswordHash == password)
                .FirstOrDefault();
        }

        /// <summary>
        /// Gets a user by its id.
        /// </summary>
        /// <param name="userId">The user id.</param>
        public User GetUserFromUserId(Guid userId)
        {
            try
            {
                var user = this.appDbContext.Users.FirstOrDefault(u => u.Id == userId);
                user.Households = this.GetUserhouseholdsByUserId(user.Id);
                user.HouseholdGroups = this.GetUserhouseholdsGroupByUserId(user.Id);

                return user;
            }
            catch (Exception)
            {
                throw new HttpResponseException(
                   new HttpResponseMessage(HttpStatusCode.InternalServerError)
                   {
                       Content = new StringContent("There was an error when retrieving a user from the database. Please try the request again."),
                       ReasonPhrase = HttpReasonPhrase
                           .GetPhrase(ReasonPhrase.ErrorRetrievingFromDatabase)
                   });
            }
        }

        /// <summary>
        /// Gets all household groups from a household using the household id.
        /// </summary>
        /// <param name="householdId">The household id</param>
        /// <returns>A list of household groups. Returns null if the household id does not exist, or there are no users.</returns>
        public List<HouseholdGroup> GetGroupsFromHousehold(Guid householdId)
        {
            var householdGroups =
                this.appDbContext
                .HouseholdGroups
                .Where(h => h.HouseholdId == householdId)
                .ToList();

            return householdGroups.Count == 0 ? null : householdGroups;
        }

        /// <summary>
        /// Gets all users from a household using the household id.
        /// </summary>
        /// <param name="householdId">The household id</param>
        /// <returns>A list of users. Returns null if the household id does not exist, or there are no users.</returns>
        public List<User> GetUsersFromHousehold(Guid householdId)
        {
            var usersHouseholds =
                this.appDbContext
                .UserHouseholds
                .Where(uh => uh.HouseholdId == householdId)
                .ToList();

            if (usersHouseholds.Count == 0)
            {
                return null;
            }

            var users = new List<User>();

            foreach (var join in usersHouseholds)
            {
                users.Add(this.appDbContext.Users.FirstOrDefault(u => u.Id == join.UserId));
            }

            return users.Count == 0 ? null : users;
        }

        /// <summary>
        /// Gets a household from the database using its id.
        /// </summary>
        /// <param name="householdId">The household id.</param>
        /// <returns>A household object. Returns null if nothing is found.</returns>
        public Household GetHouseholdWithId(Guid householdId)
        {
            return this.appDbContext
                .Households
                .FirstOrDefault(h => h.Id == householdId);
        }

        /// <summary>
        /// Gets a household group from the database using its id.
        /// </summary>
        /// <param name="groupId">The household group id.</param>
        public HouseholdGroup GetHouseholdGroupWithId(Guid groupId)
        {
            return this.appDbContext
                .HouseholdGroups
                .FirstOrDefault(h => h.Id == groupId);
        }

        /// <summary>
        /// Gets a list of users from a household group.
        /// </summary>
        /// <param name="householdId">The household group id.</param>
        public List<User> GetHouseholdGroupUsers(Guid householdGroupId)
        {
            var joins = this.appDbContext
                .UserHouseholdGroups
                .Where(h => h.HouseholdGroupId == householdGroupId).ToList();

            var users = new List<User>();

            if (joins.Count == 0)
            {
                return null;
            }

            foreach (var join in joins)
            {
                users.Add(
                    this.appDbContext
                    .Users
                    .FirstOrDefault(u => u.Id == join.UserId));
            }

            return users.Count() == 0 ? null : users;
        }
        #endregion

        #region Save new
        /// <summary>
        /// Creates and saves a household to the application database.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="allowedUsers">The users who will have read/write/full access to the household.</param>
        /// <param name="householdAddress">(Optional) The address of the household.</param>
        /// <param name="creator">(Optional) The user who created the household. If this is the very first request, 
        /// use the 'AssignHouseholdCreator' method following this to assign the newly created user as an owner.</param>
        /// <returns>The created household.</returns>
        public async Task<Household> SaveHouseholdToDb(
            HouseholdRequest request,
            AllowedUsers allowedUsers,
            Address householdAddress = null,
            User creator = null)
        {
            var household = new Household()
            {
                Name = request.Name,
                HouseholdGroups = new List<HouseholdGroup>(),
                PasswordHash = request.PasswordHash,
                Users = null,
                Address = householdAddress != null ? householdAddress : null,
                PhoneNumber = !string.IsNullOrWhiteSpace(request.PhoneNumber) ?
                    request.PhoneNumber : null,
                Creator = creator == null ? null : creator,
                AllowedUsers = allowedUsers,
                DateTimeCreated = DateTime.Now
            };

            try
            {
                this.appDbContext.Households.Add(household);

                await this.appDbContext.SaveChangesAsync();

                if (request.HouseholdGroupRequests.Length == 0)
                {
                    return household;
                } 
            }
            catch (Exception)
            {
                throw new HttpResponseException(
                    new HttpResponseMessage(HttpStatusCode.InternalServerError)
                    {
                        Content = new StringContent("There was an error when saving the household to the database. Please try again."),
                        ReasonPhrase = HttpReasonPhrase
                            .GetPhrase(ReasonPhrase.ErrorSavingToDatabase)
                    });
            }

            foreach (var groupRequest in request.HouseholdGroupRequests)
            {
                var groupAllowedUsers =
                    await this.commonDataManager.SaveAllowedUsersObjectToDb(groupRequest.AllowedUsers);

                household.HouseholdGroups.Add(
                    await this.SaveHouseholdGroupToDb(
                        request: groupRequest, 
                        household: household, 
                        creator: creator, 
                        allowedUsers: groupAllowedUsers));
            }

            try
            {
                this.appDbContext.Households.Update(household);
                await this.appDbContext.SaveChangesAsync();

                return household;
            }
            catch (Exception)
            {
                throw new HttpResponseException(
                    new HttpResponseMessage(HttpStatusCode.InternalServerError)
                    {
                        Content = new StringContent("There was an error when saving the household to the database. Please try again."),
                        ReasonPhrase = HttpReasonPhrase
                            .GetPhrase(ReasonPhrase.ErrorSavingToDatabase)
                    });
            }
        }

        /// <summary>
        /// Creates and saves a new household group to the database.
        /// </summary>
        /// <param name="request">The household group request.</param>
        /// <param name="household">The household to add the group to.</param>
        /// <param name="creator">The user who created the household group.</param>
        /// <param name="allowedUsers">A list of users who will have read/write/full access over this group.</param>
        /// <param name="members">(Optional) List of users to add to the group.</param>
        /// <returns>The created household group.</returns>
        public async Task<HouseholdGroup> SaveHouseholdGroupToDb(
            HouseholdGroupRequest request,
            Household household,
            User creator,
            AllowedUsers allowedUsers)
        {
            var householdGroup = new HouseholdGroup()
            {
                Name = request.Name,
                Household = household,
                Creator = creator,
                DateTimeCreated = DateTime.Now,
                AllowedUsers = allowedUsers
            };

            try
            {
                this.appDbContext.HouseholdGroups.Add(householdGroup);
                await this.appDbContext.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw new HttpResponseException(
                    new HttpResponseMessage(HttpStatusCode.InternalServerError)
                    {
                        Content = new StringContent("There was an error when saving a household group to the database. Please try again."),
                        ReasonPhrase = HttpReasonPhrase
                            .GetPhrase(ReasonPhrase.ErrorSavingToDatabase)
                    });
            }

            var members = new List<User>();

            if (request.UserIds.Length > 0)
            {
                foreach (var id in request.UserIds)
                {
                    members.Add(
                        this.GetUserFromUserId(Guid.Parse(id)));
                }
            }

            if (request.AddRequestingUserToGroup)
            {
                if (members.FirstOrDefault(u => u.Id == creator.Id) == null)
                {
                    members.Add(creator);
                }
            }

            if (members.Count > 0)
            {
                return await
                    this.AddMembersToHouseholdGroup(
                        members: members,
                        householdGroup: householdGroup,
                        allowedUsers: householdGroup.AllowedUsers);
            }
            else
            {
                return householdGroup;
            }
        }

        /// <summary>
        /// Creates and saves a User to the application database.
        /// </summary>
        /// <param name="request">The incoming request.</param>
        public async Task<User> SaveUserToDb(CreateUserRequest request)
        {
            var user = new User()
            {
                EmailAddress = request.EmailAddress,
                PhoneNumber = request.PhoneNumber,
                PasswordHash = request.PasswordHash,
                FirstName = request.FirstName,
                LastName = request.LastName,
                Birthday = DateTime.Parse(request.Birthday),
                Gender = (Gender)Enum.Parse(typeof(Gender), request.Gender),
                Households = new List<UserHousehold>()
            };

            var userHoushold = new UserHousehold()
            {
                Household = this.appDbContext.Households
                .FirstOrDefault(h => h.Id == request.RequestedHouseholdId),
                User = user
            };

            user.Households.Add(userHoushold);

            try
            {
                this.appDbContext.Users.Add(user);
                this.appDbContext.Households
                    .FirstOrDefault(h => h.Id == request.RequestedHouseholdId)
                    .Users
                    .Add(userHoushold);

                await this.appDbContext.SaveChangesAsync();

                return user;
            }
            catch (Exception)
            {
                throw new HttpResponseException(
                    new HttpResponseMessage(HttpStatusCode.InternalServerError)
                    {
                        Content = new StringContent("There was an error when saving the user to the database. Please try again."),
                        ReasonPhrase = HttpReasonPhrase
                            .GetPhrase(ReasonPhrase.ErrorSavingToDatabase)
                    });
            }
        }
        #endregion

        #region Add to
        /// <summary>
        /// Assigns a specified list of users as new members to a household group.
        /// </summary>
        /// <param name="members">The members to assign to the group.</param>
        /// <param name="householdGroup">The group to assign members to.</param>
        /// <param name="allowedUsers">The 'AllowedUsers' record associated with the household group.</param>
        public async Task<HouseholdGroup> AddMembersToHouseholdGroup(
            List<User> members,
            HouseholdGroup householdGroup,
            AllowedUsers allowedUsers)
        {
            var userHouseholdGroups = new List<UserHouseholdGroup>();

            foreach (var member in members)
            {
                var newMember = new UserHouseholdGroup()
                {
                    User = member,
                    HouseholdGroup = householdGroup
                };

                if (householdGroup.Members == null)
                {
                    userHouseholdGroups.Add(newMember);
                }
                else
                {
                    householdGroup.Members.Add(newMember);
                }
            }

            if (householdGroup.Members == null)
            {
                householdGroup.Members = userHouseholdGroups;
            }

            this.EnsureMembersHaveMinimumReadAccess(
                members: members,
                allowedUsers: allowedUsers);

            try
            {
                this.appDbContext.HouseholdGroups.Update(householdGroup);
                await this.appDbContext.SaveChangesAsync();

                return householdGroup;
            }
            catch (Exception)
            {
                throw new HttpResponseException(
                    new HttpResponseMessage(HttpStatusCode.InternalServerError)
                    {
                        Content = new StringContent("There was an error when adding a member to a household group. Please try again."),
                        ReasonPhrase = HttpReasonPhrase
                            .GetPhrase(ReasonPhrase.ErrorSavingToDatabase)
                    });
            }
        }

        /// <summary>
        /// Gets a specified user to a specified household group.
        /// </summary>
        /// <param name="householdId">The household id.</param>
        /// <param name="householdGroupId">The household group id.</param>
        /// <param name="userId">The user id.</param>
        public async Task<UserHouseholdGroup> AddUserToHouseholdGroup
            (Guid householdId,
            Guid householdGroupId,
            Guid userId)
        {
            var householdGroup =
                this.appDbContext
                .HouseholdGroups
                .FirstOrDefault(h => h.Id == householdGroupId);

            var user = this.appDbContext
                .Users
                .FirstOrDefault(u => u.Id == userId);

            var userHousholdGroup = new UserHouseholdGroup
            {
                HouseholdGroupId = householdGroup.Id,
                HouseholdGroup = householdGroup,
                User = user,
                UserId = user.Id
            };

            this.EnsureMembersHaveMinimumReadAccess(
                members: new List<User>() { this.GetUserFromUserId(userId) },
                allowedUsers: this.commonDataManager
                    .GetAllowedUsersObjectFromId(householdGroup.AllowedUsersId));

            try
            {
                this.appDbContext.UserHouseholdGroups.Add(userHousholdGroup);
                await this.appDbContext.SaveChangesAsync();

                return userHousholdGroup;
            }
            catch (Exception)
            {
                return null;
            }
        }
        #endregion

        /// <summary>
        /// Assigns a creator to a household.
        /// </summary>
        /// <param name="creator">The creator to assign.</param>
        /// <param name="householdId">The id of the household to assign the creator to.</param>
        /// <remarks>Only use this method if there are no existing households in the database and this is the first 'create' request.</remarks>
        public async void AssignHouseholdCreator(User creator, Guid householdId)
        {
            try
            {
                this.appDbContext
                    .Households
                    .FirstOrDefault(h => h.Id == householdId)
                    .Creator = creator;

                await this.appDbContext.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw new HttpResponseException(
                    new HttpResponseMessage(HttpStatusCode.InternalServerError)
                    {
                        Content = new StringContent("There was an error when editing an existing household in the database. Please try again."),
                        ReasonPhrase = HttpReasonPhrase
                            .GetPhrase(ReasonPhrase.ErrorSavingToDatabase)
                    });
            }
        }

        /// <summary>
        /// Gets a login token for a user.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <returns>The JWT token.</returns>
        public string GetUserLoginToken(User user)
        {
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
            return tokenHandler.WriteToken(token);
        }

        #region helper methods
        /// <summary>
        /// Gets a list of Userhousholds by user id.
        /// </summary>
        /// <param name="id">The user's id.</param>
        private List<UserHousehold> GetUserhouseholdsByUserId(Guid id)
        {
            return this.appDbContext.UserHouseholds.Where(u => id == u.UserId).ToList();
        }

        /// <summary>
        /// Gets a list of Userhoushold groups by user id.
        /// </summary>
        /// <param name="id">The user's id.</param>
        private List<UserHouseholdGroup> GetUserhouseholdsGroupByUserId(Guid id)
        {
            return this.appDbContext.UserHouseholdGroups.Where(u => id == u.UserId).ToList(); 
        }

        /// <summary>
        /// Ensures a list of users have (at minimum) read access to a resource.
        /// </summary>
        /// <param name="members">The list of users.</param>
        /// <param name="allowedUsers">The AllowedUsers record to check against.</param>
        private void EnsureMembersHaveMinimumReadAccess(List<User> members, AllowedUsers allowedUsers)
        {
            foreach (var member in members)
            {
                var userHasReadAccess =
                    this.commonDataManager.EntityIdIsInList(
                        idList: allowedUsers.ReadUserIds,
                        entityId: member.Id);

                var userHasWriteAccess =
                    this.commonDataManager.EntityIdIsInList(
                        idList: allowedUsers.WriteUserIds,
                        entityId: member.Id);

                var userHasFullAccess =
                    this.commonDataManager.EntityIdIsInList(
                        idList: allowedUsers.FullAccessUserIds,
                        entityId: member.Id);

                if (!userHasReadAccess && !userHasWriteAccess && !userHasFullAccess)
                {
                    this.commonDataManager
                        .AddUserToReadAccess(
                        userId: member.Id, 
                        allowedUsers: allowedUsers);
                }
            }
        }
        #endregion
    }
}