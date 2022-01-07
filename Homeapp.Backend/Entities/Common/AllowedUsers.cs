namespace Homeapp.Backend.Entities
{
    using System;
    
    /// <summary>
    /// Object containing a list of entities that have access to a resource.
    /// </summary>
    /// <remarks>Lists are seperated with semi colon (;)</remarks>
    public class AllowedUsers
    {
        /// <summary>
        /// The unique id of this object.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// The list of households ids that the resource will give read access to.
        /// </summary>
        public string ReadHouseholdIds { get; set; }

        /// <summary>
        /// The list of household group ids that the resource will give read access to.
        /// </summary>
        public string ReadHouseholdGroupIds { get; set; }

        /// <summary>
        /// The list of user ids that the resource will give read access to.
        /// </summary>
        public string ReadUserIds { get; set; }

        /// <summary>
        /// The list of households ids that the resource will give read and write access to.
        /// </summary>
        public string WriteHouseholdIds { get; set; }

        /// <summary>
        /// The list of household group ids that the resource will give read and write access to.
        /// </summary>
        public string WriteHouseholdGroupIds { get; set; }

        /// <summary>
        /// The list of user ids that the resource will give read and write access to.
        /// </summary>
        public string WriteUserIds { get; set; }

        /// <summary>
        /// The list of households ids that the resource will give full access to.
        /// </summary>
        public string FullAccessHouseholdIds { get; set; }

        /// <summary>
        /// The list of household group ids that the resource will give full access to.
        /// </summary>
        public string FullAccessHouseholdGroupIds { get; set; }

        /// <summary>
        /// The list of user ids that the resource will give full access to.
        /// </summary>
        public string FullAccessUserIds { get; set; }
    }
}