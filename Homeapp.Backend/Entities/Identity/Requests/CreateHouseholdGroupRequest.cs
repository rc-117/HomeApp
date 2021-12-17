namespace Homeapp.Backend.Identity.Requests
{
    using System.Collections.Generic;

    /// <summary>
    /// Class containing properties to create a new HouseholdGroup object.
    /// </summary>
    public class CreateHouseholdGroupRequest
    {
        /// <summary>
        /// The requested name of the household group.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Indicates whether or not to add the requesting user to this group.
        /// </summary>
        public bool AddRequestingUserToGroup { get; set; }

        /// <summary>
        /// List of users to add to the household group.
        /// </summary>
        public List<User> Users { get; set; }
    }
}
