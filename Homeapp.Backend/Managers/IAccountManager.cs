
namespace Homeapp.Backend.Managers
{
    using Homeapp.Backend.Entities;
    using System;

    /// <summary>
    /// The Account Manager interface.
    /// </summary>
    public interface IAccountManager
    {
        /// <summary>
        /// Gets an account by its id.
        /// </summary>
        /// <param name="accountId">The account id.</param>
        public Account GetAccountById(Guid accountId);

        /// <summary>
        /// Gets all of a user's accounts.
        /// </summary>
        /// <param name="userId">The user's id.</param>
        public Account[] GetUserAccounts(Guid userId);

        /// <summary>
        /// Gets all transactions from an account.
        /// </summary>
        /// <param name="userId">The account id.</param>
        public Transaction[] GetTransactionsByAccount(Guid accountId);
    }
}
