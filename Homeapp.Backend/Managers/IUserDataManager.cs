namespace Homeapp.Backend.Managers
{
    using Homeapp.Backend.Entities;
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
        /// Creates and saves a household to the application database.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="allowedUsers">The users who will have read/write/full access to the household.</param>
        /// <param name="householdAddress">(Optional) The address of the household.</param>
        /// <param name="creator">(Optional) The user who created the household. If this is the very first request, 
        /// use the 'AssignHouseholdCreator' method following this to assign the newly created user as an owner.</param>
        /// <returns>The created household.</returns>
        public Task<Household> SaveHouseholdToDb(
            HouseholdRequest request,
            AllowedUsers allowedUsers,
            Address householdAddress = null,
            User creator = null);

        /// <summary>
        /// Creates and saves a User to the application database.
        /// </summary>
        /// <param name="request">The request.</param>
        public Task<User> SaveUserToDb(CreateUserRequest request);


        /// <summary>
        /// Assigns a creator to a household.
        /// </summary>
        /// <param name="creator">The creator to assign.</param>
        /// <param name="householdId">The id of the household to assign the creator to.</param>
        /// <remarks>Only use this method if there are no existing households in the database and this is the first 'create' request.</remarks>
        public void AssignHouseholdCreator(User creator, Guid householdId);

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

        /// <summary>
        /// Gets a login token for a user.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <returns>The JWT token.</returns>
        public string GetUserLoginToken(User user);
    }
}