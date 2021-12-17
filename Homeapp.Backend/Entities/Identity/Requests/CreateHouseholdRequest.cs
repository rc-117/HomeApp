namespace Homeapp.Backend.Identity.Requests
{
    using Newtonsoft.Json;
    using System.Collections.Generic;

    /// <summary>
    /// Class containing properties to create a new Household object.
    /// </summary>
    public class CreateHouseholdRequest
    {
        /// <summary>
        /// The name of the household.
        /// </summary>
        [JsonProperty]
        public string Name { get; set; }
        
        /// <summary>
        /// The household group request properties.
        /// </summary>
        [JsonProperty]
        public CreateHouseholdGroupRequest[] HouseholdGroupRequests { get; set; }

        /// <summary>
        /// Hash of the household password.
        /// </summary>
        /// <remarks>
        /// Used to register a new user into an existing household.
        /// </remarks>
        [JsonProperty]
        public string PasswordHash { get; set; }
    }
}
