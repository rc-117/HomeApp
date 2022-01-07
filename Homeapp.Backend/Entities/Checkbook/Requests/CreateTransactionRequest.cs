namespace Homeapp.Backend.Entities
{
    using Homeapp.Backend.Identity;
    using Newtonsoft.Json;
    using System;
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// An object containing properties to create an account transaction.
    /// </summary>
    public class CreateTransactionRequest
    {
        /// <summary>
        /// The transaction name.
        /// </summary>
        [JsonProperty]
        public string Name { get; set; }

        /// <summary>
        /// The transaction amount.
        /// </summary>
        [JsonProperty]
        [Required]
        public double Amount { get; set; }

        /// <summary>
        /// The transaction type.
        /// </summary>
        [JsonProperty]
        public string TransactionType { get; set; }

        /// <summary>
        /// The id of the user who owns this transaction.
        /// </summary>
        [JsonProperty]
        [Required]
        public Guid UserId { get; set; }

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
        /// The account id.
        /// </summary>
        [JsonProperty]
        public Guid AccountId { get; set; }

        /// <summary>
        /// Indicates whether or not the transaction has been cleared. 
        /// </summary>
        [JsonProperty]
        public bool IsCleared { get; set; }
    }
}