namespace Homeapp.Backend.Tools
{
    using Homeapp.Backend.Db;
    using Homeapp.Backend.Entities;
    using Homeapp.Backend.Identity;
    using Homeapp.Backend.Identity.Requests;
    using System;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Web.Http;

    /// <summary>
    /// Validation class handling all identity related validation tasks.
    /// </summary>
    public static class IdentityValidation
    {
        #region Request validations
        /// <summary>
        /// Checks whether or not a request to create a user is valid.
        /// </summary>
        /// <param name="request">The request to validate.</param>
        /// <param name="appDbContext">The application database context.</param>
        /// <param name="includesCreateHouseholdRequest">Set to 'true' if the body includes a request to create a 
        /// new household. Default value is 'false'.</param>
        public static void ValidateCreateUserRequest(
            CreateUserRequest request,
            AppDbContext appDbContext,
            bool includesCreateHouseholdRequest = false)
        {
            IdentityValidation.EmailIsAlreadyInUse(
                checkIfExists: true,
                email: request.EmailAddress,
                appDbContext: appDbContext,
                errorMessage: $"Email '{request.EmailAddress}' is already in use.",
                statusCode: HttpStatusCode.BadRequest,
                reasonPhrase: ReasonPhrase.EmailAlreadyInUse);

            CommonValidation.DateStringIsValid(
                date: request.Birthday,
                errorMessage: $"Invalid date value received: '{request.Birthday}'");

            CommonValidation.BirthdayIsValid(DateTime.Parse(request.Birthday));

            if (!string.IsNullOrWhiteSpace(request.PhoneNumber))
            {
                CommonValidation.PhoneNumberIsValid(request.PhoneNumber);
            }

            if (includesCreateHouseholdRequest)
            {
                IdentityValidation.HouseholdExists(
                    householdId: request.RequestedHouseholdId,
                    appDbContext: appDbContext);

                IdentityValidation.RequestedHouseholdPasswordIsValid(
                    householdId: request.RequestedHouseholdId,
                    passwordHash: request.RequestedHousholdPasswordHash,
                    appDbContext: appDbContext);
            }
        }

        public static void ValidateHouseholdRequest(HouseholdRequest request, AppDbContext appDbContext)
        {
            if (request.AddressRequest != null)
            {
                CommonValidation.AddressRequestIsValid(request.AddressRequest);
            }
            
            CommonValidation.PhoneNumberIsValid(request.PhoneNumber);
            IdentityValidation.ValidateAllowedUsersRequest(request.AllowedUsers, appDbContext);

            if (request.HouseholdGroupRequests.Length > 0)
            {
                foreach (var groupRequest in request.HouseholdGroupRequests)
                {
                    IdentityValidation.ValidateHouseholdGroupRequest(
                        request: groupRequest,
                        appDbContext: appDbContext);
                }
            }
        }

        /// <summary>
        /// Validates an incoming request to create a new household group.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="appDbContext">The application database context.</param>
        public static void ValidateHouseholdGroupRequest(HouseholdGroupRequest request, AppDbContext appDbContext)
        {
            if (request.UserIds.Length > 0)
            {
                foreach (var id in request.UserIds)
                {
                    CommonValidation.GuidIsValid(
                        guid: id,
                        errorMessage: $"Invalid user id received: '{id}'");
                    IdentityValidation.UserExists(
                        userId: Guid.Parse(id),
                        appDbContext: appDbContext);
                }
            }

            IdentityValidation.ValidateAllowedUsersRequest(
                request: request.AllowedUsers, 
                appDbContext: appDbContext);
        }

