namespace Homeapp.Backend.Managers
{
    using Homeapp.Backend.Db;
    using Homeapp.Backend.Entities;
    using Homeapp.Backend.Identity;
    using Homeapp.Backend.Tools;
    using Newtonsoft.Json.Linq;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Threading.Tasks;
    using System.Web.Http;

    /// <summary>
    /// The shared entity data manager.
    /// </summary>
    public class AllowedUsersDataManager : IAllowedUsersDataManager
    {
        private AppDbContext appDbContext;

        /// <summary>
        /// Initializes an instance of the Shared Entity Data Manager.
        /// </summary>
        /// <param name="appDbContext">The application database context.</param>
        public AllowedUsersDataManager(AppDbContext appDbContext)
        {
            this.appDbContext = appDbContext;
        }

        /// <summary>
        /// Creates an empty AllowedUsers object.
        /// </summary>
        public AllowedUsers CreateNewEmptyAllowedUsersObject()
        {
            return new AllowedUsers()
            {
                ReadHouseholdIds = "",
                ReadHouseholdGroupIds = "",
                ReadUserIds = "",
                WriteHouseholdIds = "",
                WriteHouseholdGroupIds = "",
                WriteUserIds = "",
                FullAccessHouseholdIds = "",
                FullAccessHouseholdGroupIds = "",
                FullAccessUserIds = ""
            };
        }

        /// <summary>
        /// Creates a new SharedEntities object from a request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <remarks>This does not persist the sharedentities object to the database. This only creates an instance of a SharedEntities object.
        /// This method is to be used when creating other entities that require SharedEntities, and will the SharedEntities object is
        /// to be persisted to the database through other data managers on entity creation.</remarks>
        public AllowedUsers CreateNewAllowedUsersObject(AllowedUsersRequest request)
        {
            var allowedUsers = new AllowedUsers
            {
                ReadHouseholdIds = OutputHandler.ConvertStringArrayToString(request.ReadHouseholdIds),
                ReadHouseholdGroupIds = OutputHandler.ConvertStringArrayToString(request.ReadHouseholdGroupIds),
                ReadUserIds = OutputHandler.ConvertStringArrayToString(request.ReadUserIds),
                WriteHouseholdIds = OutputHandler.ConvertStringArrayToString(request.WriteHouseholdIds),
                WriteHouseholdGroupIds = OutputHandler.ConvertStringArrayToString(request.WriteHouseholdGroupIds),
                WriteUserIds = OutputHandler.ConvertStringArrayToString(request.WriteUserIds),
                FullAccessHouseholdIds = OutputHandler.ConvertStringArrayToString(request.FullAccessHouseholdIds),
                FullAccessHouseholdGroupIds = OutputHandler.ConvertStringArrayToString(request.FullAccessHouseholdGroupIds),
                FullAccessUserIds = OutputHandler.ConvertStringArrayToString(request.FullAccessUserIds),
            };

            return allowedUsers;
        }

        /// <summary>
        /// Gets a SharedEntities object by its id.
        /// </summary>
        /// <param name="id">The id to select the SharedEntities record.</param>
        public AllowedUsers GetAllowedUsersObjectFromId(Guid id)
        {
            return this.appDbContext.SharedEntities.FirstOrDefault(s => s.Id == id);
        }

        /// <summary>
        /// Creates a copy of an AllowedUsers object.
        /// </summary>
        /// <param name="originalCopy">The original copy.</param>
        public AllowedUsers CreateAllowedUsersCopy(AllowedUsers originalCopy)
        {
            return new AllowedUsers()
            {
                ReadHouseholdIds = originalCopy.ReadHouseholdIds,
                ReadHouseholdGroupIds = originalCopy.ReadHouseholdGroupIds,
                ReadUserIds = originalCopy.ReadUserIds,
                WriteHouseholdIds = originalCopy.WriteHouseholdIds,
                WriteHouseholdGroupIds = originalCopy.WriteHouseholdGroupIds,
                WriteUserIds = originalCopy.WriteUserIds,
                FullAccessHouseholdIds = originalCopy.FullAccessHouseholdIds,
                FullAccessHouseholdGroupIds = originalCopy.FullAccessHouseholdGroupIds,
                FullAccessUserIds = originalCopy.FullAccessUserIds
            };
        }
    }
}
