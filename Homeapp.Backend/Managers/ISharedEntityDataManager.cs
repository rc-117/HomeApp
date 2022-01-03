namespace Homeapp.Backend.Managers
{
    using Homeapp.Backend.Entities;
    using Homeapp.Backend.Entities.Common.Requests;
    using System.Threading.Tasks;

    /// <summary>
    /// The shared entity data manager interface.
    /// </summary>
    public interface ISharedEntityDataManager
    {
        /// <summary>
        /// Uses a ShredEntitiesRequest object to create a SharedEntities record in the database.
        /// </summary>
        /// <param name="request">The request.</param>
        public Task<SharedEntities> CreateNewSharedEntitiesRecord(SharedEntitiesRequest request);
    }
}