        /// <summary>
        /// Validates an AllowedUsers request.
        /// </summary>
        /// <param name="request">The request to validate.</param>
        /// <param name="appDbContext">The application database context.</param>
        public static void ValidateAllowedUsersRequest(AllowedUsersRequest request, AppDbContext appDbContext)
        {
            CommonValidation.StringArrayContainsValidGuids(request.ReadHouseholdIds);
            CommonValidation.StringArrayContainsValidGuids(request.ReadHouseholdGroupIds);
            CommonValidation.StringArrayContainsValidGuids(request.ReadUserIds);
            CommonValidation.StringArrayContainsValidGuids(request.WriteHouseholdIds);
            CommonValidation.StringArrayContainsValidGuids(request.WriteHouseholdGroupIds);
            CommonValidation.StringArrayContainsValidGuids(request.WriteUserIds);

            IdentityValidation.HouseholdIdArrayContainsExistingIds(
                ids: OutputHandler.ConvertStringArrayToGuidArray(request.ReadHouseholdIds),
                appDbContext: appDbContext);

            IdentityValidation.HouseholdGroupIdArrayContainsExistingIds(
                ids: OutputHandler.ConvertStringArrayToGuidArray(request.ReadHouseholdGroupIds),
                appDbContext: appDbContext);

            IdentityValidation.UserIdArrayContainsExistingIds(
                ids: OutputHandler.ConvertStringArrayToGuidArray(request.ReadUserIds),
                appDbContext: appDbContext);

            IdentityValidation.HouseholdIdArrayContainsExistingIds(
                ids: OutputHandler.ConvertStringArrayToGuidArray(request.WriteHouseholdIds),
                appDbContext: appDbContext);

            IdentityValidation.HouseholdGroupIdArrayContainsExistingIds(
                ids: OutputHandler.ConvertStringArrayToGuidArray(request.WriteHouseholdGroupIds),
                appDbContext: appDbContext);

            IdentityValidation.UserIdArrayContainsExistingIds(
                ids: OutputHandler.ConvertStringArrayToGuidArray(request.WriteUserIds),
                appDbContext: appDbContext);
        }
        #endregion

        /// <summary>
        /// Checks if the requesting user has read access to a resource.
        /// </summary>
        /// <param name="requestingUser">The requesting user.</param>
        /// <param name="ownerId">The resource owner's id.</param>
        /// <param name="sharedEntities">The sharedEntities record to check.</param>
        /// <param name="errorMessage">The error message to use in the response message.</param>
        public static void UserHasReadAccessToResource(
            User requestingUser,
            Guid ownerId,
            AllowedUsers sharedEntities,
            string errorMessage)
        {
            var userHasReadAccess = false;

            var userHouseholds = requestingUser.Households;
            var userHouseholdGroups = requestingUser.HouseholdGroups;

            var readUsers = OutputHandler.ConvertStringToGuidList(sharedEntities.ReadUserIds);
            var readHouseholds = OutputHandler.ConvertStringToGuidList(sharedEntities.ReadHouseholdIds);
            var readHouseholdGroups = OutputHandler.ConvertStringToGuidList(sharedEntities.ReadHouseholdGroupIds);

            var writeUsers = OutputHandler.ConvertStringToGuidList(sharedEntities.WriteUserIds);
            var writeHouseholds = OutputHandler.ConvertStringToGuidList(sharedEntities.WriteHouseholdIds);
            var writeHouseholdGroups = OutputHandler.ConvertStringToGuidList(sharedEntities.WriteHouseholdGroupIds);

            var fullAccessUsers = OutputHandler.ConvertStringToGuidList(sharedEntities.FullAccessUserIds);
            var fullAccessHouseholds = OutputHandler.ConvertStringToGuidList(sharedEntities.FullAccessUserIds);
            var fullAccessHouseholdGroups = OutputHandler.ConvertStringToGuidList(sharedEntities.FullAccessUserIds);

            if (requestingUser.Id == ownerId)
            {
                userHasReadAccess = true;
            }
            else if (readUsers.Contains(requestingUser.Id))
            {
                userHasReadAccess = true;
            }
            else if (writeUsers.Contains(requestingUser.Id))
            {
                userHasReadAccess = true;
            }
            else if (fullAccessUsers.Contains(requestingUser.Id))
            {
                userHasReadAccess = true;
            }
            else
            {
                foreach (var household in userHouseholds)
                {
                    if (readHouseholds.Contains(household.HouseholdId))
                    {
                        userHasReadAccess = true;
                        break;
                    }
                    else if (writeHouseholds.Contains(household.HouseholdId))
                    {
                        userHasReadAccess = true;
                        break;
                    }
                    else if (fullAccessHouseholds.Contains(household.HouseholdId))
                    {
                        userHasReadAccess = true;
                        break;
                    }
                }
                if (!userHasReadAccess)
                {
                    foreach (var group in userHouseholdGroups)
                    {
                        if (readHouseholdGroups.Contains(group.HouseholdGroupId))
                        {
                            userHasReadAccess = true;
                            break;
                        }
                        else if (writeHouseholdGroups.Contains(group.HouseholdGroupId))
                        {
                            userHasReadAccess = true;
                            break;
                        }
                        else if (fullAccessHouseholdGroups.Contains(group.HouseholdGroupId))
                        {
                            userHasReadAccess = true;
                            break;
                        }
                    }
                }
            }

            if (!userHasReadAccess)
            {
                throw new HttpResponseException(
                    new HttpResponseMessage(HttpStatusCode.Unauthorized)
                    {
                        Content = new StringContent(errorMessage),
                        ReasonPhrase = HttpReasonPhrase
                            .GetPhrase(ReasonPhrase.UserUnauthorized)
                    }); ;
            }
        }

