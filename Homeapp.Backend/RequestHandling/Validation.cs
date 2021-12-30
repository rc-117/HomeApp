namespace Homeapp.Backend.Tools
{
    using Homeapp.Backend.Db;
    using Homeapp.Backend.Entities;
    using Homeapp.Backend.Identity;
    using Homeapp.Backend.Managers;
    using System;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Web.Http;

    /// <summary>
    /// The validation class for the application.
    /// </summary>
    public static class Validation
    {
        /// <summary>
        /// Checks if an account belongs to the logged in user.
        /// </summary>
        /// <param name="userId">The logged in user's id.</param>
        /// <param name="account">The account to check.</param>
        /// <returns></returns>
        public static void AccountBelongsToUser(Guid userId, Account account)
        {
            if (userId != account.UserId)
            {
                throw new HttpResponseException(
                    new HttpResponseMessage(HttpStatusCode.Unauthorized)
                    {
                        Content = new StringContent("User unauthorized to access account."),
                        ReasonPhrase = HttpReasonPhrase
                            .GetPhrase(ReasonPhrase.UserUnauthorized)
                    }); ;
            } 
        }

        /// <summary>
        /// Validates that a string can be converted into base 64 bytes.
        /// </summary>
        /// <param name="value">The string to check.</param>
        /// <param name="errorMessage">The error message to use in the HttpResponse.</param>
        /// <param name="bytes">The out result.</param>
        public static void StringIsBase64Compatible(
            string value,
            string errorMessage,
            out byte[] bytes)
        {
            try
            {
                bytes = Convert.FromBase64String(value);
            }
            catch (Exception)
            {
                bytes = null;

                throw new HttpResponseException(
                    new HttpResponseMessage(HttpStatusCode.BadRequest)
                    {
                        Content = new StringContent(errorMessage),
                        ReasonPhrase = HttpReasonPhrase
                            .GetPhrase(ReasonPhrase.InvalidRequest)
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
        /// Checks if an int array can be converted to a DateTimeObject.
        /// </summary>
        /// <param name="dateArray">The int array.</param>
        public static void IntArrayIsDateTimeConvertible(int[] dateArray)
        {
            var isValid = dateArray.Count() != 3 ? false :
                (dateArray[0] < 0 || dateArray[0] > 12 ? false : (
                dateArray[1] < 0 || dateArray[1] > 31 ? false :
                dateArray[2] < 0 ? false : true));

            if (isValid)
            {
                try
                {
                    var dateTime = new DateTime(dateArray[2], dateArray[0], dateArray[1]);                    
                }
                catch (Exception)
                {
                    throw new HttpResponseException(
                        new HttpResponseMessage(HttpStatusCode.BadRequest)
                        {
                            Content = new StringContent("An invalid date array was provided. Integer array must be in m/d/yyyy format."),
                            ReasonPhrase = HttpReasonPhrase
                                .GetPhrase(ReasonPhrase.InvalidDateArray)
                        });
                }
            }            
        }

        /// <summary>
        /// Checks if a given birthday is valid.
        /// </summary>
        /// <param name="birthday">The birthday DateTime object.</param>
        public static void BirthdayIsValid(DateTime birthday)
        {
            if (birthday.Date > DateTime.Now.Date)
            {
                throw new HttpResponseException(
                    new HttpResponseMessage(HttpStatusCode.BadRequest)
                    {
                        Content = new StringContent("An invalid birthdate was provided. The DOB must be in the past."),
                        ReasonPhrase = HttpReasonPhrase
                            .GetPhrase(ReasonPhrase.InvalidBirthday)
                    });
            } 
        }

        /// <summary>
        /// Checks if an int array has valid m/d/yyy values.
        /// </summary>
        /// <param name="dateArray">The int array.</param>
        public static void DateIntArrayIsValid(int[] dateArray)
        {
            var isValid = dateArray.Count() != 3 ? false :
                (dateArray[0] < 0 || dateArray[0] > 12 ? false : (
                dateArray[1] < 0 || dateArray[1] > 31 ? false :
                dateArray[2] < 0 ? false : true));

            if (!isValid)
            {
                throw new HttpResponseException(
                    new HttpResponseMessage(HttpStatusCode.BadRequest)
                    {
                        Content = new StringContent("An invalid date array was provided. Integer array must be in m/d/yyyy format."),
                        ReasonPhrase = HttpReasonPhrase
                            .GetPhrase(ReasonPhrase.InvalidDateArray)
                    });
            }
        }

        /// <summary>
        /// Checks if a string can be converted to a GUID.
        /// </summary>
        /// <param name="guid">The string to check.</param>
        /// <param name="errorMessage">The error message to use in the response body.</param>
        public static void GuidIsValid(string guid, string errorMessage)
        {
            if (!Guid.TryParse(guid, out Guid result))
            {
                throw new HttpResponseException(
                    new HttpResponseMessage(HttpStatusCode.BadRequest)
                    {
                        Content = new StringContent(errorMessage),
                        ReasonPhrase = HttpReasonPhrase
                            .GetPhrase(ReasonPhrase.InvalidGuid)
                    });
            }            
        }

        /// <summary>
        /// Validates that a specified checkbook account exists in the database.
        /// </summary>
        /// <param name="Id">The Id of the checkbook.</param>
        /// <param name="accountManager">The account data manager.</param>
        public static void CheckbookAccountExists(Guid Id, IAccountDataManager accountManager)
        {
            var account = accountManager.GetAccountById(Id);
            if (account == null)
            {
                throw new HttpResponseException(
                    new HttpResponseMessage(HttpStatusCode.NotFound)
                    {
                        Content = new StringContent($"Checkbook account with id '{Id}' was not found."),
                        ReasonPhrase = HttpReasonPhrase
                            .GetPhrase(ReasonPhrase.CheckbookAccountNotFound)
                    });
            }
        }
    }
}
