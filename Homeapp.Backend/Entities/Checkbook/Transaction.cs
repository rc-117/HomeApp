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
        public Guid Id { get; set; }

        /// <summary>
        /// The transaction name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The transaction amount.
        /// </summary>
        public double Amount { get; set; }

        /// <summary>
        /// The transaction type.
        /// </summary>
        public TransactionType TransactionType { get; set; }

        /// <summary>
        /// (For 'Transfer' transaction type) The account id to transfer funds to. The amount must be more than zero for a transfer.
        /// </summary>
        public Guid AccountIdToTransferTo { get; set; }

        /// <summary>
        /// (For 'Transfer' transaction type) The account to transfer funds to. The amount must be more than zero for a transfer.
        /// </summary>
        public Account AccountToTransferTo { get; set; }

        /// <summary>
        /// The id of the user who owns this transaction.
        /// </summary>
        public Guid OwnerId { get; set; }

        /// <summary>
        /// The user who owns this transaction.
        /// </summary>
        public User Owner { get; set; }

        /// <summary>
        /// The expense category, if applicable.
        /// </summary>
        public ExpenseCategory ExpenseCategory { get; set; }

        /// <summary>
        /// The income category, if applicable.
        /// </summary>
        public IncomeCategory IncomeCategory { get; set; }

        /// <summary>
        /// The date and time the transaction was created
        /// </summary>
        public DateTime DateTime { get; set; }

        /// <summary>
        /// The recurring transaction id that this transaction is generated from, if applicable.
        /// </summary>
        public Guid RecurringTransactionId { get; set; }

        /// <summary>
        /// The recurring transaction that this transaction is generated from, if applicable.
        /// </summary>
        public RecurringTransaction RecurringTransaction { get; set; }

        /// <summary>
        /// The account id.
        /// </summary>
        public Guid AccountId { get; set; }

        /// <summary>
        /// The account for this transaction.
        /// </summary>
        public Account Account { get; set; }

        /// <summary>
        /// Indicates whether or not the transaction has been cleared. 
        /// </summary>
        public bool IsCleared { get; set; }

        /// <summary>
        /// The unique id of the SharedIdentities object.
        /// </summary>
        public Guid SharedEntitiesId { get; set; }

        /// <summary>
        /// Object containing a list of entities that have access to this resource.
        /// </summary>
        public SharedEntities SharedEntities { get; set; }
    }
}
