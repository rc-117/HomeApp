namespace DummyRepo.Entities
{
    using System;

    /// <summary>
    /// The base income class.
    /// </summary>
    public class Income
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
        /// The income category
        /// </summary>
        public IncomeCategory IncomeCategory { get; set; }

        /// <summary>
        /// The date and time the income record was created.
        /// </summary>
        public DateTime DateTime { get; set; }


        /// <summary>
        /// The recurring type.
        /// </summary>
        public RecurringType RecurringType { get; set; }

        /// <summary>
        /// The account for this transaction.
        /// </summary>
        public Account Account { get; set; }

        /// <summary>
        /// Indicates whether or not the income has been deposited. 
        /// </summary>
        public bool IsDeposited { get; set; }

    }
}
