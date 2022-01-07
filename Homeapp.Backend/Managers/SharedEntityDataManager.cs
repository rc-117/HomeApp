

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
    public class SharedEntityDataManager : ISharedEntityDataManager
    {
        private AppDbContext appDbContext;

        /// <summary>
        /// Initializes an instance of the Shared Entity Data Manager.
        /// </summary>
        /// <param name="appDbContext">The application database context.</param>
        public SharedEntityDataManager(AppDbContext appDbContext)
        {
            this.appDbContext = appDbContext;
        }

        /// <summary>
        /// Creates a new SharedEntities object from a request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <remarks>This does not persist the sharedentities object to the database. This only creates an instance of a SharedEntities object.
        /// This method is to be used when creating other entities that require SharedEntities, and will the SharedEntities object is
        /// to be persisted to the database through other data managers on entity creation.</remarks>
        public SharedEntities CreateNewSharedEntitiesObject(SharedEntitiesRequest request)
        {
            var sharedEntities = new SharedEntities
            {
                ReadHouseholdIds = OutputHandler.ConvertStringArrayToString(request.ReadHouseholdIds),
                ReadHouseholdGroupIds = OutputHandler.ConvertStringArrayToString(request.ReadHouseholdGroupIds),
                ReadUserIds = OutputHandler.ConvertStringArrayToString(request.ReadUserIds),
                EditHouseholdIds = OutputHandler.ConvertStringArrayToString(request.EditHouseholdIds),
                EditHouseholdGroupIds = OutputHandler.ConvertStringArrayToString(request.EditHouseholdGroupIds),
                EditUserIds = OutputHandler.ConvertStringArrayToString(request.EditUserIds)
            };

            return sharedEntities;
        }

        /// <summary>
        /// Gets a SharedEntities object by its id.
        /// </summary>
        /// <param name="id">The id to select the SharedEntities record.</param>
        public SharedEntities GetSharedEntitiesObjectFromId(Guid id)
        {
            return this.appDbContext.SharedEntities.FirstOrDefault(s => s.Id == id);
        }
    }
}
