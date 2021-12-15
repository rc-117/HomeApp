﻿namespace Homeapp.Backend.Entities
{
    using Homeapp.Backend.Identity;
    using Newtonsoft.Json;
    using System;

    /// <summary>
    /// An object containing properties to create an account.
    /// </summary>
    public class CreateAccountRequest
    {
        /// <summary>
        /// The name of the account.
        /// </summary>
        [JsonProperty]
        public string Name { get; set; }

        /// <summary>
        /// The type of account.
        /// </summary>
        [JsonProperty]
        public string AccountType { get; set; }

        /// <summary>
        /// The account's starting balance.
        /// </summary>
        [JsonProperty]
        public double StartingBalance { get; set; }
    }
}