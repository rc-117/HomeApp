﻿namespace Homeapp.Backend.Entities
{
    using Newtonsoft.Json;
    using System;

    /// <summary>
    /// The base expense class.
    /// </summary>
    public class Expense
    {
        /// <summary>
        /// The unique Id of the Expense
        /// </summary>
        [JsonProperty]
        public string Id { get; set; }

        /// <summary>
        /// The Id of the user who owns this expense
        /// </summary>
        [JsonProperty]
        public Guid UserId { get; set; }

        /// <summary>
        /// The expense category
        /// </summary>
        [JsonProperty]
        public ExpenseCategory ExpenseCategory { get; set; }

        /// <summary>
        /// The date and time the expense was created
        /// </summary>
        [JsonProperty]
        public DateTime DateTime { get; set; }


        /// <summary>
        /// The recurring type
        /// </summary>
        [JsonProperty]
        public RecurringType RecurringType { get; set; }

        /// <summary>
        /// The account for this transaction.
        /// </summary>
        [JsonProperty]
        public Account Account { get; set; }

        /// <summary>
        /// Indicates whether or not the expense has been paid for. 
        /// </summary>
        [JsonProperty]
        public bool IsPaid { get; set; }
    }
}