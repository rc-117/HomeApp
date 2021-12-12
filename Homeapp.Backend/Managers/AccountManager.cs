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
    }
}
