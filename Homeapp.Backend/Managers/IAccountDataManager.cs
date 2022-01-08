
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
        /// Calculates the balance of a specified account.
        /// </summary>
        /// <param name="accountId">The account id.</param>
        public double CalculateAccountBalance(Account account);

        /// <summary>
        /// Creates an account for a user.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="request">The user's account request.</param>
        /// <param name="sharedEntities"></param>
        /// <param name="sharedEntities">The object containing the allowed entities for this account.</param>
        public Task<Account> CreateAccount(User user, CreateAccountRequest request, AllowedUsers sharedEntities);

        /// <summary>
        /// Gets a recurring transaction record from the database using its id.
        /// </summary>
        /// <param name="id">The id of the recurring transaction record.</param>
        public RecurringTransaction GetRecurringTransactionById(Guid id);

        /// <summary>
        /// Creates a transaction record in a specified checkbook account.
        /// </summary>
        /// <param name="accountOwnerId">The id of the account owner.</param>
        /// <param name="accountId">The account id.</param>
        /// <param name="transactionOwnerId">The id of the user who created the transaction.</param>
        /// <param name="request">The request object containing properties to create the record.</param>
        /// <param name="inheritedAllowedUsers">The allowed users list that the transaction will inherit.
        /// Can inherit from the checkbook, or from the request. If coming from the request, it can come from form
        /// data or from a parent RecurringTransaction.</param>
        public Task<Transaction> CreateTransactionInAccount(
            Guid accountOwnerId,
            Guid accountId,
            Guid transactionOwnerId,
            TransactionRequest request,
            AllowedUsers inheritedAllowedUsers = null);

    }
}
