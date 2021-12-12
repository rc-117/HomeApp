namespace Homeapp.Backend.Controllers
{
    using Homeapp.Test;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using System;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Threading.Tasks;
    using System.Web;
    
    /// <summary>
    /// The checkbook operations controller.
    /// </summary>
    [ApiController]
    public class CheckbookOperationController : Controller
    {
        /// <summary>
        /// Initializes CheckbookOperationController.
        /// </summary>
        public CheckbookOperationController()
        {

        }

        /// <summary>
        /// Gets a specified account.
        /// </summary>
        [HttpGet]
        [Route("/api/Checkbook/Accounts/{accountId}/Get")]
        public IActionResult GetAccount(string accountId)
        {
            var accountGuid = new Guid();

            try
            {
                accountGuid = Guid.Parse(accountId);
            }
            catch (Exception)
            {
                return BadRequest("Invalid account Id.");
            }
                           
            
            var account = TestRepo.Accounts
                .FirstOrDefault(account => account.Id == accountGuid);

            if (account == null)
            {
                return NotFound($"Account with Id '{accountId}' was not found");
            }

            return Ok(account);
        }
    }
}
