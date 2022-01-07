namespace Homeapp.Backend.Managers
{
    using Homeapp.Backend.Entities;
    using Newtonsoft.Json.Linq;
    using System;
    using System.Threading.Tasks;

    /// <summary>
    /// The shared entity data manager interface.
    /// </summary>
    public interface ISharedEntityDataManager
    {
        /// <summary>
        /// Creates a new SharedEntities object from a request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <remarks>This does not persist the sharedentities object to the database. This only creates an instance of a SharedEntities object.
        /// This method is to be used when creating other entities that require SharedEntities, and will the SharedEntities object is
        /// to be persisted to the database through other data managers on entity creation.</remarks>
        public SharedEntities CreateNewSharedEntitiesObject(SharedEntitiesRequest request);

        /// <summary>
        /// Gets a SharedEntities object by its id.
        /// </summary>
        /// <param name="id">The id to select the SharedEntities record.</param>
        public SharedEntities GetSharedEntitiesObjectFromId(Guid id);
    }
}
