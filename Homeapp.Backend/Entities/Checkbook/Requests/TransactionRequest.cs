namespace Homeapp.Backend.Entities
{
    using Homeapp.Backend.Identity;
    using Newtonsoft.Json;
    using System;
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// An object containing properties to create or modify an existing account transaction.
    /// </summary>
    public class TransactionRequest
    {
        /// <summary>
        /// (For edit requests) The id of the transaction to modify.
        /// </summary>
        [JsonProperty]
        public string Id { get; set; }

        /// <summary>
        /// The transaction name.
        /// </summary>
        [JsonProperty]
        [Required(ErrorMessage = "Transaction name is required.")]
        public string Name { get; set; }

        /// <summary>
        /// The transaction amount.
        /// </summary>
        [JsonProperty]
        [Required(ErrorMessage = "Amount is required.")]
        public double Amount { get; set; }

        /// <summary>
        /// The transaction type.
        /// </summary>
        [JsonProperty]
        [Required(ErrorMessage = "Transaction type is required.")]
        public string TransactionType { get; set; }

        /// <summary>
        /// The id of the user who owns this transaction.
        /// </summary>
        [JsonProperty]
        [Required(ErrorMessage = "Owner id is required.")]
        public string OwnerId { get; set; }

        /// <summary>
        /// The expense category, if applicable.
        /// </summary>
        [JsonProperty]
        public string ExpenseCategory { get; set; }

        /// <summary>
        /// The income category, if applicable.
        /// </summary>
        [JsonProperty]
        public string IncomeCategory { get; set; }

        /// <summary>
        /// The date and time the transaction was created
        /// </summary>
        [JsonProperty]
        public string DateTime { get; set; }

        /// <summary>
        /// The account id.
        /// </summary>
        [JsonProperty]
        [Required]
        public string AccountId { get; set; }

        /// <summary>
        /// The account id to transfer funds to, if this is a transfer request.
        /// </summary>
        [JsonProperty]
        public string AccountIdToTransferTo { get; set; }

        /// <summary>
        /// Indicates whether or not the transaction has been cleared. 
        /// </summary>
        [JsonProperty]
        [Required]
        public bool IsCleared { get; set; }

        /// <summary>
        /// The recurring transaction id that this transaction is generated from, if applicable.
        /// </summary>
        [JsonProperty]
        public string RecurringTransactionId { get; set; }
    }
}