namespace Homeapp.Backend.Managers
{
    using Homeapp.Backend.Identity;
    using Newtonsoft.Json.Linq;

    /// <summary>
    /// The user data manager.
    /// </summary>
    public interface IUserDataManager
    {
        /// <summary>
        /// Creates a JObject from a user, exluding the user's password hash.
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public JObject CreateShortUserJobjectFromUser(User user);
    }
}