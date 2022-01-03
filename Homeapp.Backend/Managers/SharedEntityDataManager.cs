

namespace Homeapp.Backend.Managers
{
    using Homeapp.Backend.Db;
    using Homeapp.Backend.Entities;
    using Homeapp.Backend.Entities.Common.Requests;
    using Homeapp.Backend.Tools;
    using System;
    using System.Collections.Generic;
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
        /// Uses a ShredEntitiesRequest object to create a SharedEntities record in the database.
        /// </summary>
        /// <param name="request">The request.</param>
        public async Task<SharedEntities> CreateNewSharedEntitiesRecord(SharedEntitiesRequest request)
        {
            var sharedEntities =  new SharedEntities
            {
                ReadHouseholdIds = this.ConvertStringArrayToString(request.ReadHouseholdIds),
                ReadHouseholdGroupIds = this.ConvertStringArrayToString(request.ReadHouseholdGroupIds),
                ReadUserIds = this.ConvertStringArrayToString(request.ReadUserIds),
                EditHouseholdIds = this.ConvertStringArrayToString(request.EditHouseholdIds),
                EditHouseholdGroupIds = this.ConvertStringArrayToString(request.EditHouseholdGroupIds),
                EditUserIds = this.ConvertStringArrayToString(request.EditUserIds)
            };

            appDbContext.SharedEntities.Add(sharedEntities);

            try
            {
                await appDbContext.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw new HttpResponseException(
                   new HttpResponseMessage(HttpStatusCode.ServiceUnavailable)
                   {
                       Content = new StringContent("There was an error when saving the shared entities record to the database. Please try again."),
                       ReasonPhrase = HttpReasonPhrase
                           .GetPhrase(ReasonPhrase.ErrorSavingToDatabase)
                   });
            }

            return sharedEntities;
        }

        #region Helper methods
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
        #endregion
    }
}
