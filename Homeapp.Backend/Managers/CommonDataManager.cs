namespace Homeapp.Backend.Managers
{
    using Homeapp.Backend.Db;
    using Homeapp.Backend.Entities;
    using Homeapp.Backend.Entities.Requests;
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
    public class CommonDataManager : ICommonDataManager
    {
        private AppDbContext appDbContext;

        /// <summary>
        /// Initializes an instance of the Shared Entity Data Manager.
        /// </summary>
        /// <param name="appDbContext">The application database context.</param>
        public CommonDataManager(AppDbContext appDbContext)
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
        /// Adds a user to a read access list.
        /// </summary>
        /// <param name="userId">The user's id to add.</param>
        /// <param name="allowedUsers">The AllowedUsers record to add the user id to.</param>
        /// <returns>The updated AllowedUsers record.</returns>
        public AllowedUsers AddUserToReadAccess(Guid userId, AllowedUsers allowedUsers)
        {
            allowedUsers.ReadHouseholdIds =            
                OutputHandler
                .AddGuidToSemiColonSeparatedStringList(
                    guid: userId, 
                    list: allowedUsers.ReadUserIds);

            this.appDbContext.SharedEntities.Update(allowedUsers);
            this.appDbContext.SaveChanges();

            return allowedUsers;
        }

        /// <summary>
        /// Checks if a user, household, or household group id is in a semi colon separated string list of ids.
        /// </summary>
        /// <param name="idList">The list to check.</param>
        /// <param name="entityId">The id to look for.</param>
        /// <returns>True if the list contains the id. False if not.</returns>
        public bool EntityIdIsInList (string idList, Guid entityId)
        {
            var list = OutputHandler.ConvertStringToGuidList(idList);
            return list.Contains(entityId);
        }

        /// <summary>
        /// Creates and saves a new AllowedUsers record to the database.
        /// </summary>
        /// <param name="request">The request.</param>
        public async Task<AllowedUsers> SaveAllowedUsersObjectToDb(AllowedUsersRequest request)
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

            try
            {
                this.appDbContext.SharedEntities.Add(allowedUsers);
                await this.appDbContext.SaveChangesAsync();

                return allowedUsers;
            }
            catch (Exception)
            {
                throw new HttpResponseException(
                    new HttpResponseMessage(HttpStatusCode.InternalServerError)
                    {
                        Content = new StringContent("There was an error saving an 'AllowedUsers' record to the database. Please try again."),
                        ReasonPhrase = HttpReasonPhrase
                            .GetPhrase(ReasonPhrase.ErrorSavingToDatabase)
                    });
            }            
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

        /// <summary>
        /// Gets a recurring schedule object by its id.
        /// </summary>
        /// <param name="id">The unique id of the recurring schedule.</param>
        public RecurringSchedule GetRecurringScheduleById(Guid id)
        {
            return this.appDbContext
                .RecurringSchedules
                .FirstOrDefault(r => r.Id == id);
        }

        /// <summary>
        /// Creates and saves an address record in the database.
        /// </summary>
        /// <param name="addressRequest">The request.</param>
        /// <returns>The created address.</returns>
        public async Task<Address> SaveAddressToDb(AddressRequest addressRequest)
        {
            var address = new Address()
            {
                BusinessName =
                    !string.IsNullOrWhiteSpace(addressRequest.BusinessName) ?
                    addressRequest.BusinessName : null,
                StreetAddress = addressRequest.StreetAddress,
                City = addressRequest.City,
                State = addressRequest.State,
                Country = addressRequest.Country,
                ZipCode = addressRequest.ZipCode
            };

            try
            {
                this.appDbContext.Addresses.Add(address);
                await this.appDbContext.SaveChangesAsync();
                return address;
            }
            catch (Exception)
            {
                throw new HttpResponseException(
                    new HttpResponseMessage(HttpStatusCode.InternalServerError)
                    {
                        Content = new StringContent("There was an error when saving an address to the database. Please try again."),
                        ReasonPhrase = HttpReasonPhrase
                            .GetPhrase(ReasonPhrase.ErrorSavingToDatabase)
                    });
            }
        }
    }
}
