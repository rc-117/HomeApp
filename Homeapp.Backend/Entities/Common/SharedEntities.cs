namespace Homeapp.Backend.Entities
{
    using Homeapp.Backend.Identity;
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Object containing a list of entities that have access to a resource.
    /// </summary>
    public class SharedEntities
    {
        /// <summary>
        /// The unique id of this object.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The list of households ids that the resource will give read access to.
        /// </summary>
        public List<Guid> ReadHouseholdIds { get; set; }

        /// <summary>
        /// The list of household group ids that the resource will give read access to.
        /// </summary>
        public List<Guid> ReadHouseholdGroupIds { get; set; }

        /// <summary>
        /// The list of user ids that the resource will give read access to.
        /// </summary>
        public List<Guid> ReadUserIds { get; set; }

        /// <summary>
        /// The list of households ids that the resource will give read and edit access to.
        /// </summary>
        public List<Guid> EditHouseholdIds { get; set; }

        /// <summary>
        /// The list of household group ids that the resource will give read and edit access to.
        /// </summary>
        public List<Guid> EditHouseholdGroupIds { get; set; }

        /// <summary>
        /// The list of user ids that the resource will give read and edit access to.
        /// </summary>
        public List<Guid> EditUserIds { get; set; }
    }
}

// Relationship will be:
// Any resource (checbkook account, recurring expense, etc) will have a 'SharedEntities' property
// - SharedEntitiesId
// - SharedEntities
// The above is for EF core