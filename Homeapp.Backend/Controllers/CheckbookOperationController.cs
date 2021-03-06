namespace Homeapp.Backend.Controllers
{
    using Homeapp.Backend.Db;
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
        private ICommonDataManager allowedUsersDataManager;
        private AppDbContext appDbContext;

        /// <summary>
        /// Initializes CheckbookOperationController.
        /// </summary>
        /// <param name="accountManager">The account data manager.</param>
        /// <param name="userDataManager">The user data manager.</param>
        /// <param name="allowedUsersDataManager">The shared entity data manager.</param>
        /// <param name="appDbContext">The application database context.</param>
        public CheckbookOperationController(
            IAccountDataManager accountManager,
            IUserDataManager userDataManager,
            ICommonDataManager allowedUsersDataManager,
            AppDbContext appDbContext)
        {
            this.accountManager = accountManager;
            this.userDataManager = userDataManager;
            this.allowedUsersDataManager = allowedUsersDataManager;
            this.appDbContext = appDbContext;
        }

        /// <summary>
        /// Gets a specified account.
        /// </summary>
        [HttpGet]
        [Route("/api/Checkbook/Accounts/accountId/{accountId}/Get")]
        public IActionResult GetAccount(string accountId)
        {
            CommonValidation.GuidIsValid(guid: accountId, errorMessage: "Invalid account Id.");

            var accountGuid = Guid.Parse(accountId);

            CheckbookValidation.CheckbookAccountExists(
                Id: accountGuid,
                appDbContext: this.appDbContext);

            var account = this.accountManager.GetAccountById(
                accountId: accountGuid);

            var owner = this.userDataManager.GetUserById(
                userId: account.OwnerId);

            IdentityValidation.UserHasReadAccessToResource(
                requestingUser: this.userDataManager
                    .GetUserById(this.GetUserId()),
                ownerId: account.OwnerId,
                sharedEntities: 
                    this.allowedUsersDataManager
                    .GetAllowedUsersObjectFromId(account.AllowedUsersId),
                errorMessage: 
                    $"Requesting user does not have read access on account with id: '{account.Id}'.");

            var responseBody = OutputHandler.CreateCheckbookAccountJObject(
                account: account,
                accountOwner: owner,
                accountDataManager: this.accountManager,
                sharedEntityDataManager: this.allowedUsersDataManager,
                userDataManager: this.userDataManager);

            return Ok(responseBody.ToString());
        }


        /// <summary>
        /// Creates an account for a user.
        /// </summary>
        [HttpPut]
        [Route("/api/Checkbook/Accounts/Create")]
        public async Task<IActionResult> CreateCheckbookAccountForUser(
            [FromBody] CreateAccountRequest accountRequest)
        {
            IdentityValidation.UserExists(
                userId: this.GetUserId(),
                appDbContext: this.appDbContext);

            var user = this.userDataManager.GetUserById(userId: this.GetUserId());

            IdentityValidation.ValidateAllowedUsersRequest(
                request: accountRequest.AllowedUsersRequest,
                appDbContext: this.appDbContext);

            var allowedUsers = await this.allowedUsersDataManager
                .SaveAllowedUsersObjectToDb(request: accountRequest.AllowedUsersRequest);

            var createdAccount = await this.accountManager.CreateAccount(
                user: user, 
                request: accountRequest,
                allowedUsers: allowedUsers);
            
            var response = OutputHandler.CreateCheckbookAccountJObject(
                account: createdAccount,
                accountOwner: user,
                accountDataManager: accountManager,
                sharedEntityDataManager: this.allowedUsersDataManager,
                userDataManager: this.userDataManager);
                
            return Ok(response.ToString());
        }

        /// <summary>
        /// Gets all accounts a user owns.
        /// </summary>
        [HttpGet]
        [Route("/api/Checkbook/Accounts/user/{userId}/GetAll")]
        public IActionResult GetAllChecbookAccountsFromUser(string userId)
        {
            CommonValidation.GuidIsValid(
                guid: userId,
                errorMessage: "Invalid user id received.");

            var userIdGuid = Guid.Parse(userId);

            IdentityValidation.UserExists(
                userId: userIdGuid, 
                appDbContext: this.appDbContext);
            
            var accounts = new Account[]{ };

            try
            {
                accounts = this.accountManager.GetUserAccounts(userIdGuid);
            }
            catch (Exception)
            {
                return StatusCode(500, "Failed to retrieve checkbook accounts from database. Please try the request again.");
            }

            var accountJArray = new JArray();

            foreach (var account in accounts)
            {
                accountJArray.Add(new JObject()
                {
                    { "AccountId", account.Id },
                    { "AccountName", account.Name },
                    { "OwnerId", account.OwnerId },
                    { "AccountBalance", this.accountManager.CalculateAccountBalance(account) }
                });
            } 

            return Ok(new JObject()
            {
                { "Accounts", accountJArray }
            }.ToString());
        }

        /// <summary>
        /// Creates a transaction record in a specified account that the requesting user has access to.
        /// </summary>
        /// <param name="userId">The id of the user that owns the account.</param>
        /// <param name="accountId">The id of the account.</param>
        /// <param name="request">The request body containing properties to create a checkbook transaction record.</param>
        [HttpPut]
        [Route("/api/Checkbook/Accounts/userId/{userId}/accountId/{accountId}/Transactions/Create")]
        public async Task<IActionResult> CreateAccountTransaction
            (string userId,
            string accountId,
            TransactionRequest request)
        {
            CommonValidation.GuidIsValid(guid: userId, errorMessage: "Invalid user id received.");
            CommonValidation.GuidIsValid(guid: accountId, errorMessage: "Invalid account id received.");

            var account = this.accountManager.GetAccountById(Guid.Parse(accountId));

            IdentityValidation.UserHasWriteAccessToResource(
                requestingUser: this.userDataManager.GetUserById(this.GetUserId()),
                ownerId: account.OwnerId,
                sharedEntities: this.allowedUsersDataManager
                    .GetAllowedUsersObjectFromId(account.AllowedUsersId),
                errorMessage: $"The requesting user does not have write access to account with id: '{account.Id}'");

            CheckbookValidation.ValidateTransactionRequest(
                request: request,
                appDbContext: this.appDbContext);

            var transaction = 
                await this.accountManager
                    .CreateTransactionInAccount(
                    accountOwnerId: account.OwnerId,
                    accountId: account.Id,
                    transactionOwnerId: this.GetUserId(),
                    request: request,
                    allowedUsers:
                        request.InheritAllowedUsersFromCheckbook ?
                        this.allowedUsersDataManager
                            .GetAllowedUsersObjectFromId(account.AllowedUsersId) :
                        await this.allowedUsersDataManager.SaveAllowedUsersObjectToDb(request.AllowedUsersRequest));

            return Ok(OutputHandler.CreateTransactionJObject(
                transaction: transaction,
                userDataManager: this.userDataManager,
                accountDataManager: this.accountManager,
                commonDataManager: this.allowedUsersDataManager)
                .ToString());
        }
    }
}