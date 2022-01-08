namespace Homeapp.Backend.Entities
{
    using Newtonsoft.Json;
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// An object containing properties to create or modify an existing income category.
    /// </summary>
    public class IncomeCategoryRequest
    {
        /// <summary>
        /// (For edit requests) The unique Id of the income category.
        /// </summary>
        [JsonProperty]
        public string Id { get; set; }

        /// <summary>
        /// The name of the category.
        /// </summary>
        [JsonProperty]
        [Required]
        public string Name { get; set; }

        /// <summary>
        /// (Optional) A description of the category.
        /// </summary>
        [JsonProperty]
        public string Description { get; set; }

        /// <summary>
        /// An object containing a list of users allowed to access this resource.
        /// </summary>
        [JsonProperty]
        [Required]
        public AllowedUsersRequest AllowedUsersRequest { get; set; }
    }
}
