namespace Homeapp.Backend.Identity.Requests
{
    using Newtonsoft.Json;
    using System;
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
        /// The user's birthday in m/d/yyyy format.
        /// </summary>
        public int[] Birthday { get; set; }

        /// <summary>
        /// The user's gender.
        /// </summary>
        [JsonProperty]
        public Gender Gender { get; set; }

        /// <summary>
        /// The id of the household the user is requesting to join.
        /// </summary>
        [JsonProperty]
        public Guid RequestedHouseholdId { get; set; }

        /// <summary>
        /// The password hash of the household the user is attempting to join.
        /// </summary>
        [JsonProperty]
        public string RequestedHousholdPasswordHash { get; set; }
    }
}
