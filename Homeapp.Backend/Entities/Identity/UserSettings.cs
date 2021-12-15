namespace Homeapp.Backend.Identity
{
    using System;

    /// <summary>
    /// Class containing user app settings.
    /// </summary>
    public class UserSettings
    {
        /// <summary>
        /// The id of user who owns the settings.
        /// </summary>
        public Guid UserId { get; set; }

        /// <summary>
        /// The user who owns the settings.
        /// </summary>
        public User User { get; set; }
    }
}
