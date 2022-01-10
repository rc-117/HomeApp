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
        public Guid Id { get; set; }

        /// <summary>
        /// The user's email address.
        /// </summary>
        public string EmailAddress { get; set; }

        /// <summary>
        /// (Optional) The user's phone number.
        /// </summary>
        /// <remarks>Stored in 10 digit format (i.e. "1234567890")</remarks>
        public string PhoneNumber { get; set; }

        /// <summary>
        /// Hash of the user's password.
        /// </summary>
        public string PasswordHash { get; set; }

        /// <summary>
        /// The user's first name. 
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// The user's last name. 
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// The user's gender.
        /// </summary>
        public Gender Gender { get; set; }

        /// <summary>
        /// The user's birthday.
        /// </summary>
        public DateTime Birthday { get; set; }

        /// <summary>
        /// The list of households that the user is a member of.
        /// </summary>
        /// <remarks>
        /// The user must be a member of at least one household.
        /// </remarks>
        public List<UserHousehold> Households { get; set; }

        /// <summary>
        /// The list of household groups that the user is a member of.
        /// </summary>
        /// The user is not required to be a member of any household group.
        public List<UserHouseholdGroup> HouseholdGroups { get; set; }
    }
}
