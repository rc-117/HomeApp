namespace Homeapp.Backend.Tools
{
    using Homeapp.Backend.Db;
    using Homeapp.Backend.Entities;
    using Homeapp.Backend.Identity;
    using System;
    using System.Linq;

    /// <summary>
    /// The validation class for the application.
    /// </summary>
    public static class Validation
    {
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

        public static bool EmailIsAlreadyInUse(string email, AppDbContext appDbContext)
        {
            User userWithExistingEmail = appDbContext.Users.FirstOrDefault(u => u.EmailAddress == email);

            return userWithExistingEmail != null;
        }

        public static bool HouseholdExists(Guid householdId, AppDbContext appDbContext)
        {
            var existingHousehold = appDbContext.Households.FirstOrDefault(h => h.Id == householdId);

            return existingHousehold != null;
        }

        public static bool HouseholdGroupExists(Guid groupId, AppDbContext appDbContext)
        {
            var existingHouseholdGroup = appDbContext.HouseholdGroups.FirstOrDefault(u => u.Id == groupId);

            return existingHouseholdGroup != null;
        }

        public static bool GroupIsInHousehold(Guid groupId, Guid householdId, AppDbContext appDbContext)
        {
            var existingHouseholdGroup = 
                appDbContext
                .HouseholdGroups
                .Where(h => h.Id == groupId)
                .FirstOrDefault(h => h.HouseholdId == householdId);

            return existingHouseholdGroup != null;
        }

        public static bool UserIsInHousehold(Guid userId, Guid householdId, AppDbContext appDbContext)
        {
            var existingUser =
                appDbContext
                .UserHouseholds
                .Where(h => h.HouseholdId == householdId)
                .FirstOrDefault(h => h.UserId == userId);

            return existingUser != null;
        }

        public static bool UserExists(Guid userId, AppDbContext appDbContext)
        {
            var existingUserId = appDbContext.Users.FirstOrDefault(u => u.Id == userId);

            return existingUserId != null;
        }

        public static bool RequestedHouseholdPasswordIsValid
            (Guid householdId, 
            string passwordHash,
            AppDbContext appDbContext)
        {
            return appDbContext
                .Households
                .FirstOrDefault(h => h.Id == householdId)
                .PasswordHash == passwordHash;
        }

        /// <summary>
        /// Checks if a given email password combination is valid.
        /// </summary>
        /// <param name="email">The given email address.</param>
        /// <param name="passwordHash">The given password hash.</param>
        /// <param name="appDbContext">The app database context.</param>
        /// <returns></returns>
        public static bool UserEmailPasswordComboIsValid
            (string email,
            string passwordHash,
            AppDbContext appDbContext)
        {
            return appDbContext
                .Users
                .FirstOrDefault(u => u.EmailAddress == email)
                .PasswordHash == passwordHash;
        }

        /// <summary>
        /// Checks if an int array can be converted to a DateTimeObject.
        /// </summary>
        /// <param name="dateArray">The int array.</param>
        public static bool IntArrayIsDateTimeConvertible(int[] dateArray)
        {
            if (DateIntArrayIsValid(dateArray))
            {
                try
                {
                    var dateTime = new DateTime(dateArray[2], dateArray[0], dateArray[1]);
                    
                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
            }
            return false;
        }

        /// <summary>
        /// Checks if a given birthday is valid.
        /// </summary>
        /// <param name="birthday">The birthday DateTime object.</param>
        public static bool BirthdayIsValid(DateTime birthday)
        {
            return birthday.Date < DateTime.Now.Date;            
        }

        /// <summary>
        /// Checks if an int array has valid m/d/yyy values.
        /// </summary>
        /// <param name="dateArray">The int array.</param>
        public static bool DateIntArrayIsValid(int[] dateArray)
        {
            return dateArray.Count() != 3 ? false :
                (dateArray[0] < 0 || dateArray[0] > 12 ? false : (
                dateArray[1] < 0 || dateArray[1] > 31 ? false :
                dateArray[2] < 0 ? false : true));
        }


    }
}
