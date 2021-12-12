namespace Homeapp.Backend.Controllers
{
    using Homeapp.Backend.Managers;
    using Homeapp.Backend.Tools;
    using Homeapp.Test;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Newtonsoft.Json.Linq;
    using System;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Text;
    using System.Threading.Tasks;
    using System.Web;
    
    /// <summary>
    /// The checkbook operations controller.
    /// </summary>
    [ApiController]
    public class CheckbookOperationController : HomeappControllerBase
    {
        private IAccountManager accountManager;

        /// <summary>
        /// Initializes CheckbookOperationController.
        /// </summary>
        public CheckbookOperationController(IAccountManager accountManager)
        {
            this.accountManager = accountManager;
        }



        /// <summary>
        /// Gets a specified account.
        /// </summary>
        [HttpGet]
        [Route("/api/Checkbook/Accounts/{accountId}/Get")]
        public IActionResult GetAccount(string accountId)
        {
            var accountGuid = Guid.Empty;

            if (!Validation.GuidIsValid(accountId, out Guid guid))
            {
                return BadRequest("Invalid account Id.");
            }
            else
            {
                accountGuid = guid;
            }

            var account = this.accountManager.GetAccountById(accountGuid);

            if (account == null)
            {
                return NotFound($"Account with Id '{accountId}' was not found");
            }
            else if (!Validation.AccountBelongsToUser(this.GetUserId(), account))
            {
                return Unauthorized($"User unauthorized to view the specified account.");
            }

            // When expenses are implemented need to add a field here "current balance"
            // then in accountmanager need to calculate the current balance (starting balance - alltransactions)
            // Then need to include an array of expenses going as far back as 3 months

            var transactions = this.accountManager.GetTransactionsByAccount(account.Id);

            var responseBody = new JObject()
            {
                { "AccountId", account.Id },
                { "AccountName", account.Name },
                { "AccountOwner", new JObject() {
                        { "UserId", account.UserId },
                        { "UserEmail", account.User.EmailAddress },
                        { "UserFirstName", account.User.FirstName },
                        { "UserLastName", account.User.LastName }
                    }
                },
                { "AccountTransactions", new JObject()
                    {
                        transactions
                    } 
                }
            };

            return Ok(responseBody.ToString());
        }
    }
}
