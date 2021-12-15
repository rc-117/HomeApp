namespace Homeapp.Backend.Identity.Requests
{
    using System.Collections.Generic;

    /// <summary>
    /// Class containing properties to create a new Household object.
    /// </summary>
    public class CreateHouseholdRequest
    {
        /// <summary>
        /// The name of the household.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Groupings of users within the household.
        /// </summary>
        public List<HouseholdGroup> HouseholdGroups { get; set; }

        /// <summary>
        /// Hash of the household password.
        /// </summary>
        /// <remarks>
        /// Used to register a new user into an existing household.
        /// </remarks>
        public string PasswordHash { get; set; }
    }
}
