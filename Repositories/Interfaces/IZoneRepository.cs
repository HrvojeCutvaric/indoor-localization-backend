using IndoorLocalization.Models.Entities;

namespace IndoorLocalization.Repositories.Interfaces
{
    public interface IZoneRepository
    {
        Task<IEnumerable<Zone>> GetAllAsync();
        Task<IEnumerable<Zone>> GetByFloorMapAsync(long floorMapId);
        Task<Zone?> GetByIdAsync(long id);
        Task<bool> AddAsync(Zone zone);
        Task<bool> UpdateAsync(Zone zone);
        Task<bool> DeleteAsync(Zone zone);
    }
}
