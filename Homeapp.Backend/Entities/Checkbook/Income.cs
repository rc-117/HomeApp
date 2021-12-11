namespace Homeapp.Backend.Entities
{
    using Newtonsoft.Json;
    using System;

    /// <summary>
    /// The base income class.
    /// </summary>
    public class Income
    {
        /// <summary>
        /// The unique Id of the Expense
        /// </summary>
        [JsonProperty]
        public string Id { get; set; }

        /// <summary>
        /// The Id of the user who owns this expense
        /// </summary>
        [JsonProperty]
        public Guid UserId { get; set; }

        /// <summary>
        /// The income category
        /// </summary>
        [JsonProperty]
        public IncomeCategory IncomeCategory { get; set; }

        /// <summary>
        /// The date and time the income record was created.
        /// </summary>
        [JsonProperty]
        public DateTime DateTime { get; set; }


        /// <summary>
        /// The recurring type.
        /// </summary>
        [JsonProperty]
        public RecurringType RecurringType { get; set; }

        /// <summary>
        /// The account for this transaction.
        /// </summary>
        [JsonProperty]
        public Account Account { get; set; }

        /// <summary>
        /// Indicates whether or not the income has been deposited. 
        /// </summary>
        [JsonProperty]
        public bool IsDeposited { get; set; }

    }
}
