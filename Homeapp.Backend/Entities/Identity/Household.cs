namespace Homeapp.Backend.Identity
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Class containing all users in a household.
    /// </summary>
    /// <remarks>
    /// Users are required to register their user into at least one household.
    /// </remarks>
    public class Household
    {
        /// <summary>
        /// The unique id of the household.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// The name of the household.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Groupings of users within the household.
        /// </summary>
        public List<HouseholdGroup> HouseholdGroups { get; set; }

        /// <summary>
        /// List of users in the household.
        /// </summary>
        public List<UserHousehold> Users { get; set; }

        /// <summary>
        /// Hash of the household password.
        /// </summary>
        /// <remarks>
        /// Used to register a new user into an existing household.
        /// </remarks>
        public string PasswordHash { get; set; }
    }
}
