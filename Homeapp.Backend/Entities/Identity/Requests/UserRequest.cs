namespace Homeapp.Backend.Identity.Requests
{
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Class containing properties to create or modify an existing User in the database.
    /// </summary>
    public class UserRequest
    {
        /// <summary>
        /// (Optional) The id of the user to modify.
        /// </summary>
        [JsonProperty]
        public string Id { get; set; }

        /// <summary>
        /// The user's email address.
        /// </summary>
        [JsonProperty]
        [EmailAddress]
        [Required(ErrorMessage = "Email address is required.")]
        public string EmailAddress { get; set; }

        /// <summary>
        /// (Optional) The user's phone number.
        /// </summary>
        [JsonProperty]
        public string PhoneNumber { get; set; }

        /// <summary>
        /// Hash of the user's password.
        /// </summary>
        [JsonProperty]
        [Required(ErrorMessage = "Password is required.")]
        public string PasswordHash { get; set; }

        /// <summary>
        /// The user's first name. 
        /// </summary>
        [JsonProperty]
        [Required(ErrorMessage = "First name is required.")]
        public string FirstName { get; set; }

        /// <summary>
        /// The user's last name. 
        /// </summary>
        [JsonProperty]
        [Required(ErrorMessage = "Last name is required.")]
        public string LastName { get; set; }

        /// <summary>
        /// The user's birthday in m/d/yyyy format.
        /// </summary>
        [Required(ErrorMessage = "Birthday is required.")]
        public string Birthday { get; set; }

        /// <summary>
        /// The user's gender.
        /// </summary>
        [JsonProperty]
        [Required(ErrorMessage = "Gender is required.")]
        public string Gender { get; set; }

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