        /// <summary>
        /// Checks if the requesting user has write access to a resource.
        /// </summary>
        /// <param name="requestingUser">The requesting user.</param>
        /// <param name="ownerId">The resource owner's id.</param>
        /// <param name="sharedEntities">The sharedEntities record to check.</param>
        /// <param name="errorMessage">The error message to use in the response message.</param>
        public static void UserHasWriteAccessToResource(
            User requestingUser,
            Guid ownerId,
            AllowedUsers sharedEntities,
            string errorMessage)
        {
            var userHasWriteAccess = false;

            var userHouseholds = requestingUser.Households;
            var userHouseholdGroups = requestingUser.HouseholdGroups;

            var writeUsers = OutputHandler.ConvertStringToGuidList(sharedEntities.WriteUserIds);
            var writeHouseholds = OutputHandler.ConvertStringToGuidList(sharedEntities.WriteHouseholdIds);
            var writeHouseholdGroups = OutputHandler.ConvertStringToGuidList(sharedEntities.WriteHouseholdGroupIds);

            var fullAccessUsers = OutputHandler.ConvertStringToGuidList(sharedEntities.FullAccessUserIds);
            var fullAccessHouseholds = OutputHandler.ConvertStringToGuidList(sharedEntities.FullAccessUserIds);
            var fullAccessHouseholdGroups = OutputHandler.ConvertStringToGuidList(sharedEntities.FullAccessUserIds);

            if (requestingUser.Id == ownerId)
            {
                userHasWriteAccess = true;
            }
            else if (writeUsers.Contains(requestingUser.Id))
            {
                userHasWriteAccess = true;
            }
            else if (fullAccessUsers.Contains(requestingUser.Id))
            {
                userHasWriteAccess = true;
            }
            else
            {
                foreach (var household in userHouseholds)
                {
                    if (writeHouseholds.Contains(household.HouseholdId))
                    {
                        userHasWriteAccess = true;
                        break;
                    }
                    else if (fullAccessHouseholds.Contains(household.HouseholdId))
                    {
                        userHasWriteAccess = true;
                        break;
                    }
                }
                if (!userHasWriteAccess)
                {
                    foreach (var group in userHouseholdGroups)
                    {
                        if (writeHouseholdGroups.Contains(group.HouseholdGroupId))
                        {
                            userHasWriteAccess = true;
                            break;
                        }
                        else if (fullAccessHouseholdGroups.Contains(group.HouseholdGroupId))
                        {
                            userHasWriteAccess = true;
                            break;
                        }
                    }
                }
            }

            if (!userHasWriteAccess)
            {
                throw new HttpResponseException(
                    new HttpResponseMessage(HttpStatusCode.Unauthorized)
                    {
                        Content = new StringContent(errorMessage),
                        ReasonPhrase = HttpReasonPhrase
                            .GetPhrase(ReasonPhrase.UserUnauthorized)
                    }); ;
            }
        }

