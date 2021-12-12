namespace Homeapp.Backend.Entities
{
    using Homeapp.Backend.Identity;
    using Newtonsoft.Json;
    using System;

    /// <summary>
    /// The account transaction class.
    /// </summary>
    public class Transaction
    {
        /// <summary>
        /// The unique id of the transaction.
        /// </summary>
        [JsonProperty]
        public Guid Id { get; set; }

        /// <summary>
        /// The transaction name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The id of the user who owns this transaction.
        /// </summary>
        [JsonProperty]
        public Guid UserId { get; set; }

        /// <summary>
        /// The user who owns this transaction.
        /// </summary>
        [JsonProperty]
        public User User { get; set; }

        /// <summary>
        /// The expense category, if applicable.
        /// </summary>
        [JsonProperty]
        public ExpenseCategory ExpenseCategory { get; set; }

        /// <summary>
        /// The income category, if applicable.
        /// </summary>
        [JsonProperty]
        public IncomeCategory IncomeCategory { get; set; }

        /// <summary>
        /// The date and time the transaction was created
        /// </summary>
        [JsonProperty]
        public DateTime DateTime { get; set; }


        /// <summary>
        /// The recurring type
        /// </summary>
        [JsonProperty]
        public RecurringType RecurringType { get; set; }

        /// <summary>
        /// The account id.
        /// </summary>
        [JsonProperty]
        public Guid AccountId { get; set; }

        /// <summary>
        /// The account for this transaction.
        /// </summary>
        [JsonProperty]
        public Account Account { get; set; }

        /// <summary>
        /// Indicates whether or not the transaction has been cleared. 
        /// </summary>
        [JsonProperty]
        public bool IsCleared { get; set; }
    }
}
