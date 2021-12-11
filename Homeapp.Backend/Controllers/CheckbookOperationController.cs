namespace Homeapp.Backend.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using System.Net;
    using System.Net.Http;
    using System.Threading.Tasks;

    /// <summary>
    /// The checkbook operations controller.
    /// </summary>
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
        /// <returns></returns>
        public async Task<HttpResponseMessage> GetAccount()
        {
            return new HttpResponseMessage(HttpStatusCode.OK);
        }
    }
}