        /// <summary>
        /// Checks if the requesting user has full access to a resource.
        /// </summary>
        /// <param name="requestingUser">The requesting user.</param>
        /// <param name="ownerId">The resource owner's id.</param>
        /// <param name="sharedEntities">The sharedEntities record to check.</param>
        /// <param name="errorMessage">The error message to use in the response message.</param>
        public static void UserHasFullAccessToResource(
            User requestingUser,
            Guid ownerId,
            AllowedUsers sharedEntities,
            string errorMessage)
        {
            var userHasFullAccess = false;

            var userHouseholds = requestingUser.Households;
            var userHouseholdGroups = requestingUser.HouseholdGroups;

            var fullAccessUsers = OutputHandler.ConvertStringToGuidList(sharedEntities.FullAccessUserIds);
            var fullAccessHouseholds = OutputHandler.ConvertStringToGuidList(sharedEntities.FullAccessUserIds);
            var fullAccessHouseholdGroups = OutputHandler.ConvertStringToGuidList(sharedEntities.FullAccessUserIds);

            if (requestingUser.Id == ownerId)
            {
                userHasFullAccess = true;
            }
            else if (fullAccessUsers.Contains(requestingUser.Id))
            {
                userHasFullAccess = true;
            }
            else
            {
                foreach (var household in userHouseholds)
                {
                    if (fullAccessHouseholds.Contains(household.HouseholdId))
                    {
                        userHasFullAccess = true;
                        break;
                    }
                }
                if (!userHasFullAccess)
                {
                    foreach (var group in userHouseholdGroups)
                    {
                        if (fullAccessHouseholdGroups.Contains(group.HouseholdGroupId))
                        {
                            userHasFullAccess = true;
                            break;
                        }
                    }
                }
            }

            if (!userHasFullAccess)
            {
                throw new HttpResponseException(
                    new HttpResponseMessage(HttpStatusCode.Unauthorized)
                    {
                        Content = new StringContent(errorMessage),
                        ReasonPhrase = HttpReasonPhrase
                            .GetPhrase(ReasonPhrase.UserUnauthorized)
                    });
            }
        }

        /// <summary>
        /// Checks if a specified email address is already in use.
        /// </summary>
        /// <param name="checkIfExists">Set to 'true' to throw an exception if the email exists. 
        /// Set to 'false' to throw an exception if the email does not exist.</param>
        /// <param name="email">The email to look for.</param>
        /// <param name="appDbContext">The application database context.</param>
        /// <param name="errorMessage">The error message to use in the HttpResponse.</param>
        /// <param name="statusCode">The status code to provide in the response.</param>
        /// <param name="reasonPhrase">The reason phrase to provide in the response.</param>
        public static void EmailIsAlreadyInUse(
            bool checkIfExists,
            string email,
            AppDbContext appDbContext,
            string errorMessage,
            HttpStatusCode statusCode,
            ReasonPhrase reasonPhrase)
        {
            User userWithExistingEmail = appDbContext.Users.FirstOrDefault(u => u.EmailAddress == email);

            if (checkIfExists)
            {
                if (userWithExistingEmail != null)
                {
                    throw new HttpResponseException(
                        new HttpResponseMessage(statusCode)
                        {
                            Content = new StringContent(errorMessage),
                            ReasonPhrase = HttpReasonPhrase
                                .GetPhrase(reasonPhrase)
                        });
                }
            }
            else
            {
                if (userWithExistingEmail == null)
                {
                    throw new HttpResponseException(
                        new HttpResponseMessage(statusCode)
                        {
                            Content = new StringContent(errorMessage),
                            ReasonPhrase = HttpReasonPhrase
                                .GetPhrase(reasonPhrase)
                        });
                }
            }
        }


