namespace Homeapp.Backend.Identity
{
    using System;

    /// <summary>
    /// The User/HouseholdGroup join.
    /// </summary>
    public class UserHouseholdGroup
    {
        /// <summary>
        /// Unique id of the join.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// The unique id of the user.
        /// </summary>
        public Guid UserId { get; set; }

        /// <summary>
        /// The user.
        /// </summary>
        public User User { get; set; }

        /// <summary>
        /// The unique id of the household group.
        /// </summary>
        public Guid HouseholdGroupId { get; set; }

        /// <summary>
        /// The household group.
        /// </summary>
        public HouseholdGroup HouseholdGroup { get; set; }
    }
}