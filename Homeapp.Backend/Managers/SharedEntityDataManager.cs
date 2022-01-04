

namespace Homeapp.Backend.Managers
{
    using Homeapp.Backend.Db;
    using Homeapp.Backend.Entities;
    using Homeapp.Backend.Entities.Common.Requests;
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
                ReadHouseholdIds = this.ConvertStringArrayToString(request.ReadHouseholdIds),
                ReadHouseholdGroupIds = this.ConvertStringArrayToString(request.ReadHouseholdGroupIds),
                ReadUserIds = this.ConvertStringArrayToString(request.ReadUserIds),
                EditHouseholdIds = this.ConvertStringArrayToString(request.EditHouseholdIds),
                EditHouseholdGroupIds = this.ConvertStringArrayToString(request.EditHouseholdGroupIds),
                EditUserIds = this.ConvertStringArrayToString(request.EditUserIds)
            };

            return sharedEntities;
        }

        /// <summary>
        /// Returns a JSON object containing shared/allowed entities for an item. Used for response handling.
        /// </summary>
        /// <param name="id">The id to select the SharedEntities record.</param>
        public JObject GetSharedEntitiesJObjectFromId(Guid id)
        {
            var sharedEntities = this.appDbContext.SharedEntities.FirstOrDefault(s => s.Id == id);

            var readHouseholdArray = ConvertStringToJArrayOfGuids(sharedEntities.ReadHouseholdIds);
            var readHouseholdGroupArray = ConvertStringToJArrayOfGuids(sharedEntities.ReadHouseholdGroupIds);
            var readUserArray = ConvertStringToJArrayOfGuids(sharedEntities.ReadUserIds);
            var editHouseholdArray = ConvertStringToJArrayOfGuids(sharedEntities.EditHouseholdIds);
            var editHouseholdGroupArray = ConvertStringToJArrayOfGuids(sharedEntities.EditHouseholdGroupIds);
            var editUserArray = ConvertStringToJArrayOfGuids(sharedEntities.EditUserIds);

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
        /// Converts an array of guids into a semi colon seperated string.
        /// </summary>
        /// <param name="guidList">The list of guids to convert.</param>
        private string ConvertGuidArrayToString(Guid[] guidList)
        {
            var stringList = "";

            foreach (var guid in guidList)
            {
                stringList += guid.ToString() + ";";
            }

            return stringList;
        }

        /// <summary>
        /// Converts a list of guids into a semi colon seperated string.
        /// </summary>
        /// <param name="guidList">The list of guids to convert.</param>
        private string ConvertGuidListToString(List<Guid> guidList)
        {
            var stringList = "";

            foreach (var guid in guidList)
            {
                stringList += guid.ToString() + ";";
            }

            return stringList;
        }

        /// <summary>
        /// Converts an array of strings into an array of guids.
        /// </summary>
        /// <param name="guidList">The string array of guids to convert.</param>
        private Guid[] ConvertStringArrayToGuidArray(string[] guidList)
        {
            var stringList = new List<Guid>();

            foreach (var guid in guidList)
            {
                stringList.Add(Guid.Parse(guid));
            }

            return stringList.ToArray();
        }

        /// <summary>
        /// Converts an array of guid strings into a single semi colon separated string.
        /// </summary>
        /// <param name="guidList">The string array of guids to convert.</param>
        private string ConvertStringArrayToString(string[] guidList)
        {
            var stringList = "";

            foreach (var guid in guidList)
            {
                stringList += guid.ToString() + ";";
            }

            return stringList;
        }

        /// <summary>
        /// Converts a string into a list of guids.
        /// </summary>
        /// <param name="guids">The string containing a list of guids separated with semicolon.</param>
        private List<Guid> ConvertStringToGuidList(string guids)
        {
            var stringList = guids.Split(';');
            List<Guid> guidsList = new List<Guid>();

            foreach (var guid in stringList)
            {
                guidsList.Add(Guid.Parse(guid));
            }

            return guidsList;
        }

        /// <summary>
        /// Converts a string into a JArray of guids.
        /// </summary>
        /// <param name="guids">The string containing a list of guids separated with semicolon.</param>
        private JArray ConvertStringToJArrayOfGuids(string guids)
        {
            if (string.IsNullOrWhiteSpace(guids))
            {
                return new JArray();
            }

            var jArray = new JArray();
            var stringList = guids.Split(';');
            
            foreach (var guid in stringList)
            {
                if (string.IsNullOrWhiteSpace(guid))
                {
                    continue;
                }

                jArray.Add(Guid.Parse(guid));
            }

            return jArray;
        }
        #endregion
    }
}
