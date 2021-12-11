namespace Homeapp.Backend.Entities
{
    using Newtonsoft.Json;
    using System;

    public class ExpenseCategory
    {
        /// <summary>
        /// The unique Id of the expense category.
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