

namespace Homeapp.Backend.Managers
{
    using Homeapp.Backend.Db;
    using Homeapp.Backend.Entities;
    using Homeapp.Backend.Entities.Common.Requests;
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

        /// <summary>
        /// Returns a JSON object containing shared/allowed entities for an item. Used for response handling.
        /// </summary>
        /// <param name="id">The id to select the SharedEntities record.</param>
        public JObject GetSharedEntitiesJObjectFromId(Guid id)
        {
            var sharedEntities = this.appDbContext.SharedEntities.FirstOrDefault(s => s.Id == id);

            var readHouseholdArray = ConvertStringToJArrayOfNameGuidPairs(sharedEntities.ReadHouseholdIds, searchHouseholds: true);
            var readHouseholdGroupArray = ConvertStringToJArrayOfNameGuidPairs(sharedEntities.ReadHouseholdGroupIds, searchGroups: true);
            var readUserArray = ConvertStringToJArrayOfNameGuidPairs(sharedEntities.ReadUserIds, searchUsers: true);
            var editHouseholdArray = ConvertStringToJArrayOfNameGuidPairs(sharedEntities.EditHouseholdIds, searchHouseholds: true);
            var editHouseholdGroupArray = ConvertStringToJArrayOfNameGuidPairs(sharedEntities.EditHouseholdGroupIds, searchGroups: true);
            var editUserArray = ConvertStringToJArrayOfNameGuidPairs(sharedEntities.EditUserIds, searchUsers: true);

            return new JObject()
            {   
                { "ReadHousholds", readHouseholdArray },
                { "ReadHousholdGroups", readHouseholdGroupArray },
                { "ReadUsers", readUserArray },
                { "EditHousholds", editHouseholdArray},
                { "EditHousholdGroups", editHouseholdGroupArray},
                { "EditUsers", editUserArray }   
            };
        }

        #region Private helper methods
        /// <summary>
        /// Converts a semi colon seperated string into a JArray of name/guid pairs.
        /// </summary>
        /// <param name="guids">The string containing a list of guids separated with semicolon.</param>
        /// <param name="searchHouseholds">Set to true to search Households in the database.</param>
        /// <param name="searchGroups">Set to true to search Household groups in the database.</param>
        /// <param name="searchUsers">Set to true to search Users in the database.</param>
        private JArray ConvertStringToJArrayOfNameGuidPairs(
            string guids, 
            bool searchHouseholds = false, 
            bool searchGroups = false, 
            bool searchUsers = false)
        {
            if (string.IsNullOrWhiteSpace(guids))
            {
                return new JArray();
            }

            var jArray = new JArray();
            var nameList = new List<string>();
            var guidList = guids.Split(';');

            foreach (var guid in guidList)
            {
                if (string.IsNullOrWhiteSpace(guid))
                {
                    continue;
                }

                if (searchHouseholds)
                {
                    var id = Guid.Parse(guid);
                    var name = this.appDbContext.Households.FirstOrDefault(h => h.Id == id).Name;

                    jArray.Add(new JObject()
                    {
                        { "Id", id },
                        { "Name", name }
                    });
                }
                else if (searchGroups)
                {
                    var id = Guid.Parse(guid);
                    var name = this.appDbContext.HouseholdGroups.FirstOrDefault(h => h.Id == id).Name;

                    jArray.Add(new JObject()
                    {
                        { "Id", id },
                        { "Name", name }
                    });
                }
                else if (searchUsers)
                {
                    var id = Guid.Parse(guid);
                    var user = this.appDbContext.Users.FirstOrDefault(u => u.Id == id);

                    jArray.Add(OutputHandler.CreateUserJObject(user));
                }
            }

            return jArray;
        }
        #endregion
    }
}