        /// <summary>
        /// Checks if a specified household exists in the database.
        /// </summary>
        /// <param name="householdId">The household id to look for.</param>
        /// <param name="appDbContext">The application database context.</param>
        public static void HouseholdExists(Guid householdId, AppDbContext appDbContext)
        {
            var existingHousehold = appDbContext.Households.FirstOrDefault(h => h.Id == householdId);

            if (existingHousehold == null)
            {
                throw new HttpResponseException(
                    new HttpResponseMessage(HttpStatusCode.NotFound)
                    {
                        Content = new StringContent($"Houshold with id '{householdId}' was not found."),
                        ReasonPhrase = HttpReasonPhrase
                            .GetPhrase(ReasonPhrase.HouseholdNotFound)
                    });
            }
        }

        /// <summary>
        /// Validates that a group exists.
        /// </summary>
        /// <param name="groupId">The group id to look for.</param>
        /// <param name="appDbContext">The application database context.</param>
        public static void HouseholdGroupExists(Guid groupId, AppDbContext appDbContext)
        {
            var existingHouseholdGroup = appDbContext.HouseholdGroups.FirstOrDefault(u => u.Id == groupId);

            if (existingHouseholdGroup == null)
            {
                throw new HttpResponseException(
                   new HttpResponseMessage(HttpStatusCode.NotFound)
                   {
                       Content = new StringContent($"Household group with id '{groupId}' was not found."),
                       ReasonPhrase = HttpReasonPhrase
                           .GetPhrase(ReasonPhrase.HouseholdGroupNotFound)
                   });
            }
        }

        /// <summary>
        /// Validates that a group is in a household.
        /// </summary>
        /// <param name="groupId">The group id to look for.</param>
        /// <param name="householdId">The household id to look in.</param>
        /// <param name="appDbContext">The application database context.</param>
        public static void GroupIsInHousehold(Guid groupId, Guid householdId, AppDbContext appDbContext)
        {
            var existingHouseholdGroup =
                appDbContext
                .HouseholdGroups
                .Where(h => h.Id == groupId)
                .FirstOrDefault(h => h.HouseholdId == householdId);

            if (existingHouseholdGroup == null)
            {
                throw new HttpResponseException(
                   new HttpResponseMessage(HttpStatusCode.NotFound)
                   {
                       Content = new StringContent($"Group with id '{groupId}' does not exist in household with id '{householdId}'"),
                       ReasonPhrase = HttpReasonPhrase
                           .GetPhrase(ReasonPhrase.HouseholdGroupNotFound)
                   });
            }
        }

        /// <summary>
        /// Validates that a user is in a household.
        /// </summary>
        /// <param name="userId">The user id to check.</param>
        /// <param name="householdId">The household id to look in.</param>
        /// <param name="appDbContext">The application database context.</param>
        /// <param name="errorMessage">The error message to use in the HTTP response message in the event of an error.</param>
        public static void UserIsInHousehold(
            Guid userId,
            Guid householdId,
            AppDbContext appDbContext,
            string errorMessage)
        {
            var existingUser =
                appDbContext
                .UserHouseholds
                .Where(h => h.HouseholdId == householdId)
                .FirstOrDefault(h => h.UserId == userId);

            if (existingUser == null)
            {
                throw new HttpResponseException(
                   new HttpResponseMessage(HttpStatusCode.Unauthorized)
                   {
                       Content = new StringContent(errorMessage),
                       ReasonPhrase = HttpReasonPhrase
                           .GetPhrase(ReasonPhrase.UserUnauthorized)
                   });
            }
        }

        /// <summary>
        /// Checks whether or not a user id exists in the database.
        /// </summary>
        /// <param name="userId">The user id to check.</param>
        /// <param name="appDbContext">The application database context object.</param>
        /// <exception cref="HttpResponseException"></exception>
        public static void UserExists(Guid userId, AppDbContext appDbContext)
        {
            var existingUserId = appDbContext.Users.FirstOrDefault(u => u.Id == userId);

            if (existingUserId == null)
            {
                throw new HttpResponseException(
                   new HttpResponseMessage(HttpStatusCode.NotFound)
                   {
                       Content = new StringContent($"User with id '{userId}' was not found."),
                       ReasonPhrase = HttpReasonPhrase
                           .GetPhrase(ReasonPhrase.UserNotFound)
                   });
            }
        }

