namespace Homeapp.Backend.Managers
{
    using Homeapp.Backend.Identity;
    using Newtonsoft.Json.Linq;

    /// <summary>
    /// The user data manager.
    /// </summary>
    public class UserDataManager : IUserDataManager
    {
        /// <summary>
        /// Initializes the user data manager class.
        /// </summary>
        public UserDataManager()
        {

        }

        /// <summary>
        /// Creates a JObject from a user, exluding the user's password hash.
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public JObject CreateShortUserJobjectFromUser(User user)
        {
            return new JObject
            {
                { "UserId", user.Id },
                { "UserEmail", user.EmailAddress },
                { "UserFirstName", user.FirstName },
                { "UserLastName", user.LastName }
            };
        }
    }
}
