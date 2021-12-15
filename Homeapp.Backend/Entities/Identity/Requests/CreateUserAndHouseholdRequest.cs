namespace Homeapp.Backend.Identity.Requests
{
    using Newtonsoft.Json;

    /// <summary>
    /// Class containing properties to create a new Household object and User in the application database.
    /// </summary>
    public class CreateUserAndHouseholdRequest
    {
        /// <summary>
        /// The household request properties.
        /// </summary>
        [JsonProperty]
        public CreateHouseholdRequest HouseholdRequest { get; set; }

        /// <summary>
        /// The user request properties.
        /// </summary>
        [JsonProperty]
        public CreateUserRequest UserRequest { get; set; }
    }
}
