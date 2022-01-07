namespace Homeapp.Backend.Entities
{
    using Homeapp.Backend.Identity;
    using Newtonsoft.Json;
    using System;

    /// <summary>
    /// The income category.
    /// </summary>
    public class IncomeCategory
    {
        /// <summary>
        /// The unique Id of the income category.
        /// </summary>
        [JsonProperty]
        public Guid Id { get; set; }

        /// <summary>
        /// The name of the category.
        /// </summary>
        [JsonProperty]
        public string Name { get; set; }

        /// <summary>
        /// (Optional) A description of the category.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// The id of the user who created this category.
        /// </summary>
        [JsonProperty]
        public Guid CreatingUserId { get; set; }

        /// <summary>
        /// The user who created this category.
        /// </summary>
        [JsonProperty]
        public User CreatingUser { get; set; }

        /// <summary>
        /// The unique id of the SharedIdentities object.
        /// </summary>
        [JsonProperty]
        public Guid SharedEntitiesId { get; set; }

        /// <summary>
        /// Object containing a list of entities that have access to this resource.
        /// </summary>
        [JsonProperty]
        public SharedEntities SharedEntities { get; set; }
    }
}