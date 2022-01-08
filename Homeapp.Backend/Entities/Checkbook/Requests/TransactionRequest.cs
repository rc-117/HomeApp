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
        /// Indicates whether or not this transaction is transferring funds to 
        /// an account external to this application.
        /// </summary>
        [JsonProperty]
        [Required(ErrorMessage = "'TransferToExternalAccount' bool is required.")]
        public bool TransferToExternalAccount { get; set; }

        /// <summary>
        /// Indicates whether or not this transaction is receiving funds from 
        /// an account external to this application.
        /// </summary>
        [JsonProperty]
        [Required(ErrorMessage = "'TransferFromExternalAccount' bool is required.")]
        public bool TransferFromExternalAccount { get; set; }

        /// <summary>
        /// The id of the user who owns this transaction.
        /// </summary>
        [JsonProperty]
        [Required(ErrorMessage = "Owner id is required.")]
        public string OwnerId { get; set; }

        /// <summary>
        /// The expense category id, if applicable.
        /// </summary>
        [JsonProperty]
        public string ExpenseCategoryId { get; set; }

        /// <summary>
        /// The income category id, if applicable.
        /// </summary>
        [JsonProperty]
        public string IncomeCategoryId { get; set; }

        /// <summary>
        /// The expense category request. Used 
        /// if creating a new expense category for this transaction.
        /// </summary>
        [JsonProperty]
        public ExpenseCategoryRequest ExpenseCategoryRequest { get; set; }

        /// <summary>
        /// The income category request. Used 
        /// if creating a new income category for this transaction.
        /// </summary>
        [JsonProperty]
        public IncomeCategoryRequest IncomeCategoryRequest { get; set; }

        /// <summary>
        /// The date and time the transaction was created.
        /// </summary>
        [JsonProperty]
        public string DateTime { get; set; }

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

        /// <summary>
        /// Indicates whether or not this request is for a recurring transaction.
        /// </summary>
        [JsonProperty]
        [Required]
        public bool IsRecurringTransaction { get; set; }
    }
}