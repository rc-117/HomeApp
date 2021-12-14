namespace Homeapp.Backend.Managers
{
    using Homeapp.Backend.Identity;
    using Newtonsoft.Json.Linq;
    using System;

    /// <summary>
    /// The user data manager.
    /// </summary>
    public interface IUserDataManager
    {
        /// <summary>
        /// Creates a JObject from a user, exluding the user's password hash.
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public JObject CreatetUserJObjectFromUser(User user);


        /// <summary>
        /// Gets a user by its id.
        /// </summary>
        /// <param name="userId">The user id.</param>
        public User GetUserFromUserId(Guid userId);
    }
}