namespace Homeapp.Backend.Identity.Requests
{
    using Homeapp.Backend.Entities;
    using Homeapp.Backend.Entities.Requests;
    using Newtonsoft.Json;
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Class containing properties to create or modify an existing household object.
    /// </summary>
    public class HouseholdRequest
    {
        /// <summary>
        /// (Optional) The id of the household to modify.
        /// </summary>
        [JsonProperty]
        public string Id { get; set; }

        /// <summary>
        /// The name of the household.
        /// </summary>
        [JsonProperty]
        [Required(ErrorMessage = "Name of household is required.")]
        public string Name { get; set; }

        /// <summary>
        /// Hash of the household password.
        /// </summary>
        /// <remarks>
        /// Used to register a new user into an existing household.
        /// </remarks>
        [JsonProperty]
        [Required(ErrorMessage = "Password hash is required.")]
        public string PasswordHash { get; set; }

        /// <summary>
        /// Request object containing properties to create or modify and existing address record.
        /// </summary>
        [JsonProperty]
        public AddressRequest Address { get; set; }

        /// <summary>
        /// (Optional) The phone number for the household.
        /// </summary>
        [JsonProperty]
        public string PhoneNumber { get; set; }

        /// <summary>
        /// List of users who will have read/write/full permissions over this household.
        /// </summary>
        [JsonProperty]
        [Required(ErrorMessage = "'AllowedUsers' is required. If there are no entities who will have access to the household, create a blank 'AllowedUsers' request.")]
        public AllowedUsersRequest AllowedUsers { get; set; }

        /// <summary>
        /// The household group request properties.
        /// </summary>
        [JsonProperty]
        [Required(ErrorMessage = "Household group requests required. An empty array is acceptable if no group creations are requested.")]
        public HouseholdGroupRequest[] HouseholdGroupRequests { get; set; }
    }
}
