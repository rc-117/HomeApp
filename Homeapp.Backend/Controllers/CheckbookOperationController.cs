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
        private ISharedEntityDataManager sharedEntityDataManager;
        private AppDbContext appDbContext;

        /// <summary>
        /// Initializes CheckbookOperationController.
        /// </summary>
        /// <param name="accountManager">The account data manager.</param>
        /// <param name="userDataManager">The user data manager.</param>
        /// <param name="sharedEntityDataManager">The shared entity data manager.</param>
        /// <param name="appDbContext">The application database context.</param>
        public CheckbookOperationController(
            IAccountDataManager accountManager,
            IUserDataManager userDataManager,
            ISharedEntityDataManager sharedEntityDataManager,
            AppDbContext appDbContext)
        {
            this.accountManager = accountManager;
            this.userDataManager = userDataManager;
            this.sharedEntityDataManager = sharedEntityDataManager;
            this.appDbContext = appDbContext;
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

            var account = this.accountManager.GetAccountById(
                accountId: accountGuid);

            var owner = this.userDataManager.GetUserFromUserId(
                userId: account.UserId);

            Validation.AccountBelongsToUser(
                userId: this.GetUserId(),
                account: account);

            var responseBody = OutputHandler.CreateCheckbookAccountJObject(
                account: account,
                accountOwner: owner,
                accountDataManager: this.accountManager,
                sharedEntityDataManager: this.sharedEntityDataManager);

            return Ok(responseBody.ToString());
        }


        /// <summary>
        /// Creates an account for a user.
        /// </summary>
        [HttpPut]
        [Route("/api/Checkbook/Accounts/user/{userId}/Create")]
        public async Task<IActionResult> CreateChecbookAccountForUser(
            string userId,
            [FromBody] CreateAccountRequest accountRequest)
        {
            Validation.GuidIsValid(
                guid: userId,
                errorMessage: "Invalid user id received.");

            var userIdGuid = Guid.Parse(userId);

            Validation.UserExists(
                userId: userIdGuid,
                appDbContext: this.appDbContext);

            Validation.SharedEntitiesRequestIsValid(
                request: accountRequest.SharedEntitiesRequest,
                appDbContext: this.appDbContext);

            var user = this.userDataManager.GetUserFromUserId(userIdGuid);

            // TODO: Write code somewhere here to undo creating this SharedEntities object if the
            // CreateAccount method doesnt save to db for some reason
            var sharedEntities = this.sharedEntityDataManager.CreateNewSharedEntitiesObject(
                    request: accountRequest.SharedEntitiesRequest);

            var createdAccount = await this.accountManager.CreateAccount(
                user: user, 
                request: accountRequest,
                sharedEntities: sharedEntities);
            
            return Ok(new JObject()
                    {
                        { "Id", createdAccount.Id },
                        { "Name", createdAccount.Name },
                        { "AccountType", ((int)createdAccount.AccountType) },
                        { "UserId", createdAccount.UserId },
                        { "AllowedUsers", new JObject(){
                            { "ReadHouseholds", createdAccount.SharedEntities.ReadHouseholdIds },
                            { "ReadHouseholdGroups", createdAccount.SharedEntities.ReadHouseholdGroupIds },
                            { "ReadUsers", createdAccount.SharedEntities.ReadUserIds },
                            { "EditHouseholds", createdAccount.SharedEntities.EditHouseholdIds },
                            { "EditHouseholdGroups", createdAccount.SharedEntities.EditHouseholdGroupIds },
                            { "EditUsers", createdAccount.SharedEntities.EditUserIds },
                        }}
                    }.ToString());
        }

        /// <summary>
        /// Gets all accounts a user owns.
        /// </summary>
        [HttpGet]
        [Route("/api/Checkbook/Accounts/user/{userId}/GetAll")]
        public IActionResult GetAllChecbookAccountsFromUser(string userId)
        {
            Validation.GuidIsValid(
                guid: userId,
                errorMessage: "Invalid user id received.");

            var userIdGuid = Guid.Parse(userId);

            Validation.UserExists(
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