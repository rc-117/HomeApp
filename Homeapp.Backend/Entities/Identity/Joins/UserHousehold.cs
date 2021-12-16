namespace Homeapp.Backend.Identity
{
    using System;

    /// <summary>
    /// The User/Household join.
    /// </summary>
    public class UserHousehold
    {
        /// <summary>
        /// The unique id of the user.
        /// </summary>
        public Guid UserId { get; set; }

        /// <summary>
        /// The user.
        /// </summary>
        public User User { get; set; }

        /// <summary>
        /// The unique id of the household.
        /// </summary>
        public Guid HouseholdId { get; set; }

        /// <summary>
        /// The household.
        /// </summary>
        public Household Household { get; set; }
    }
}