namespace Homeapp.Backend.Entities
{
    using System;
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// The schedule that the recurring item will follow.
    /// </summary>
    public class RecurringSchedule
    {
        /// <summary>
        /// The unique id of the schedule.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// The recurring type.
        /// </summary>
        public RecurringType RecurringType { get; set; }

        /// <summary>
        /// The hour of the day the recurring event will occur.
        /// </summary>
        /// <remarks>For recurring events with no time, set both hour and minutes to '0'.</remarks>
        [Range(minimum: 0, maximum: 24)]
        public int Hours { get; set; }

        /// <summary>
        /// The minutes in an hour the recurring event will occur.
        /// </summary>
        /// <remarks>For recurring events with no time, set both hour and minutes to '0'.</remarks>
        [Range(minimum: 0, maximum: 59)]
        public int Minutes { get; set; }

        /// <summary>
        /// (For weekly and Biweekly occurences) The day of the week.
        /// </summary>
        public DayOfWeek DayOfWeek { get; set; }

        /// <summary>
        /// The date that the recurring event will occur in the month.
        /// </summary>
        /// <remarks>For bi-monthly recurring events, use 'DateOfMonth' for the first date 
        /// and 'SecondDateOfMonth' for the second date.</remarks>
        [Range(minimum: 1, maximum: 31)]
        public int DateOfMonth { get; set; }

        /// <summary>
        /// The second date that the recurring event will occur in the month.
        /// </summary>
        /// <remarks>For bi-monthly recurring events, use 'DateOfMonth' for the first date 
        /// and 'SecondDateOfMonth' for the second date.</remarks>
        [Range(minimum: 1, maximum: 31)]
        public int SecondDateOfMonth { get; set; }

        /// <summary>
        /// The month that the recurring event will occur in the year.
        /// </summary>
        /// <remarks>For bi-annual recurring events, use 'AnnualMonth/AnnualMonthDate' for the first date 
        /// and 'SecondAnnualMonth/SecondAnnualMonthDate' for the second date.</remarks>
        [Range(minimum: 1, maximum: 12)]
        public int AnnualMonth  { get; set; }

        /// <summary>
        /// The date in a month that the recurring event will occur in the year.
        /// </summary>
        /// <remarks>For bi-annual recurring events, use 'AnnualMonth/AnnualMonthDate' for the first date 
        /// and 'SecondAnnualMonth/SecondAnnualMonthDate' for the second date.</remarks>
        [Range(minimum: 1, maximum: 31)]
        public int AnnualDateMonthDate { get; set; }

        /// <summary>
        /// The month that the recurring event will occur in the year.
        /// </summary>
        /// <remarks>For bi-annual recurring events, use 'AnnualMonth/AnnualMonthDate' for the first date 
        /// and 'SecondAnnualMonth/SecondAnnualMonthDate' for the second date.</remarks>
        [Range(minimum: 1, maximum: 12)]
        public int SecondAnnualMonth { get; set; }

        /// <summary>
        /// The date in a month that the recurring event will occur in the year.
        /// </summary>
        /// <remarks>For bi-annual recurring events, use 'AnnualMonth/AnnualMonthDate' for the first date 
        /// and 'SecondAnnualMonth/SecondAnnualMonthDate' for the second date.</remarks>
        [Range(minimum: 1, maximum: 31)]
        public int SecondAnnualDateMonthDate { get; set; }
    }
}