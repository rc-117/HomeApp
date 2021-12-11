namespace DummyRepo.Entities
{
    using System;

    /// <summary>
    /// Bank account class.
    /// </summary>
    public class Account
    {
        /// <summary>
        /// The unique Id of the account.
        /// </summary>
        public Guid Id { get; set; }

        //public User User { get; set; }
        //public string UserId { get; set; }

        /// <summary>
        /// The name of the account.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The account's starting balance.
        /// </summary>
        public double StartingBalance { get; set; }
    }
}