namespace Homeapp.Backend.Managers
{
    using Homeapp.Backend.Identity;
    using Homeapp.Backend.Identity.Requests;
    using Newtonsoft.Json.Linq;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    /// <summary>
    /// The user data manager.
    /// </summary>
    public interface IUserDataManager
    {
        /// <summary>
        /// Gets a user from the database using a given email and password combination.
        /// </summary>
        /// <param name="email">The email address.</param>
        /// <param name="password">The password hash.</param>
        /// <returns></returns>
        public User GetUserWithEmailAndPassword(string email, string password);

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

        /// <summary>
        /// Gets all users from a household using the household id.
        /// </summary>
        /// <param name="householdId">The household id</param>
        /// <returns>A list of users. Returns null if the household id does not exist, or there are no users.</returns>
        public List<User> GetUsersFromHousehold(Guid householdId);

        /// <summary>
        /// Gets all household groups from a household using the household id.
        /// </summary>
        /// <param name="householdId">The household id</param>
        /// <returns>A list of household groups. Returns null if the household id does not exist, or there are no users.</returns>
        public List<HouseholdGroup> GetGroupsFromHousehold(Guid householdId);

        /// <summary>
        /// Gets a household from the database using its id.
        /// </summary>
        /// <param name="householdId">The household id.</param>
        /// <returns>A household object. Returns null if nothing is found.</returns>
        public Household GetHouseholdWithId(Guid householdId);

        /// <summary>
        /// Gets a list of users from a household group.
        /// </summary>
        /// <param name="householdId">The household group id.</param>
        public List<User> GetHouseholdGroupUsers(Guid householdGroupId);

        /// <summary>
        /// Gets a specified user to a specified household group.
        /// </summary>
        /// <param name="householdId">The household id.</param>
        /// <param name="householdGroupId">The household group id.</param>
        /// <param name="userId">The user id.</param>
        public Task<UserHouseholdGroup> AddUserToHouseholdGroup
            (Guid householdId,
            Guid householdGroupId,
            Guid userId);

        /// <summary>
        /// Gets a household group from the database using its id.
        /// </summary>
        /// <param name="groupId">The household group id.</param>
        public HouseholdGroup GetHouseholdGroupWithId(Guid groupId);
    }
}