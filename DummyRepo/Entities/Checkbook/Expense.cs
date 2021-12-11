namespace DummyRepo.Entities
{
    using System;

    /// <summary>
    /// The base expense class.
    /// </summary>
    public class Expense
    {
        /// <summary>
        /// The unique Id of the Expense
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// The Id of the user who owns this expense
        /// </summary>
        public Guid UserId { get; set; }

        /// <summary>
        /// The expense category
        /// </summary>
        public ExpenseCategory ExpenseCategory { get; set; }

        /// <summary>
        /// The date and time the expense was created
        /// </summary>
        public DateTime DateTime { get; set; }


        /// <summary>
        /// The recurring type
        /// </summary>
        public RecurringType RecurringType { get; set; }

        /// <summary>
        /// The account for this transaction.
        /// </summary>
        public Account Account { get; set; }

        /// <summary>
        /// Indicates whether or not the expense has been paid for. 
        /// </summary>
        public bool IsPaid { get; set; }
    }
}
