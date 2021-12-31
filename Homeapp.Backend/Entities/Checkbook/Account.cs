namespace Homeapp.Backend.Entities
{
    using Homeapp.Backend.Identity;
    using Newtonsoft.Json;
    using System;

    /// <summary>
    /// Bank account class.
    /// </summary>
    public class Account
    {
        /// <summary>
        /// The unique Id of the account.
        /// </summary>
        [JsonProperty]
        public Guid Id { get; set; }

        /// <summary>
        /// The name of the account.
        /// </summary>
        [JsonProperty]
        public string Name { get; set; }

        /// <summary>
        /// The type of account.
        /// </summary>
        public AccountType AccountType { get; set; }

        /// <summary>
        /// The account's starting balance.
        /// </summary>
        [JsonProperty]
        public double StartingBalance { get; set; }

        /// <summary>
        /// The user who owns the account.
        /// </summary>
        [JsonProperty]
        public User User { get; set; }

        /// <summary>
        /// The owning user's id.
        /// </summary>
        [JsonProperty]
        public Guid UserId { get; set; }
    }
}