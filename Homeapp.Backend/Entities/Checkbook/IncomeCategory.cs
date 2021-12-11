namespace Homeapp.Backend.Entities
{
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
    }
}