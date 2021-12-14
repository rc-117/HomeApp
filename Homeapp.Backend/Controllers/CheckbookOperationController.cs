﻿namespace Homeapp.Backend.Controllers
{
    using Homeapp.Backend.Entities;
    using Homeapp.Backend.Identity;
    using Homeapp.Backend.Managers;
    using Homeapp.Backend.Tools;
    using Homeapp.Test;
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

  
            var responseBody = new JObject()
            {
                { "AccountId", account.Id },
                { "AccountName", account.Name },
                { "AccountType", Enum.GetName(typeof(AccountType), account.AccountType) },
                { "AccountBalance", this.accountManager.CalculateAccountBalance(account.Id) },
                { "AccountOwner", new JObject() {
                        { "UserId", account.UserId },
                        { "UserEmail", account.User.EmailAddress },
                        { "UserFirstName", account.User.FirstName },
                        { "UserLastName", account.User.LastName }
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
            var userIdGuid = Guid.Empty;

            if (!Validation.GuidIsValid(userId, out Guid guid))
            {
                return BadRequest("Invalid user Id.");
            }
            else
            {
                userIdGuid = guid;
            }

            if (this.userDataManager.GetUserFromUserId(userIdGuid) == null)
            {
                return NotFound($"User '{userIdGuid}' not found.");
            }

            var user = this.userDataManager.GetUserFromUserId(userIdGuid);
                        
            var createdAccount = await this.accountManager.CreateAccount(user, accountRequest);
            
            return createdAccount == null ? 
                Ok("Error saving account to database.") : 
                Ok(new JObject()
                    {
                        { "Id", createdAccount.Id },
                        { "Name", createdAccount.Name },
                        { "AccountType", ((int)createdAccount.AccountType) },
                        { "UserId", createdAccount.UserId },                      
                    }.ToString());
        }
    }
}
