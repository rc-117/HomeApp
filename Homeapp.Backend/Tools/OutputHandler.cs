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
        #region JSON property names
        private static string _Id = "Id";
        private static string _AccountId = "AccountId";
        private static string _AccountName = "AccountName";
        private static string _AccountType = "AccountType";
        private static string _Balance = "Balance";
        private static string _Owner = "Owner";
        private static string _AllowedUsers = "AllowedUsers";
        private static string _Name = "Name";
        private static string _Amount = "Amount";
        private static string _TransactionType = "TransactionType";
        private static string _ExpenseCategory = "ExpenseCategory";
        private static string _IncomeCategory = "IncomeCategory";
        private static string _DateTime = "DateTime";
        private static string _RecurringTransactionId = "RecurringTransactionId";
        private static string _RecurringSchedule = "RecurringSchedule";
        private static string _IsCleared = "IsCleared";
        private static string _RecurringType = "RecurringType";
        private static string _Time = "Time";
        private static string _DayOfWeek = "DayOfWeek";
        private static string _DateOfMonth = "DateOfMonth";
        private static string _SecondDateOfMonth = "SecondDateOfMonth";
        private static string _AnnualMonth = "AnnualMonth";
        private static string _SecondAnnualMonth = "SecondAnnualMonth";
        private static string _AnnualMonthDate = "AnnualMonthDate";
        private static string _SecondAnnualMonthDate = "SecondAnnualMonthDate";
        private static string _EmailAddress = "EmailAddress";
        private static string _PhoneNumber = "PhoneNumber";
        private static string _FirstName = "FirstName";
        private static string _LastName = "LastName";
        private static string _Age = "Age";
        private static string _Gender = "Gender";
        private static string _Birthday = "Birthday";
        private static string _Creator = "Creator";
        private static string _DateCreated = "DateCreated";
        private static string _TimeCreated = "TimeCreated";
        private static string _HouseholdId = "HouseholdId";
        private static string _HouseholdGroups = "HouseholdGroups";
        private static string _Members = "Members";
        private static string _ReadHousholds;
        #endregion

        #region Common JObject creators
        /// <summary>
        /// Returns a JSON object containing shared/allowed entities for an item. Used for response handling.
        /// </summary>
        /// <param name="id">The id to select the SharedEntities record.</param>
        /// <param name="commonDataManager">The shared entity data manager.</param>
        /// <param name="userDataManager">The user data manager.</param>
        public static JObject CreateAllowedUsersJObject(Guid id, ICommonDataManager commonDataManager, IUserDataManager userDataManager)
        {
            var allowedUsers = commonDataManager.GetAllowedUsersObjectFromId(id);

            var readHouseholdIds = OutputHandler.ConvertStringToGuidList(allowedUsers.ReadHouseholdIds);
            var writeHouseholdIds = OutputHandler.ConvertStringToGuidList(allowedUsers.WriteHouseholdIds);
            var fullAccessHouseholdIds = OutputHandler.ConvertStringToGuidList(allowedUsers.FullAccessHouseholdIds);

            var readHouseholdGroupsIds = OutputHandler.ConvertStringToGuidList(allowedUsers.ReadHouseholdGroupIds);
            var writeHouseholdGroupsIds = OutputHandler.ConvertStringToGuidList(allowedUsers.WriteHouseholdGroupIds);
            var fullAccessHouseholdGroupsIds = OutputHandler.ConvertStringToGuidList(allowedUsers.FullAccessHouseholdGroupIds);

            var readUserIds = OutputHandler.ConvertStringToGuidList(allowedUsers.ReadUserIds);
            var writeUserIds = OutputHandler.ConvertStringToGuidList(allowedUsers.WriteUserIds);
            var fullAccessUserIds = OutputHandler.ConvertStringToGuidList(allowedUsers.FullAccessUserIds);

            return new JObject()
            {
                { _ReadHousholds, OutputHandler. },
                { _Wi }
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
                { _Id, user.Id },
                { _EmailAddress, user.EmailAddress },
                { _PhoneNumber, 
                    string.IsNullOrWhiteSpace(user.PhoneNumber) 
                    ? null : user.PhoneNumber },
                { _FirstName, user.FirstName },
                { _LastName, user.LastName },
                { _Age, OutputHandler.GetAgeFromBirthday(user.Birthday) },
                { _Gender, Enum.GetName(typeof(Gender), user.Gender) },
                { _Birthday, user.Birthday.ToLongDateString() }
            };
        }

        /// <summary>
        /// Creates a JSON array of users.
        /// </summary>
        /// <param name="users">The list of users to add to the JSON array.</param>
        /// <returns>JArray containing users.</returns>
        public static JArray CreateUserJArray(List<User> users)
        {
            var jArray = new JArray();

            foreach (var user in users)
            {
                jArray.Add(OutputHandler.CreateUserJObject(user));
            }

            return jArray;
        }

        /// <summary>
        /// Creates a JSON object containing details about a household group.
        /// </summary>
        /// <param name="group">The household group.</param>
        public static JObject CreateHouseholdGroupJObject(
            HouseholdGroup group, 
            IUserDataManager userDataManager, 
            ICommonDataManager commonDataManager,
            bool includeAllowedUsers)
        {
            var jObject = new JObject()
            {
                { _Id, group.Id },
                { _HouseholdId, group.HouseholdId },
                { _Name, group.Name },
                { _Creator, 
                    OutputHandler.
                        CreateUserJObject(
                            userDataManager
                            .GetUserFromUserId(group.CreatorId)) },
                { _Members, OutputHandler.CreateUserJArray(userDataManager.GetHouseholdGroupUsers(group.Id)) },
                { _DateCreated, group.DateTimeCreated.ToLongDateString() },
                { _TimeCreated, group.DateTimeCreated.ToShortTimeString() },
            };

            if (includeAllowedUsers)
            {
                jObject.Add(new JObject()
                    {
                        _AllowedUsers,
                            OutputHandler
                                .CreateAllowedUsersJObject(
                                id: group.AllowedUsersId,
                                commonDataManager: commonDataManager,
                                userDataManager: userDataManager)
                    });
            }

        }

        /// <summary>
        /// Creates a JSON array of household groups.
        /// </summary>
        /// <param name="groups">The list of household groups to add to the JSON array.</param>
        /// <param name="userDataManager">The user data manager.</param>
        /// <returns>JArray containing household groups.</returns>
        public static JArray CreateHouseholdGroupJArray(List<HouseholdGroup> groups, IUserDataManager userDataManager)
        {
            var jArray = new JArray();

            foreach (var group in groups)
            {
                jArray.Add
                    (OutputHandler.CreateHouseholdGroupJObject(group, userDataManager));
            }

            return jArray;
        }

        /// <summary>
        /// Creates a JSON object containing details about a household.
        /// </summary>
        /// <param name="household">The household.</param>
        public static JObject CreateHouseholdJObject(Household household, IUserDataManager userDataManager)
        {
            return new JObject()
            {
                { _Id, household.Id },
                { _Name, household.Name },
                { _HouseholdGroups, 
                    OutputHandler
                        .CreateHouseholdGroupJArray(
                            groups: 
                                userDataManager
                                .GetGroupsFromHousehold(household.Id), 
                            userDataManager: userDataManager) },
                { _Creator,
                    OutputHandler
                        .CreateUserJObject(
                            userDataManager
                            .GetUserFromUserId(household.CreatorId)) },
                { _Members, 
                    OutputHandler
                        .CreateUserJArray(
                            userDataManager
                                .GetUsersFromHousehold(household.Id)) },
                { _DateCreated, household.DateTimeCreated.ToLongDateString() },
                { _TimeCreated, household.DateTimeCreated.ToShortTimeString() },
                //{ _AllowedUsers, OutputHandler.CreateAllowedUsersJObject() },
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
            ICommonDataManager sharedEntityDataManager,
            IUserDataManager userDataManager)
        {
            return new JObject()
            {
                { _Id, account.Id },
                { _AccountName, account.Name },
                { _AccountType, Enum.GetName(typeof(AccountType), account.AccountType) },
                { _Balance, accountDataManager.CalculateAccountBalance(account) },
                { _Owner, OutputHandler.CreateUserJObject(accountOwner) },
                {
                   _AllowedUsers, OutputHandler.CreateAllowedUsersJObject(
                       id: account.AllowedUsersId,
                       commonDataManager: sharedEntityDataManager,
                       userDataManager: userDataManager)
                }
            };
        }

        /// <summary>
        /// Creates a JArray containing all transactions in an account.
        /// </summary>
        /// <param name="accountId">The account id.</param>
        /// <param name="accountDataManager">The account data manager.</param>
        /// <param name="userDataManager">The user data manager.</param>
        /// <param name="sharedEntityDataManager">The shared entity data manager.</param>
        public static JArray CreateTransactionsJArrayByAccountId(
            Guid accountId, 
            IAccountDataManager accountDataManager, 
            IUserDataManager userDataManager,
            ICommonDataManager sharedEntityDataManager)
        {
            var transactions = accountDataManager.GetTransactionsByAccount(accountId);
            var jArray = new JArray();
            foreach (var transaction in transactions)
            {
                jArray.Add(CreateTransactionJObject(
                    transaction: transaction,
                    userDataManager: userDataManager,
                    accountDataManager: accountDataManager,
                    commonDataManager: sharedEntityDataManager));
            }
            return jArray;
        }

        /// <summary>
        /// Creates a JSON object with a transaction's properties.
        /// </summary>
        /// <param name="transaction">The transaction object to use.</param>
        /// <param name="userDataManager">The user data manager.</param>
        /// <param name="accountDataManager">The checkbook account data manager.</param>
        /// <param name="commonDataManager">The allowed users data manager.</param>
        public static JObject CreateTransactionJObject(
            Transaction transaction, 
            IUserDataManager userDataManager,
            IAccountDataManager accountDataManager,
            ICommonDataManager commonDataManager)
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
                    { _Id, transaction.Id },
                    { _Name, transaction.Name },
                    { _Amount, transactionAmount },
                    { _TransactionType, Enum.GetName(
                        typeof(TransactionType), transaction.TransactionType) },
                    { _Owner, OutputHandler
                        .CreateUserJObject(
                            userDataManager
                            .GetUserFromUserId(transaction.OwnerId))},
                    { _ExpenseCategory, expenseCategory },
                    { _IncomeCategory, incomeCategory },
                    { _DateTime, transaction.DateTime },
                    { _RecurringTransactionId, transaction.RecurringTransactionId == null?
                        null : transaction.RecurringTransactionId.ToString() },
                    { _RecurringSchedule, transaction.RecurringTransactionId == null?
                        null : OutputHandler
                        .CreateRecurringScheduleJObject(
                            commonDataManager.GetRecurringScheduleById(
                                accountDataManager.GetRecurringTransactionById(
                                    transaction.RecurringTransactionId).RecurringScheduleId)) },
                    { _AccountId, transaction.AccountId },
                    { _IsCleared, transaction.IsCleared },
                    { _AllowedUsers, 
                        OutputHandler.CreateAllowedUsersJObject(
                            id: transaction.AllowedUsersId,
                            commonDataManager: commonDataManager,
                            userDataManager: userDataManager)}
                };
        }

        /// <summary>
        /// Creates a JSON object with a recurring schedule's properties.
        /// </summary>
        /// <param name="schedule">The recurring schedule.</param>
        public static JObject CreateRecurringScheduleJObject(RecurringSchedule schedule)
        {
            return new JObject()
            {
                { _Id, schedule.Id },
                { _RecurringType, Enum.GetName(typeof(RecurringType), schedule.RecurringType) },
                { _Time, string.Format("{0}:{1}", schedule.Hours, schedule.Minutes) },
                { _DayOfWeek, schedule.DayOfWeek == null?
                    null : schedule.DayOfWeek.ToString() },
                { _DateOfMonth, schedule.DateOfMonth == null?
                    null : schedule.DateOfMonth },
                { _SecondDateOfMonth, schedule.SecondDateOfMonth == null?
                    null : schedule.SecondDateOfMonth },
                { _AnnualMonth, schedule.AnnualMonth == null?
                    null : schedule.AnnualMonth },
                { _SecondAnnualMonth, schedule.SecondAnnualMonth == null?
                    null : schedule.SecondAnnualMonth },
                { _DateOfMonth, schedule.DateOfMonth == null?
                    null : schedule.DateOfMonth },
                { _AnnualMonthDate, schedule.AnnualMonthDate == null?
                    null : schedule.AnnualMonthDate },
                { _SecondAnnualMonthDate, schedule.SecondAnnualMonthDate == null?
                    null : schedule.SecondAnnualMonthDate },
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

        /// <summary>
        /// Adds a guid to a semicolon separated string list.
        /// </summary>
        /// <param name="guid">The guid to add.</param>
        /// <param name="list">The list to add to.</param>
        /// <returns>The string with the added guid.</returns>
        public static string AddGuidToSemiColonSeparatedStringList(Guid guid, string list)
        {
            var ids = OutputHandler.ConvertStringToGuidList(list);
            
            if (!ids.Contains(guid))
            {
                ids.Add(guid);
            } 

            return OutputHandler.ConvertGuidListToString(ids);
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