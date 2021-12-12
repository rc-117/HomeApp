namespace Homeapp.Backend.Tools
{
    using Homeapp.Backend.Entities;
    using System;

    /// <summary>
    /// The validation class for the application.
    /// </summary>
    public static class Validation
    {
        /// <summary>
        /// Checks if a guid is valid
        /// </summary>
        /// <param name="guid">The string guid to check.</param>
        public static bool GuidIsValid(string guid, out Guid outGuid)
        {
            try
            {
                outGuid = Guid.Parse(guid);
                return true;
            }
            catch (Exception)
            {
                outGuid = Guid.Empty;
                return false;
            }
        }

        /// <summary>
        /// Checks if an account belongs to the logged in user.
        /// </summary>
        /// <param name="userId">The logged in user's id.</param>
        /// <param name="account">The account to check.</param>
        /// <returns></returns>
        public static bool AccountBelongsToUser(Guid userId, Account account)
        {
            return userId == account.UserId;
        }

        public static bool StringIsBase64Compatible(string value, out byte[] bytes)
        {
            try
            {
                bytes = Convert.FromBase64String(value);
                return true;
            }
            catch (Exception)
            {
                bytes = null;
                return false;
            }
        }
    }
}
