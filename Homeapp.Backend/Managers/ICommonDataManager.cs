namespace Homeapp.Backend.Managers
{
    using Homeapp.Backend.Entities;
    using Newtonsoft.Json.Linq;
    using System;
    using System.Threading.Tasks;

    /// <summary>
    /// The common data manager interface.
    /// </summary>
    public interface ICommonDataManager
    {
        /// <summary>
        /// Creates and saves a new AllowedUsers record to the database.
        /// </summary>
        /// <param name="request">The request.</param>
        public Task<AllowedUsers> SaveAllowedUsersObjectToDb(AllowedUsersRequest request);

        /// <summary>
        /// Gets a SharedEntities object by its id.
        /// </summary>
        /// <param name="id">The id to select the SharedEntities record.</param>
        public AllowedUsers GetAllowedUsersObjectFromId(Guid id);

        /// <summary>
        /// Creates an empty AllowedUsers object.
        /// </summary>
        public AllowedUsers CreateNewEmptyAllowedUsersObject();

        /// <summary>
        /// Creates a copy of an AllowedUsers object.
        /// </summary>
        /// <param name="originalCopy">The original copy.</param>
        public AllowedUsers CreateAllowedUsersCopy(AllowedUsers originalCopy);

        /// <summary>
        /// Gets a recurring schedule object by its id.
        /// </summary>
        /// <param name="id">The unique id of the recurring schedule.</param>
        public RecurringSchedule GetRecurringScheduleById(Guid id);
    }
}
