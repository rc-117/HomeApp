using Homeapp.Backend.Identity;
using System.Collections.Generic;

namespace Homeapp.Backend.Entities
{
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
        /// The list of households that the resource will give access to.
        /// </summary>
        public List<Household> Households { get; set; }

        /// <summary>
        /// The list of household groups that the resource will give access to.
        /// </summary>
        public List<HouseholdGroup> HouseholdGroups { get; set; }

        /// <summary>
        /// The list of users that the resource will give access to.
        /// </summary>
        public List<User> Users { get; set; }
    }
}

// Relationship will be:
// Any resource (checbkook account, recurring expense, etc) will have a 'SharedEntities' property
// - SharedEntitiesId
// - SharedEntities
// The above is for EF core