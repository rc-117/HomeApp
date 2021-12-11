namespace DummyRepo.Entities
{
    using System;

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
        /// Hash of the user's password.
        /// </summary>
        private string PasswordHash { get; set; }

        /// <summary>
        /// The user's first name. 
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// The user's last name. 
        /// </summary>
        public string LastName { get; set; }
    }
}
