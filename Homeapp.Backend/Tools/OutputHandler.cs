namespace Homeapp.Backend.Tools
{
    using Homeapp.Backend.Entities;
    using Homeapp.Backend.Identity;
    using Homeapp.Backend.Managers;
    using Newtonsoft.Json.Linq;
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Static public class containing methods for manipulating strings and guids.
    /// </summary>
    public static class OutputHandler
    {
        #region JObject creators
        /// <summary>
        /// Creates a JSON object containing user details
        /// </summary>
        /// <param name="user">The user</param>
        public static JObject CreateUserJObject(User user)
        {
            return new JObject()
            {
                { "Id", user.Id },
                { "FirstName", user.FirstName },
                { "LastName", user.LastName },
                { "Age", OutputHandler.GetAgeFromBirthday(user.Birthday) },
                { "Gender", Enum.GetName(typeof(Gender), user.Gender) },
                { "EmailAddress", user.EmailAddress },
                { "Birthday", user.Birthday.ToLongDateString() }
            };
        }

        /// <summary>
        /// Creates a JSON object containing checkbook account details
        /// </summary>
        /// <param name="account">The checkbook account.</param>
        /// <param name="accountOwner">The account owner.</param>
        /// <param name="accountDataManager">The account data manager</param>
        /// <param name="sharedEntityDataManager">The shared entities data manager.</param>
        public static JObject CreateCheckbookAccountJObject(
            Account account, 
            User accountOwner, 
            IAccountDataManager accountDataManager,
            ISharedEntityDataManager sharedEntityDataManager)
        {
            return new JObject()
            {
                { "AccountId", account.Id },
                { "AccountName", account.Name },
                { "AccountType", Enum.GetName(typeof(AccountType), account.AccountType) },
                { "AccountBalance", accountDataManager.CalculateAccountBalance(account) },
                { "AccountOwner", OutputHandler.CreateUserJObject(accountOwner) },
                {
                   "AllowedUsers", sharedEntityDataManager.GetSharedEntitiesJObjectFromId(account.SharedEntitiesId)
                }
            };
        }
        #endregion

        #region String/Guid handlers
        /// <summary>
        /// Converts an array of guids into a semi colon seperated string.
        /// </summary>
        /// <param name="guidList">The list of guids to convert.</param>
        public static string ConvertGuidArrayToString(Guid[] guidList)
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
        public static string ConvertGuidListToString(List<Guid> guidList)
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
        public static Guid[] ConvertStringArrayToGuidArray(string[] guidList)
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
        public static string ConvertStringArrayToString(string[] guidList)
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
        public static List<Guid> ConvertStringToGuidList(string guids)
        {
            if (string.IsNullOrWhiteSpace(guids))
            {
                return new List<Guid>();
            }

            var stringList = guids.Split(';');
            List<Guid> guidsList = new List<Guid>();

            foreach (var guid in stringList)
            {
                if (string.IsNullOrWhiteSpace(guid))
                {
                    continue;
                }
                guidsList.Add(Guid.Parse(guid));
            }

            return guidsList;
        }

        /// <summary>
        /// Converts a string into a JArray of guids.
        /// </summary>
        /// <param name="guids">The string containing a list of guids separated with semicolon.</param>
        public static JArray ConvertStringToJArrayOfGuids(string guids)
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

        /// <summary>
        /// Calculates a user's age using their birthdate
        /// </summary>
        /// <param name="birthday">The user's birthday</param>
        public static int GetAgeFromBirthday(DateTime birthday)
        {
            if (DateTime.Now.Month < birthday.Month)
            {
                return DateTime.Now.Year - birthday.Year - 1;
            }
            else if (DateTime.Now.Month == birthday.Month)
            {
                return DateTime.Now.Day < birthday.Day ?
                    DateTime.Now.Year - birthday.Year - 1 :
                    DateTime.Now.Year - birthday.Year;
            }
            else
            {
                return DateTime.Now.Year - birthday.Year;
            }
        }
    }
}
