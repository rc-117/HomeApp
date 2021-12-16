namespace Homeapp.Backend.Identity
{
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// The user class.
    /// </summary>
    public class User
    {
        /// <summary>
        /// The unique Id of the user.
        /// </summary>
        [JsonProperty]
        public Guid Id { get; set; }

        /// <summary>
        /// The user's email address.
        /// </summary>
        [JsonProperty]
        public string EmailAddress { get; set; }

        /// <summary>
        /// Hash of the user's password.
        /// </summary>
        public string PasswordHash { get; set; }

        /// <summary>
        /// The user's first name. 
        /// </summary>
        [JsonProperty]
        public string FirstName { get; set; }

        /// <summary>
        /// The user's last name. 
        /// </summary>
        [JsonProperty]
        public string LastName { get; set; }

        /// <summary>
        /// The user's gender.
        /// </summary>
        public Gender Gender { get; set; }

        /// <summary>
        /// The unique id of the household the user belongs to.
        /// </summary>
        public Guid HouseholdId { get; set; }

        /// <summary>
        /// The list of households that the user is a member of.
        /// </summary>
        /// <remarks>
        /// The user must be a member of at least one household.
        /// </remarks>
        [JsonProperty]
        public List<Household> Households { get; set; }

        /// <summary>
        /// The list of household groups that the user is a member of.
        /// </summary>
        /// The user is not required to be a member of any household group.
        [JsonProperty]
        public List<HouseholdGroup> HouseholdGroups { get; set; }
    }
}
