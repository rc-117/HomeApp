namespace Homeapp.Backend.Entities
{
    using System;
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Object containing share permissions to apply to a resource.
    /// </summary>
    public class SharedEntitiesRequest
    {
        /// <summary>
        /// The list of households ids that the resource will give read access to.
        /// </summary>
        [Required]
        public string[] ReadHouseholdIds { get; set; }

        /// <summary>
        /// The list of household group ids that the resource will give read access to.
        /// </summary>
        [Required]
        public string[] ReadHouseholdGroupIds { get; set; }

        /// <summary>
        /// The list of user ids that the resource will give read access to.
        /// </summary>
        [Required]
        public string[] ReadUserIds { get; set; }

        /// <summary>
        /// The list of households ids that the resource will give read and edit access to.
        /// </summary>
        [Required]
        public string[] EditHouseholdIds { get; set; }

        /// <summary>
        /// The list of household group ids that the resource will give read and edit access to.
        /// </summary>
        [Required]
        public string[] EditHouseholdGroupIds { get; set; }

        /// <summary>
        /// The list of user ids that the resource will give read and edit access to.
        /// </summary>
        [Required]
        public string[] EditUserIds { get; set; }
    }
}
