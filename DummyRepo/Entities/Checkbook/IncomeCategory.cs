namespace DummyRepo.Entities
{
    using System;

    /// <summary>
    /// The income category.
    /// </summary>
    public class IncomeCategory
    {
        /// <summary>
        /// The unique Id of the income category.
        /// </summary>    
        public Guid Id { get; set; }

        /// <summary>
        /// The name of the category.
        /// </summary>
        public string Name { get; set; }
    }
}