
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
        /// <param name="accountId"></param>
        /// <returns>An account, if any. Returns null if no account is found.</returns>
        public Account GetAccountById(Guid accountId);
    }
}
