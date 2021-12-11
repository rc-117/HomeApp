namespace Homeapp.Backend.Entities
{
    /// <summary>
    /// The recurring type.
    /// </summary>
    public enum RecurringType
    {
        /// <summary>
        /// Will occur once.
        /// </summary>
        Once,

        /// <summary>
        /// Will occur once a day.
        /// </summary>
        Daily,

        /// <summary>
        /// Will occur once a week.
        /// </summary>
        Weekly,

        /// <summary>
        /// Will occur every two weeks.
        /// </summary>
        BiWeekly,

        /// <summary>
        /// Will occur once a month.
        /// </summary>
        Monthly,

        /// <summary>
        /// Will occur once a year
        /// </summary>
        Annually
    }
}