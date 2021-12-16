namespace Homeapp.Backend.Managers
{
    using Homeapp.Backend.Identity;
    using Homeapp.Backend.Identity.Requests;
    using Newtonsoft.Json.Linq;
    using System;
    using System.Threading.Tasks;

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

        /// <summary>
        /// Creates and saves a User and Household to the application database.
        /// </summary>
        /// <param name="request"></param>
        public Task<string> SaveUserAndHouseholdToDb(CreateUserAndHouseholdRequest request);

        /// <summary>
        /// Creates and saves a User to the application database.
        /// </summary>
        /// <param name="request">The incoming request.</param>
        public Task<string> SaveUserToDb(CreateUserRequest request);
    }
}