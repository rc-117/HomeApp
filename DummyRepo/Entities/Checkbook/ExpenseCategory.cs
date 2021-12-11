namespace DummyRepo.Entities
{
    using System;

    /// <summary>
    /// The expense category
    /// </summary>
    public class ExpenseCategory
    {
        /// <summary>
        /// The unique Id of the expense category.
        /// </summary>
        public Guid Id { get; set; }
        
        /// <summary>
        /// The name of the category.
        /// </summary>
        public string Name { get; set; }
    }
}