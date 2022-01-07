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
        public Guid Id { get; set; }

        /// <summary>
        /// The recurring transaction name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The recurring transaction amount.
        /// </summary>
        public double Amount { get; set; }

        /// <summary>
        /// The recurring transaction type.
        /// </summary>
        public TransactionType TransactionType { get; set; }

        /// <summary>
        /// The id of the user who created this recurring transaction.
        /// </summary>
        public Guid CreatingUserId { get; set; }

        /// <summary>
        /// The user who who created this recurring transaction.
        /// </summary>
        public User CreatingUser { get; set; }

        /// <summary>
        /// The expense category, if applicable.
        /// </summary>
        public ExpenseCategory ExpenseCategory { get; set; }

        /// <summary>
        /// The income category, if applicable.
        /// </summary>
        public IncomeCategory IncomeCategory { get; set; }

        /// <summary>
        /// The date and time the recurring transaction was created.
        /// </summary>
        public DateTime DateTimeCreated { get; set; }

        /// <summary>
        /// The date the recurring transaction starts.
        /// </summary>
        public DateTime DateStart { get; set; }

        /// <summary>
        /// The unique id of the recurring schedule for this item, if applicable.
        /// </summary>
        public Guid RecurringScheduleId { get; set; }

        /// <summary>
        /// The schedule that the recurring item will follow, if applicable.
        /// </summary>
        public RecurringSchedule RecurringSchedule { get; set; }

        /// <summary>
        /// The account id.
        /// </summary>
        public Guid AccountId { get; set; }

        /// <summary>
        /// The account for this transaction.
        /// </summary>
        public Account Account { get; set; }

        /// <summary>
        /// The unique id of the SharedIdentities object.
        /// </summary>
        public Guid SharedEntitiesId { get; set; }

        /// <summary>
        /// Object containing a list of entities that have access to this resource.
        /// </summary>
        public AllowedUsers SharedEntities { get; set; }
    }
}
