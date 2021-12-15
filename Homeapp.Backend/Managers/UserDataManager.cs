﻿namespace Homeapp.Backend.Managers
{
    using Homeapp.Backend.Identity;
    using Homeapp.Test;
    using Newtonsoft.Json.Linq;
    using System;
    using System.Linq;

    /// <summary>
    /// The user data manager.
    /// </summary>
    public class UserDataManager : IUserDataManager
    {
        /// <summary>
        /// Initializes the user data manager class.
        /// </summary>
        public UserDataManager()
        {

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
    }
}