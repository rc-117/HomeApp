﻿namespace Homeapp.Backend.Managers
{
    using Homeapp.Backend.Db;
    using Homeapp.Backend.Identity;
    using Homeapp.Backend.Identity.Requests;
    using Homeapp.Test;
    using Newtonsoft.Json.Linq;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    /// <summary>
    /// The user data manager.
    /// </summary>
    public class UserDataManager : HomeappDataManagerBase, IUserDataManager
    {
        private AppDbContext appDbContext;

        /// <summary>
        /// Initializes the user data manager class.
        /// </summary>
        public UserDataManager(AppDbContext appDbContext)
        {
            this.appDbContext = appDbContext;
        }

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
        /// Creates a JObject from a user, exluding the user's password hash.
        /// </summary>
        /// <param name="user">The user.</param>
        public JObject CreatetUserJObjectFromUser(User user)
        {
            return new JObject
            {
                { "Id", user.Id },
                { "Email", user.EmailAddress },
                { "FirstName", user.FirstName },
                { "LastName", user.LastName }
            };
        }

        /// <summary>
        /// Gets a user by its id.
        /// </summary>
        /// <param name="userId">The user id.</param>
        public User GetUserFromUserId(Guid userId)
        {
            //static repo code
            return TestRepo.Users
                .FirstOrDefault(u => u.Id == userId);
        }

        /// <summary>
        /// Creates and saves a User and Household to the application database.
        /// </summary>
        /// <param name="request">The incoming request.</param>
        public async Task<string> SaveUserAndHouseholdToDb(CreateUserAndHouseholdRequest request)
        {
            var household = new Household()
            {
                Name = request.HouseholdRequest.Name,
                HouseholdGroups = null,
                PasswordHash = request.HouseholdRequest.PasswordHash
            };

            var householdGroups = new List<HouseholdGroup>();

            foreach (var householdGroup in request.HouseholdRequest.HouseholdGroupRequests)
            {
                householdGroups.Add(new HouseholdGroup()
                {
                    Name = householdGroup.Name,
                    Household = household
                }); 
            }

            var user = new User
            {
                EmailAddress = request.UserRequest.EmailAddress,
                PasswordHash = request.UserRequest.PasswordHash,
                FirstName = request.UserRequest.FirstName,
                LastName = request.UserRequest.LastName,
                Birthday = this.GetDateFromIntArray(request.UserRequest.Birthday),
                Gender = request.UserRequest.Gender,
                HouseholdGroups = null
            };

            //Joins
            
            var userHousehold = new List<UserHousehold>()
            {
                new UserHousehold()
                {
                    User = user,
                    Household = household
                }
            };

            var userHouseholdGroup = new UserHouseholdGroup();

            if (householdGroups.Count > 1)
            {
                user.HouseholdGroups = new List<UserHouseholdGroup>();

                for (int i = 0; i < request.HouseholdRequest.HouseholdGroupRequests.Count(); i++)
                {
                    if (request.HouseholdRequest.HouseholdGroupRequests[i].AddRequestingUserToGroup)
                    {
                        userHouseholdGroup= new UserHouseholdGroup()
                        {
                            HouseholdGroup = householdGroups[i],
                            User = user,
                        };

                        householdGroups[i].Users = new List<UserHouseholdGroup>();
                        householdGroups[i].Users.Add(userHouseholdGroup);
                        user.HouseholdGroups.Add(userHouseholdGroup);
                    }
                }
            }

            user.Households = userHousehold;
            
            household.Users = userHousehold;
            household.HouseholdGroups = householdGroups.Count < 1 ? 
                null : householdGroups;

            this.appDbContext.Households.Add(household);

            if (householdGroups != null)
            {
                foreach (var householdGroup in householdGroups)
                {
                    this.appDbContext.HouseholdGroups.Add(householdGroup);
                }
            }
            
            this.appDbContext.Users.Add(user);

            try
            {
                await this.appDbContext.SaveChangesAsync();
                
                var householdGroupArray = householdGroups == null ? 
                    null : CreateHouseholdGroupJArrayResponse(householdGroups);


                return new JObject
                {
                    { "Household", new JObject()
                        {
                            { "Name", request.HouseholdRequest.Name },
                            { "HouseholdGroups", householdGroupArray }
                        }
                    },
                    { "User", new JObject()
                        {
                            { "Name", string.Format
                                ("{0} {1}",
                                user.FirstName,
                                user.LastName)
                            },
                            { "Gender", Enum.GetName(typeof(Gender), user.Gender) },
                            { "Birthday", user.Birthday.ToLongDateString() },
                            { "EmailAddress", user.EmailAddress }
                        }
                    }                    
                }.ToString();
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// Creates and saves a User to the application database.
        /// </summary>
        /// <param name="request">The incoming request.</param>
        public async Task<string> SaveUserToDb(CreateUserRequest request)
        {
            var user = new User()
            {
                EmailAddress = request.EmailAddress,
                PasswordHash = request.PasswordHash,
                FirstName = request.FirstName,
                LastName = request.LastName,
                Birthday = this.GetDateFromIntArray(request.Birthday),
                Gender = request.Gender,
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

                return new JObject()
                {
                    { "Id", user.Id },
                    { "Name", $"{user.FirstName} {user.LastName}" },
                    { "Birthday", user.Birthday.ToLongDateString() },
                    { "EmailAddress", $"{user.EmailAddress}" }
                }.ToString();
            }
            catch (Exception)
            {
                return null;
            }
        }

        #region helper methods
        private JArray CreateHouseholdGroupJArrayResponse(List<HouseholdGroup> groups)
        {
            var array = new JArray();

            foreach (var group in groups)
            {
                array.Add(new JObject()
                {
                    { "Name", group.Name }
                });
            }

            return array;
        }
        #endregion
    }
}
