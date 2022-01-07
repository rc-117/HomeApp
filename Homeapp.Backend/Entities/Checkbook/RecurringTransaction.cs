namespace Homeapp.Backend.Entities
{
    using Homeapp.Backend.Identity;
    using Newtonsoft.Json;
    using System;

    /// <summary>
    /// The recurring transaction class.
    /// </summary>
    public class RecurringTransaction
    {
        /// <summary>
        /// The unique id of the recurring transaction.
        /// </summary>
        [JsonProperty]
        public Guid Id { get; set; }

        /// <summary>
        /// The recurring transaction name.
        /// </summary>
        [JsonProperty]
        public string Name { get; set; }

        /// <summary>
        /// The recurring transaction amount.
        /// </summary>
        public double Amount { get; set; }

        /// <summary>
        /// The recurring transaction type.
        /// </summary>
        [JsonProperty]
        public TransactionType TransactionType { get; set; }

        /// <summary>
        /// The id of the user who created this recurring transaction.
        /// </summary>
        [JsonProperty]
        public Guid CreatingUserId { get; set; }

        /// <summary>
        /// The user who who created this recurring transaction.
        /// </summary>
        [JsonProperty]
        public User CreatingUser { get; set; }

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
        /// The date and time the recurring transaction was created.
        /// </summary>
        [JsonProperty]
        public DateTime DateTimeCreated { get; set; }

        /// <summary>
        /// The date the recurring transaction starts.
        /// </summary>
        [JsonProperty]
        public DateTime DateStart { get; set; }

        /// <summary>
        /// The time of day that the recurring item will occur.
        /// </summary>
        [JsonProperty]
        public RecurringTime RecurringTime { get; set; }

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
