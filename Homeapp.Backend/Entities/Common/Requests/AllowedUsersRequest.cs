namespace Homeapp.Backend.Entities
{
    using Newtonsoft.Json;
    using System;
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Object containing lists of users who will have access to a resource.
    /// </summary>
    public class AllowedUsersRequest
    {
        /// <summary>
        /// The list of households ids that the resource will give read access to.
        /// </summary>
        [Required(AllowEmptyStrings = true)]
        [JsonProperty]
        public string[] ReadHouseholdIds { get; set; }

        /// <summary>
        /// The list of household group ids that the resource will give read access to.
        /// </summary>
        [Required(AllowEmptyStrings = true)]
        [JsonProperty]
        public string[] ReadHouseholdGroupIds { get; set; }

        /// <summary>
        /// The list of user ids that the resource will give read access to.
        /// </summary>
        [Required(AllowEmptyStrings = true)]
        [JsonProperty]
        public string[] ReadUserIds { get; set; }

        /// <summary>
        /// The list of households ids that the resource will give read and write access to.
        /// </summary>
        [Required(AllowEmptyStrings = true)]
        [JsonProperty]
        public string[] WriteHouseholdIds { get; set; }

        /// <summary>
        /// The list of household group ids that the resource will give read and write access to.
        /// </summary>
        [Required(AllowEmptyStrings = true)]
        [JsonProperty]
        public string[] WriteHouseholdGroupIds { get; set; }

        /// <summary>
        /// The list of user ids that the resource will give read and write access to.
        /// </summary>
        [Required(AllowEmptyStrings = true)]
        [JsonProperty]
        public string[] WriteUserIds { get; set; }

        /// <summary>
        /// The list of households ids that the resource will give full access to.
        /// </summary>
        [Required(AllowEmptyStrings = true)]
        [JsonProperty]
        public string[] FullAccessHouseholdIds { get; set; }

        /// <summary>
        /// The list of household group ids that the resource will give full access to.
        /// </summary>
        [Required(AllowEmptyStrings = true)]
        [JsonProperty]
        public string[] FullAccessHouseholdGroupIds { get; set; }

        /// <summary>
        /// The list of user ids that the resource will give full access to.
        /// </summary>
        [Required(AllowEmptyStrings = true)]
        [JsonProperty]
        public string[] FullAccessUserIds { get; set; }
    }
}
