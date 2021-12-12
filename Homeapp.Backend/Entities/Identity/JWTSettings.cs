namespace Homeapp.Backend.Identity
{
    /// <summary>
    /// JWT settings class containing the secret key.
    /// </summary>
    public class JWTSettings
    {
        /// <summary>
        /// The secret key.
        /// </summary>
        public string SecretKey { get; set; }
    }
}
