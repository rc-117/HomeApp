namespace Homeapp.Backend.Identity
{
    using Newtonsoft.Json;
    using System;

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
        private string PasswordHash { get; set; }

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
    }
}
