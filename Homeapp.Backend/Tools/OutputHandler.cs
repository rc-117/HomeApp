namespace Homeapp.Backend.Tools
{
    using Homeapp.Backend.Db;
    using Homeapp.Backend.Entities;
    using Homeapp.Backend.Identity;
    using Homeapp.Backend.Managers;
    using Newtonsoft.Json.Linq;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Static public class containing methods for manipulating strings and guids.
    /// </summary>
    public static class OutputHandler
    {
        #region Common JObject creators
        /// <summary>
        /// Returns a JSON object containing shared/allowed entities for an item. Used for response handling.
        /// </summary>
        /// <param name="id">The id to select the SharedEntities record.</param>
        /// <param name="allowedUsersDataManager">The shared entity data manager.</param>
        /// <param name="userDataManager">The user data manager.</param>
        public static JObject GetAllowedUsersJObjectWithId(Guid id, IAllowedUsersDataManager allowedUsersDataManager, IUserDataManager userDataManager)
        {
            var sharedEntities = allowedUsersDataManager.GetAllowedUsersObjectFromId(id);

            var readHouseholdArray = 
                ConvertStringToJArrayOfNameGuidPairs(sharedEntities.ReadHouseholdIds, searchHouseholds: true, userDataManager: userDataManager);
            var readHouseholdGroupArray = 
                ConvertStringToJArrayOfNameGuidPairs(sharedEntities.ReadHouseholdGroupIds, searchGroups: true, userDataManager: userDataManager);
            var readUserArray = 
                ConvertStringToJArrayOfNameGuidPairs(sharedEntities.ReadUserIds, searchUsers: true, userDataManager: userDataManager);
            var editHouseholdArray = 
                ConvertStringToJArrayOfNameGuidPairs(sharedEntities.WriteHouseholdIds, searchHouseholds: true, userDataManager: userDataManager);
            var editHouseholdGroupArray = 
                ConvertStringToJArrayOfNameGuidPairs(sharedEntities.WriteHouseholdGroupIds, searchGroups: true, userDataManager: userDataManager);
            var editUserArray = 
                ConvertStringToJArrayOfNameGuidPairs(sharedEntities.WriteUserIds, searchUsers: true, userDataManager: userDataManager);

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
        #endregion

        #region Identity JObject Creators
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
        #endregion

        #region Checkbook JObject Creators
        /// <summary>
        /// Creates a JSON object containing checkbook account details
        /// </summary>
        /// <param name="account">The checkbook account.</param>
        /// <param name="accountOwner">The account owner.</param>
        /// <param name="accountDataManager">The account data manager</param>
        /// <param name="sharedEntityDataManager">The shared entities data manager.</param>
        /// <param name="userDataManager">The user data manager.</param>
        public static JObject CreateCheckbookAccountJObject(
            Account account, 
            User accountOwner, 
            IAccountDataManager accountDataManager,
            IAllowedUsersDataManager sharedEntityDataManager,
            IUserDataManager userDataManager)
        {
            return new JObject()
            {
                { "AccountId", account.Id },
                { "AccountName", account.Name },
                { "AccountType", Enum.GetName(typeof(AccountType), account.AccountType) },
                { "AccountBalance", accountDataManager.CalculateAccountBalance(account) },
                { "AccountOwner", OutputHandler.CreateUserJObject(accountOwner) },
                {
                   "AllowedUsers", OutputHandler.GetAllowedUsersJObjectWithId(
                       id: account.AllowedUsersId,
                       allowedUsersDataManager: sharedEntityDataManager,
                       userDataManager: userDataManager)
                }
            };
        }

        /// <summary>
        /// /// Creates a JArray containing all transactions in an account.
        /// </summary>
        /// <param name="accountId">The account id.</param>
        /// <param name="accountDataManager">The account data manager.</param>
        /// <param name="userDataManager">The user data manager.</param>
        /// <param name="sharedEntityDataManager">The shared entity data manager.</param>
        public static JArray CreateTransactionsJArrayByAccountId(
            Guid accountId, 
            IAccountDataManager accountDataManager, 
            IUserDataManager userDataManager,
            IAllowedUsersDataManager sharedEntityDataManager)
        {
            var transactions = accountDataManager.GetTransactionsByAccount(accountId);
            var jArray = new JArray();
            foreach (var transaction in transactions)
            {
                jArray.Add(CreateTransactionJObject(
                    transaction: transaction,
                    userDataManager: userDataManager,
                    accountDataManager: accountDataManager,
                    allowedUsersDataManager: sharedEntityDataManager));
            }
            return jArray;
        }

        /// <summary>
        /// Creates a JSON object with a transaction's properties.
        /// </summary>
        /// <param name="transaction">The transaction object to use.</param>
        /// <param name="userDataManager">The user data manager.</param>
        /// <param name="accountDataManager">The checkbook account data manager.</param>
        /// <param name="allowedUsersDataManager">The allowed users data manager.</param>
        public static JObject CreateTransactionJObject(
            Transaction transaction, 
            IUserDataManager userDataManager,
            IAccountDataManager accountDataManager,
            IAllowedUsersDataManager allowedUsersDataManager)
        {
            var incomeCategory = transaction.IncomeCategory == null ?
                null : transaction.IncomeCategory.Name;

            var expenseCategory = transaction.ExpenseCategory == null ?
                null : transaction.ExpenseCategory.Name;
            
            var transactionAmount =
                transaction.TransactionType == TransactionType.Expense ||
                transaction.TransactionType == TransactionType.Transfer ?
                transaction.Amount * -1 : transaction.Amount;

            return new JObject
                {
                    { "Id", transaction.Id },
                    { "Name", transaction.Name },
                    { "Amount", transactionAmount },
                    { "TransactionType", Enum.GetName(
                        typeof(TransactionType), transaction.TransactionType) },
                    { "Owner", OutputHandler
                        .CreateUserJObject(
                            userDataManager
                            .GetUserFromUserId(transaction.OwnerId))},
                    { "ExpenseCategory", expenseCategory },
                    { "IncomeCategory", incomeCategory },
                    { "DateTime", transaction.DateTime },
                    { "RecurringType", transaction.RecurringTransactionId == null?
                        "" : Enum.GetName(typeof(RecurringType),
                                accountDataManager.
                                GetRecurringTransactionById(transaction.RecurringTransactionId)
                                .RecurringSchedule
                                .RecurringType)},
                    { "AccountId", transaction.AccountId },
                    { "IsCleared", transaction.IsCleared },
                    { "AllowedUsers", 
                        OutputHandler.GetAllowedUsersJObjectWithId(
                            id: transaction.AllowedUsersId,
                            allowedUsersDataManager: allowedUsersDataManager,
                            userDataManager: userDataManager)}
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

        #region Private helper methods
        /// <summary>
        /// Converts a semi colon seperated string into a JArray of name/guid pairs.
        /// </summary>
        /// <param name="guids">The string containing a list of guids separated with semicolon.</param>
        /// <param name="userDataManager">The user data manager.</param>
        /// <param name="searchHouseholds">Set to true to search Households in the database.</param>
        /// <param name="searchGroups">Set to true to search Household groups in the database.</param>
        /// <param name="searchUsers">Set to true to search Users in the database.</param>
        private static JArray ConvertStringToJArrayOfNameGuidPairs(
            string guids,
            IUserDataManager userDataManager,
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
                    var name = userDataManager.GetHouseholdWithId(id).Name;

                    jArray.Add(new JObject()
                    {
                        { "Id", id },
                        { "Name", name }
                    });
                }
                else if (searchGroups)
                {
                    var id = Guid.Parse(guid);
                    var name = userDataManager.GetHouseholdGroupWithId(id).Name;

                    jArray.Add(new JObject()
                    {
                        { "Id", id },
                        { "Name", name }
                    });
                }
                else if (searchUsers)
                {
                    var id = Guid.Parse(guid);
                    var user = userDataManager.GetUserFromUserId(id);

                    jArray.Add(OutputHandler.CreateUserJObject(user));
                }
            }

            return jArray;
        }
        #endregion
    }
}