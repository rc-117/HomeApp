namespace Homeapp.Backend.Identity.Responses
{
    using Newtonsoft.Json.Linq;

    /// <summary>
    /// Response object containing a User and Househould.
    /// </summary>
    public class CreateUserAndHouseholdResponse
    {
        /// <summary>
        /// JSON object containing the user. 
        /// </summary>
        public JObject User { get; set; }

        /// <summary>
        /// JSON object containing the household.
        /// </summary>
        public JObject Household { get; set; }
    }
}
