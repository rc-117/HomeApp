namespace Homeapp.Backend.Identity.Requests
{
    using Homeapp.Backend.Entities;
    using Newtonsoft.Json;
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Class containing properties to create a or modify an existing householdGroup object.
    /// </summary>
    public class HouseholdGroupRequest
    {
        /// <summary>
        /// (Optional) The unique id of the household group to modify.
        /// </summary>
        [JsonProperty]
        public string Id { get; set; }

        /// <summary>
        /// (Optional) The id of the household to add this group to.
        /// </summary>
        /// <remarks>Use this property when adding a group to an existing household.</remarks>
        [JsonProperty]
        public string HouseholdId { get; set; }

        /// <summary>
        /// The requested name of the household group.
        /// </summary>
        [JsonProperty]
        [Required(ErrorMessage = "'Name' is required.")]
        public string Name { get; set; }

        /// <summary>
        /// Indicates whether or not to add the requesting user to this group.
        /// </summary>
        [JsonProperty]
        [Required(ErrorMessage = "'AddRequestingUserToGroup' is required.")]
        public bool AddRequestingUserToGroup { get; set; }

        /// <summary>
        /// Array of user ids to add to the household group.
        /// </summary>
        [JsonProperty]
        [Required(ErrorMessage = "'UserIds' is required. If there are no users to add, create a blank 'UserIds' field.", 
            AllowEmptyStrings = true)]
        public string[] UserIds { get; set; }

        /// <summary>
        /// List of users who will have read/write/full access to this household group.
        /// </summary>
        [JsonProperty]
        [Required(ErrorMessage = "'AllowedUsers' is required. If there are no users to add, create a blank 'AllowedUsers' field.")]
        public AllowedUsersRequest AllowedUsers { get; set; }
    }
}
