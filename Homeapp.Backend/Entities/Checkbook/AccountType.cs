namespace Homeapp.Backend.Entities
{
    /// <summary>
    /// The type of bank account.
    /// </summary>
    public enum AccountType
    {
        /// <summary>
        /// Checkings account.
        /// </summary>
        Checkings,

        /// <summary>
        /// Savings account.
        /// </summary>
        Savings,

        /// <summary>
        /// Credit card from a bank.
        /// </summary>
        CreditCard,

        /// <summary>
        /// Bank loan.
        /// </summary>
        Loan
    }
}