
namespace Homeapp.Backend.Managers
{
    using Homeapp.Backend.Entities;
    using Homeapp.Backend.Identity;
    using Newtonsoft.Json.Linq;
    using System;
    using System.Threading.Tasks;

    /// <summary>
    /// The Account Manager interface.
    /// </summary>
    public interface IAccountDataManager
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

        /// <summary>
        /// Gets all transactions from an account.
        /// </summary>
        /// <param name="userId">The account id.</param>
        public JArray GetTransactionsJObjectByAccount(Guid accountId);


        /// <summary>
        /// Calculates the balance of a specified account.
        /// </summary>
        /// <param name="accountId">The account id.</param>
        public double CalculateAccountBalance(Guid accountId);

        /// <summary>
        /// Creates an account for a user.
        /// </summary>
        /// <param name="userId">The user's id.</param>
        /// <param name="request">The user's account request.</param>
        public Task<Account> CreateAccount(Guid userId, CreateAccountRequest request);

    }
}
