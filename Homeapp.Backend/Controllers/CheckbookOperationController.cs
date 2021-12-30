namespace Homeapp.Backend.Controllers
{
    using Homeapp.Backend.Entities;
    using Homeapp.Backend.Identity;
    using Homeapp.Backend.Managers;
    using Homeapp.Backend.Tools;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Newtonsoft.Json;
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
        private IAccountDataManager accountManager;
        private IUserDataManager userDataManager;

        /// <summary>
        /// Initializes CheckbookOperationController.
        /// </summary>
        public CheckbookOperationController(
            IAccountDataManager accountManager,
            IUserDataManager userDataManager)
        {
            this.accountManager = accountManager;
            this.userDataManager = userDataManager;
        }

        /// <summary>
        /// Gets a specified account.
        /// </summary>
        [HttpGet]
        [Route("/api/Checkbook/Accounts/accountId/{accountId}/Get")]
        public IActionResult GetAccount(string accountId)
        {
            Validation.GuidIsValid(guid: accountId, errorMessage: "Invalid account Id.");

            var accountGuid = Guid.Parse(accountId);

            Validation.CheckbookAccountExists(
                Id: accountGuid,
                accountManager: this.accountManager);

            var account = this.accountManager.GetAccountById(accountGuid);

            Validation.AccountBelongsToUser(
                userId: this.GetUserId(),
                account: account);

            var responseBody = new JObject()
            {
                { "AccountId", account.Id },
                { "AccountName", account.Name },
                { "AccountType", Enum.GetName(typeof(AccountType), account.AccountType) },
                { "AccountBalance", this.accountManager.CalculateAccountBalance(account) },
                { "AccountOwner", new JObject() {
                        { "Id", account.UserId },
                        { "Email", account.User.EmailAddress },
                        { "FirstName", account.User.FirstName },
                        { "LastName", account.User.LastName }
                    }
                },
                { 
                    "AccountTransactions", this.accountManager.GetTransactionsJObjectByAccount(account.Id)
                }
            };

            return Ok(responseBody.ToString());
        }


        /// <summary>
        /// Creates an account for a user.
        /// </summary>
        [HttpPut]
        [Route("/api/Checkbook/Accounts/user/{userId}/Create")]
        public async Task<IActionResult> CreateAccountForUser(
            string userId,
            [FromBody] CreateAccountRequest accountRequest)
        {
            Validation.GuidIsValid(
                guid: userId,
                errorMessage: "Invalid user id received.");

            var userIdGuid = Guid.Parse(userId);

            if (this.userDataManager.GetUserFromUserId(userIdGuid) == null)
            {
                return NotFound($"User '{userIdGuid}' not found.");
            }

            var user = this.userDataManager.GetUserFromUserId(userIdGuid);
            var createdAccount = await this.accountManager.CreateAccount(user, accountRequest);
            
            return createdAccount == null ? 
                StatusCode(500, "Error saving account to database.") : 
                Ok(new JObject()
                    {
                        { "Id", createdAccount.Id },
                        { "Name", createdAccount.Name },
                        { "AccountType", ((int)createdAccount.AccountType) },
                        { "UserId", createdAccount.UserId },                      
                    }.ToString());
        }

        /// <summary>
        /// Gets all accounts a user owns.
        /// </summary>
        [HttpGet]
        [Route("/api/Checkbook/Accounts/user/{userId}/GetAll")]
        public IActionResult GetAllAccountsFromUser(string userId)
        {
            var userIdGuid = Guid.TryParse(userId, out Guid guid) == true ? guid : Guid.Empty;

            if (userIdGuid == Guid.Empty)
            {
                return BadRequest("Invalid user Id.");
            }            

            if (this.userDataManager.GetUserFromUserId(userIdGuid) == null)
            {
                return NotFound($"User '{userIdGuid}' not found.");
            }

            var accounts = new Account[]{ };

            try
            {
                accounts = this.accountManager.GetUserAccounts(userIdGuid);
            }
            catch (Exception)
            {
                return StatusCode(500, "Failed to retrieve accounts from database.");
            }

            var accountJArray = new JArray();

            foreach (var account in accounts)
            {
                accountJArray.Add(new JObject()
                {
                    { "AccountId", account.Id },
                    { "AccountName", account.Name },
                    { "OwnerId", account.UserId },
                    { "AccountBalance", this.accountManager.CalculateAccountBalance(account) }
                });
            } 

            return Ok(new JObject()
            {
                { "Accounts", accountJArray }
            }.ToString());
        }

        /// <summary>
        /// Creates a transaction record in a specified account owned by the user.
        /// </summary>
        /// <param name="accountId"></param>
        //[HttpPut]
        //[Route("/api/Checkbook/Accounts/userId/{userId}/accountId/{accountId}/Transactions/Create")]
        //public async Task<IActionResult> CreateAccountTransaction
        //    (string userId,
        //    string accountId)
        //{
        //    var userIdGuid = Guid.TryParse(userId, out Guid guid) == true ? guid : Guid.Empty;
        //    var accountIdGuid = Guid.TryParse(userId, out Guid accountGuid) == true ? accountGuid : Guid.Empty;

        //    if (userIdGuid == Guid.Empty)
        //    {
        //        return BadRequest("Invalid user Id.");
        //    }
        //    else if (this.userDataManager.GetUserFromUserId(userIdGuid) == null)
        //    {
        //        return NotFound($"User '{userIdGuid}' not found.");
        //    }
        //    //else if (this)
        //    //{
        //        // This needs to check if requesting user is in a list of authorized users for the account
        //    //}
        //    if (accountIdGuid == Guid.Empty)
        //    {
        //        return BadRequest("Invalid account Id.");
        //    }
        //}
    }
}