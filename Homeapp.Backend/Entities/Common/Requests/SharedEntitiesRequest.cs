﻿namespace Homeapp.Backend.Entities.Common.Requests
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
        public Guid[] ReadHouseholdIds { get; set; }

        /// <summary>
        /// The list of household group ids that the resource will give read access to.
        /// </summary>
        [Required]
        public Guid[] ReadHouseholdGroupIds { get; set; }

        /// <summary>
        /// The list of user ids that the resource will give read access to.
        /// </summary>
        [Required]
        public Guid[] ReadUserIds { get; set; }

        /// <summary>
        /// The list of households ids that the resource will give read and edit access to.
        /// </summary>
        [Required]
        public Guid[] EditHouseholdIds { get; set; }

        /// <summary>
        /// The list of household group ids that the resource will give read and edit access to.
        /// </summary>
        [Required]
        public Guid[] EditHouseholdGroupIds { get; set; }

        /// <summary>
        /// The list of user ids that the resource will give read and edit access to.
        /// </summary>
        [Required]
        public Guid[] EditUserIds { get; set; }
    }
}
