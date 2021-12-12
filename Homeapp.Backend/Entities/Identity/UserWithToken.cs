namespace Homeapp.Backend.Identity
{
    /// <summary>
    /// Class containing a user and a valid JWT token.
    /// </summary>
    public class UserWithToken
    {
        /// <summary>
        /// The JWT token.
        /// </summary>
        public string Token { get; set; }

        /// <summary>
        /// The user.
        /// </summary>
        public User User { get; set; }

        /// <summary>
        /// Initializes a UserWithToken
        /// </summary>
        public UserWithToken(User user)
        {
            this.User = user;  
        }
    }
}
