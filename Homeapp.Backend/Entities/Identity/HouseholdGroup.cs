namespace Homeapp.Backend.Identity
{
    using Homeapp.Backend.Entities;
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// A grouping of users within a household.
    /// </summary>
    public class HouseholdGroup
    {
        /// <summary>
        /// The unique id of the household group.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// The name of the household group.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The unique id of the household containing this group.
        /// </summary>
        public Guid HouseholdId { get; set; }

        /// <summary>
        /// The household containing the group.
        /// </summary>
        public Household Household { get; set; }

        /// <summary>
        /// List of users in the household group.
        /// </summary>
        public List<UserHouseholdGroup> Users { get; set; }

        /// <summary>
        /// The id of the user who created the household.
        /// </summary>
        public Guid CreatorId { get; set; }

        /// <summary>
        /// The user who created the household.
        /// </summary>
        public User Creator { get; set; }

        /// <summary>
        /// The date and time that the household was created.
        /// </summary>
        public DateTime DateTimeCreated { get; set; }

        /// <summary>
        /// A list of users who have read, write, and full access over this household.
        /// </summary>
        public AllowedUsers AllowedUsers { get; set; }
    }
}