        /// <summary>
        /// Validates that a household password is valid.
        /// </summary>
        /// <param name="householdId">The household id.</param>
        /// <param name="passwordHash">The password hash to check.</param>
        /// <param name="appDbContext">The application database context object.</param>
        public static void RequestedHouseholdPasswordIsValid
            (Guid householdId,
            string passwordHash,
            AppDbContext appDbContext)
        {
            if (appDbContext
                .Households
                .FirstOrDefault(h => h.Id == householdId)
                .PasswordHash != passwordHash)
            {
                throw new HttpResponseException(
                   new HttpResponseMessage(HttpStatusCode.Unauthorized)
                   {
                       Content = new StringContent($"Invalid household password."),
                       ReasonPhrase = HttpReasonPhrase
                           .GetPhrase(ReasonPhrase.InvalidHouseholdPassword)
                   });
            }
        }

        /// <summary>
        /// Checks if a given email password combination is valid.
        /// </summary>
        /// <param name="email">The given email address.</param>
        /// <param name="passwordHash">The given password hash.</param>
        /// <param name="appDbContext">The application database context object.</param>
        public static void UserEmailPasswordComboIsValid
            (string email,
            string passwordHash,
            AppDbContext appDbContext)
        {
            if (appDbContext
                .Users
                .FirstOrDefault(u => u.EmailAddress == email)
                .PasswordHash != passwordHash)
            {
                throw new HttpResponseException(
                   new HttpResponseMessage(HttpStatusCode.NotFound)
                   {
                       Content = new StringContent("Invalid user email or password."),
                       ReasonPhrase = HttpReasonPhrase
                           .GetPhrase(ReasonPhrase.InvalidUserCredentials)
                   });
            }
        }

        /// <summary>
        /// Confirms that all household ids in an array exist in the database.
        /// </summary>
        /// <param name="ids">The list of ids to check.</param>
        /// <param name="appDbContext">The application database context.</param>
        public static void HouseholdIdArrayContainsExistingIds(Guid[] ids, AppDbContext appDbContext)
        {
            foreach (var id in ids)
            {
                if (appDbContext.Households
                    .FirstOrDefault(h => h.Id == id) == null)
                {
                    throw new HttpResponseException(
                    new HttpResponseMessage(HttpStatusCode.NotFound)
                    {
                        Content = new StringContent($"Household with id '{id}' was not found."),
                        ReasonPhrase = HttpReasonPhrase
                            .GetPhrase(ReasonPhrase.HouseholdNotFound)
                    });
                }
            }
        }

        /// <summary>
        /// Confirms that all household group ids in an array exist in the database.
        /// </summary>
        /// <param name="ids">The list of ids to check.</param>
        /// <param name="appDbContext">The application database context.</param>
        public static void HouseholdGroupIdArrayContainsExistingIds(Guid[] ids, AppDbContext appDbContext)
        {
            foreach (var id in ids)
            {
                if (appDbContext.HouseholdGroups
                    .FirstOrDefault(h => h.Id == id) == null)
                {
                    throw new HttpResponseException(
                    new HttpResponseMessage(HttpStatusCode.NotFound)
                    {
                        Content = new StringContent($"Household group with id '{id}' was not found."),
                        ReasonPhrase = HttpReasonPhrase
                            .GetPhrase(ReasonPhrase.HouseholdGroupNotFound)
                    });
                }
            }
        }

        /// <summary>
        /// Confirms that all user ids in an array exist in the database.
        /// </summary>
        /// <param name="ids">The list of ids to check.</param>
        /// <param name="appDbContext">The application database context.</param>
        public static void UserIdArrayContainsExistingIds(Guid[] ids, AppDbContext appDbContext)
        {
            foreach (var id in ids)
            {
                if (appDbContext.Users
                    .FirstOrDefault(u => u.Id == id) == null)
                {
                    throw new HttpResponseException(
                    new HttpResponseMessage(HttpStatusCode.NotFound)
                    {
                        Content = new StringContent($"User with id '{id}' was not found."),
                        ReasonPhrase = HttpReasonPhrase
                            .GetPhrase(ReasonPhrase.UserNotFound)
                    });
                }
            }
        }
    }
}
