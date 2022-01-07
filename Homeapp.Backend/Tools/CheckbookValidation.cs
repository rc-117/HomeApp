namespace Homeapp.Backend.Tools
{
    using Homeapp.Backend.Db;
    using Homeapp.Backend.Managers;
    using System;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Web.Http;

    /// <summary>
    /// Validation class handling all checkbook/transaction related validation tasks.
    /// </summary>
    public static class CheckbookValidation
    {
        /// <summary>
        /// Validates that a specified checkbook account exists in the database.
        /// </summary>
        /// <param name="Id">The Id of the checkbook.</param>
        /// <param name="appDbContext">The application database context.</param>
        public static void CheckbookAccountExists(Guid Id, AppDbContext appDbContext)
        {
            var account = appDbContext.Accounts.FirstOrDefault(a => a.Id == Id);
            if (account == null)
            {
                throw new HttpResponseException(
                    new HttpResponseMessage(HttpStatusCode.NotFound)
                    {
                        Content = new StringContent($"Checkbook account with id '{Id}' was not found."),
                        ReasonPhrase = HttpReasonPhrase
                            .GetPhrase(ReasonPhrase.CheckbookAccountNotFound)
                    });
            }
        }
    }
}