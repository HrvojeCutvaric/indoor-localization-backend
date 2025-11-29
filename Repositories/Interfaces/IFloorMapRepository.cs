using IndoorLocalization.Models.Entities;

namespace IndoorLocalization.Repositories.Interfaces
{
    public interface IFloorMapRepository
    {
        Task<List<FloorMap>> GetAllAsync();
        Task<FloorMap?> GetByIdAsync(long id);
        Task<FloorMap?> GetByNameAsync(string name);
        Task<bool> ExistsByNameAsync(string name);

        Task AddAsync(FloorMap map);
        Task UpdateAsync(FloorMap map);
        Task DeleteAsync(FloorMap map);

        Task<List<Asset>> GetAssetsByFloorMapIdAsync(long floorMapId);


    }
}
