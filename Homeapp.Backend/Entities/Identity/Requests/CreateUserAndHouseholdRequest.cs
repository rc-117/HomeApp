namespace Homeapp.Backend.Identity.Requests
{
    using Newtonsoft.Json;
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Class containing properties to create a new Household object and User in the application database.
    /// </summary>
    public class CreateUserAndHouseholdRequest
    {
        /// <summary>
        /// The household request properties.
        /// </summary>
        [JsonProperty]
        [Required(ErrorMessage = "Household request required.")]
        public HouseholdRequest HouseholdRequest { get; set; }

        /// <summary>
        /// The user request properties.
        /// </summary>
        [JsonProperty]
        [Required(ErrorMessage = "User request required.")]
        public UserRequest UserRequest { get; set; }
    }
}