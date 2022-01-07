namespace Homeapp.Backend.Entities
{
    using Newtonsoft.Json;
    using System;
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// An object containing properties to create an Income/Expense category or modify an existing one.
    /// </summary>
    public class CategoryRequest
    {
        /// <summary>
        /// (Optional, provide if modifying an existing category) The unique id of the category.
        /// </summary>
        [JsonProperty]
        public string CategoryId { get; set; }

        /// <summary>
        /// The name of the category.
        /// </summary>
        [Required]
        [JsonProperty]
        public string Name { get; set; }

        /// <summary>
        /// (Optional) A description of the category.
        /// </summary>
        [JsonProperty]
        public string Description { get; set; }

        /// <summary>
        /// The id of the user who created this category.
        /// </summary>
        [Required]
        [JsonProperty]
        public string CreatingUserId { get; set; }

        /// <summary>
        /// (Optional, provide if modifying an existing category) The unique id of the SharedEntitiesRecord.
        /// </summary>
        [JsonProperty]
        public string SharedEntitiesId { get; set; }

        /// <summary>
        /// Object containing a list of entities that have access to this resource.
        /// </summary>
        [Required]
        [JsonProperty]
        public SharedEntitiesRequest SharedEntities { get; set; }
    }
}
