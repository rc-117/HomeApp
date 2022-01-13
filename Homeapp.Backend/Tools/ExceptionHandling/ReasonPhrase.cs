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
        /// The address request is invalid and could not be processed.
        /// </summary>
        InvalidAddress,

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
        /// The provided date is invalid.
        /// </summary>
        InvalidDate,

        /// <summary>
        /// The provided birthday in invalid.
        /// </summary>
        InvalidBirthday,

        /// <summary>
        /// The Checkbook account could not be found.
        /// </summary>
        CheckbookAccountNotFound,

        /// <summary>
        /// There was an error saving an entity/entities to the database.
        /// </summary>
        ErrorSavingToDatabase,

        /// <summary>
        /// There was an error when retrieving an entity/entities from the database.
        /// </summary>
        ErrorRetrievingFromDatabase,

        /// <summary>
        /// There was an error when editing a record in the database.
        /// </summary>
        ErrorModifyingDatabaseRecord,

        /// <summary>
        /// An invalid transaction type was received.
        /// </summary>
        InvalidTransactionType,

        /// <summary>
        /// An invalid transaction request was received.
        /// </summary>
        InvalidTransactionRequest,

        /// <summary>
        /// An invalid phone number was received.
        /// </summary>
        InvalidPhoneNumber,
    }
}
