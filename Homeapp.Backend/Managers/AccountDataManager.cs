namespace Homeapp.Backend.Managers
{
    using Homeapp.Backend.Entities;
    using Homeapp.Test;
    using Newtonsoft.Json.Linq;
    using System;
    using System.Linq;

    /// <summary>
    /// The Account Manager.
    /// </summary>
    public class AccountDataManager : IAccountDataManager
    {
        /// <summary>
        /// The user data manager.
        /// </summary>
        private IUserDataManager userDataManager;

        public AccountDataManager(IUserDataManager userDataManager)
        {
            this.userDataManager = userDataManager;
        }

        /// <summary>
        /// Gets an account by its id.
        /// </summary>
        /// <param name="accountId"></param>
        /// <returns>An account, if any. Returns null if no account is found.</returns>
        public Account GetAccountById(Guid accountId)
        {
            //Test repo code
            return TestRepo.Accounts
                .FirstOrDefault(account => account.Id == accountId);
        }

        /// <summary>
        /// Gets all of a user's accounts.
        /// </summary>
        /// <param name="userId">The user's id.</param>
        public Account[] GetUserAccounts(Guid userId) 
        {
            //Test repo code
            return TestRepo.Accounts
                .Where(account => account.UserId == userId)
                .ToArray();
        }

        /// <summary>
        /// Gets all transactions from an account.
        /// </summary>
        /// <param name="userId">The account id.</param>
        public Transaction[] GetTransactionsByAccount(Guid accountId)
        {
            //test code
            var testGenerator = new TransactionGenerator();
            return testGenerator.Transactions
                .Where(transaction => transaction.AccountId == accountId)
                .ToArray();
        }

        /// <summary>
        /// Gets all transactions from an account.
        /// </summary>
        /// <param name="userId">The account id.</param>
        public JArray GetTransactionsJObjectByAccount(Guid accountId)
        {
            var account = this.GetAccountById(accountId);
            var transactions = this.GetTransactionsByAccount(accountId);
            var jArray = new JArray();
            foreach (var transaction in transactions)
            {
                var incomeCategory = transaction.IncomeCategory == null ?
                    null : transaction.IncomeCategory.Name;

                var expenseCategory = transaction.ExpenseCategory == null ?
                    null : transaction.ExpenseCategory.Name;

                var transactionType = Enum.GetName(typeof(TransactionType), transaction.TransactionType);

                var transactionAmount = 
                    transaction.TransactionType == TransactionType.Expense || 
                    transaction.TransactionType == TransactionType.Transfer ?
                    transaction.Amount * -1 : transaction.Amount;

                jArray.Add(new JObject
                {
                    { "Id", transaction.Id },
                    { "Name", transaction.Name },
                    { "Amount", transactionAmount },
                    { "TransactionType", transactionType },
                    { "Owner", this.userDataManager.CreateShortUserJobjectFromUser(account.User) },
                    { "ExpenseCategory", expenseCategory },
                    { "IncomeCategory", incomeCategory },
                    { "DateTime", transaction.DateTime },
                    { "RecurringType", Enum.GetName(typeof(RecurringType), transaction.RecurringType) },
                    { "AccountId", transaction.AccountId },
                    { "IsCleared", transaction.IsCleared }
                });
            }
            return jArray;
        }

        /// <summary>
        /// Calculates the balance of a specified account.
        /// </summary>
        /// <param name="accountId">The account id.</param>
        public double CalculateAccountBalance(Guid accountId)
        {
            double totalincome = 0;
            double totalexpense = 0;

            var incomeList = this.GetTransactionsByAccount(accountId)
                .Where(t => t.TransactionType == TransactionType.Income);

            var expenseList = this.GetTransactionsByAccount(accountId)
                .Where(t => t.TransactionType == TransactionType.Expense ||
                            t.TransactionType == TransactionType.Transfer);

            foreach (var income in incomeList)
            {
                totalincome += income.Amount;
            }
            foreach (var expense in expenseList)
            {
                totalexpense += expense.Amount;
            }

            return GetAccountById(accountId)
                .StartingBalance 
                + totalincome 
                - totalexpense;
        }
    }
}
