namespace Homeapp.Backend.Tools
{
    using Homeapp.Backend.Db;
    using Homeapp.Backend.Entities;
    using Homeapp.Backend.Entities.Requests;
    using Homeapp.Backend.Identity;
    using Homeapp.Backend.Managers;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Web.Http;

    /// <summary>
    /// The validation class for the application.
    /// </summary>
    public static class CommonValidation
    {
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
        /// Checks if a string can be converted into a DateTime value..
        /// </summary>
        /// <param name="date">The string to check.</param>
        public static void DateStringIsValid(string date, string errorMessage)
        {
            if (!DateTime.TryParse(date, out var dateTime))
            {
                throw new HttpResponseException(
                    new HttpResponseMessage(HttpStatusCode.BadRequest)
                    {
                        Content = new StringContent(errorMessage),
                        ReasonPhrase = HttpReasonPhrase
                            .GetPhrase(ReasonPhrase.InvalidDate)
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
        /// Validates that an array of strings contains valid guid values.
        /// </summary>
        /// <param name="array">The array to check.</param>
        public static void StringArrayContainsValidGuids(string[] array)
        {
            if (array.Length == 0)
            {
                return;
            }

            var exception = new HttpResponseException(
                    new HttpResponseMessage(HttpStatusCode.BadRequest)
                    {
                        ReasonPhrase = HttpReasonPhrase
                            .GetPhrase(ReasonPhrase.InvalidGuid)
                    });

            foreach (var guid in array)
            {   
                if (!Guid.TryParse(guid, out Guid result))
                {
                    exception.Response.Content = new StringContent($"Invalid id received: '{guid}'");
                    throw exception;
                }
            }
        }

        /// <summary>
        /// Checks if the received phone number is valid.
        /// </summary>
        /// <param name="phoneNumber">The phone number value to check.</param>
        public static void PhoneNumberIsValid(string phoneNumber)
        {
            if (phoneNumber.Length != 10)
            {
                throw new HttpResponseException(
                    new HttpResponseMessage(HttpStatusCode.BadRequest)
                    {
                        Content = 
                            new StringContent
                                ($"Invalid phone number received: '{phoneNumber}'. Phone number must contain exactly 10 digits."),
                        ReasonPhrase = HttpReasonPhrase
                            .GetPhrase(ReasonPhrase.InvalidPhoneNumber)
                    });
            }
            else
            {
                foreach (var character in phoneNumber)
                {
                    if (!int.TryParse(character.ToString(), out int result))
                    {
                        throw new HttpResponseException(
                            new HttpResponseMessage(HttpStatusCode.BadRequest)
                            {
                                Content =
                                    new StringContent
                                        ($"Invalid phone number received: '{phoneNumber}'. Phone number must contain only digits."),
                                ReasonPhrase = HttpReasonPhrase
                                    .GetPhrase(ReasonPhrase.InvalidPhoneNumber)
                            });
                    }
                }
            }
        }

        public static void AddressRequestIsValid(AddressRequest request)
        {
            if (!string.IsNullOrWhiteSpace(request.Id))
            {
                CommonValidation.GuidIsValid(
                    guid: request.Id,
                    errorMessage: $"Invalid id received: '{request.Id}'.");
            }

            var exception = new HttpResponseException(
                    new HttpResponseMessage(HttpStatusCode.BadRequest)
                    {
                        ReasonPhrase = HttpReasonPhrase
                            .GetPhrase(ReasonPhrase.InvalidAddress)
                    });

            if (!request.City.Any(char.IsLetter))
            {
                exception.Response.Content = new StringContent("Invalid address received. City name cannot contain any numbers or special characters.");
                throw exception;
            }
            if (!request.State.Any(char.IsLetter))
            {
                exception.Response.Content = new StringContent("Invalid address received. State name cannot contain any numbers or special characters.");
                throw exception;
            }
            if (!request.Country.Any(char.IsLetter))
            {
                exception.Response.Content = new StringContent("Invalid address received. Country name cannot contain any numbers or special characters.");
                throw exception;
            }
            if (!request.ZipCode.Any(char.IsNumber))
            {
                exception.Response.Content = new StringContent("Invalid address received. Zip code can only contain numbers.");
                throw exception;
            }
        }

        /// <summary>
        /// Generates an empty string if a JSON semi colon separated string with ids is null.
        /// </summary>
        /// <returns>An empty string.</returns>
        public static string GenerateEmptyStringIfIdStringIsNull()
        {
            return "";
        }

        #region Helper methods
        /// <summary>
        /// Converts an array of guids into a semi colon seperated string.
        /// </summary>
        /// <param name="guidList">The list of guids to convert.</param>
        private static string ConvertGuidArrayToString(Guid[] guidList)
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
        private static string ConvertGuidListToString(List<Guid> guidList)
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
        private static Guid[] ConvertStringArrayToGuidArray(string[] guidList)
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
        private static string ConvertStringArrayToString(string[] guidList)
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
        private static List<Guid> ConvertStringToGuidList(string guids)
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
