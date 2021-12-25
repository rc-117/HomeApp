namespace Homeapp.Backend.Tools
{
    /// <summary>
    /// Enum containing common Reason Phrases for use in HttpError responses.
    /// </summary>
    public enum ReasonPhrase
    {
        /// <summary>
        /// User is unauthorized to perform an action.
        /// </summary>
        UserUnauthorized,

        /// <summary>
        /// The specified household could not be found.
        /// </summary>
        HouseholdNotFound,

        /// <summary>
        /// The supplied guid is invalid.
        /// </summary>
        InvalidGuid,

        /// <summary>
        /// User could not be found.
        /// </summary>
        UserNotFound,

        /// <summary>
        /// Household group could not be found in a household.
        /// </summary>
        HouseholdGroupNotFound,

        /// <summary>
        /// The request is invalid and could not be processed.
        /// </summary>
        InvalidRequest,

        /// <summary>
        /// The provided credentials are invalid.
        /// </summary>
        InvalidUserCredentials,

        /// <summary>
        /// The requested email is already in use.
        /// </summary>
        EmailAlreadyInUse,

        /// <summary>
        /// The provided household password is invalid.
        /// </summary>
        InvalidHouseholdPassword,

        /// <summary>
        /// The provided date array is invalid.
        /// </summary>
        InvalidDateArray,

        /// <summary>
        /// The provided birthday in invalid.
        /// </summary>
        InvalidBirthday
    }
}
