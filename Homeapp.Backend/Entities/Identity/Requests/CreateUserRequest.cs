namespace Homeapp.Backend.Identity.Requests
{
    using Newtonsoft.Json;
    using System.Collections.Generic;

    /// <summary>
    /// Class containing properties to create a new User in the application database.
    /// </summary>
    public class CreateUserRequest
    {
        /// <summary>
        /// The user's email address.
        /// </summary>
        [JsonProperty]
        public string EmailAddress { get; set; }

        /// <summary>
        /// Hash of the user's password.
        /// </summary>
        [JsonProperty]
        public string PasswordHash { get; set; }

        /// <summary>
        /// The user's first name. 
        /// </summary>
        [JsonProperty]
        public string FirstName { get; set; }

        /// <summary>
        /// The user's last name. 
        /// </summary>
        [JsonProperty]
        public string LastName { get; set; }

        /// <summary>
        /// The user's gender.
        /// </summary>
        public Gender Gender { get; set; }

        /// <summary>
        /// The household that the user is requesting to join.
        /// </summary>
        /// <remarks>
        /// The user must be a member of one household, so on user creation the user must create or request to join an existing household.
        /// </remarks>
        [JsonProperty]
        public Household Household { get; set; }

        /// <summary>
        /// The household group that the user requesting to join.
        /// </summary>
        /// The user is not required to be a member of any household group.
        [JsonProperty]
        public HouseholdGroup HouseholdGroup { get; set; }
    }
}
