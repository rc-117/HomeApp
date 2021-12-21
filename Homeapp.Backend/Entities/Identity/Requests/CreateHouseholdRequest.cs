namespace Homeapp.Backend.Identity.Requests
{
    using Newtonsoft.Json;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Class containing properties to create a new Household object.
    /// </summary>
    public class CreateHouseholdRequest
    {
        /// <summary>
        /// The name of the household.
        /// </summary>
        [JsonProperty]
        [Required(ErrorMessage = "Name of household is required.")]
        public string Name { get; set; }
        
        /// <summary>
        /// The household group request properties.
        /// </summary>
        [JsonProperty]
        [Required(ErrorMessage = "Household group requests required. An empty array is acceptable if no group creations are requested.")]
        public CreateHouseholdGroupRequest[] HouseholdGroupRequests { get; set; }

        /// <summary>
        /// Hash of the household password.
        /// </summary>
        /// <remarks>
        /// Used to register a new user into an existing household.
        /// </remarks>
        [JsonProperty]
        [Required(ErrorMessage = "Password hash is required.")]
        public string PasswordHash { get; set; }
    }
}
