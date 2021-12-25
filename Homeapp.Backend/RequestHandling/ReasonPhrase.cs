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
        /// A specified household could not be found.
        /// </summary>
        HouseholdNotFound,

        /// <summary>
        /// A supplied guid is invalid.
        /// </summary>
        InvalidGuid,

        /// <summary>
        /// User could not be found.
        /// </summary>
        UserNotFound,

        /// <summary>
        /// Household group could not be found in a household.
        /// </summary>
        HouseholdGroupNotFound
    }
}
