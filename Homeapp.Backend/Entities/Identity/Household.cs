namespace Homeapp.Backend.Identity
{
    using Homeapp.Backend.Entities;
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

        /// <summary>
        /// (Optional) The address of the house.
        /// </summary>
        public Address Address { get; set; }

        /// <summary>
        /// (Optional) The phone number of the house.
        /// </summary>
        public string PhoneNumber { get; set; }

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
