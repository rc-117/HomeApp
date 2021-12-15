namespace Homeapp.Backend.Identity
{
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
        /// List of users in the household group.
        /// </summary>
        public List<User> Users { get; set; }
    }
}