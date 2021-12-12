namespace Homeapp.Backend.Managers
{
    using Homeapp.Backend.Entities;
    using Homeapp.Test;
    using System;
    using System.Linq;

    /// <summary>
    /// The Account Manager.
    /// </summary>
    public class AccountManager : IAccountManager
    {
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
    }
}
