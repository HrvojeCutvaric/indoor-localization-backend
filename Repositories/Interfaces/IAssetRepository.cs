using IndoorLocalization.Models.Entities;

namespace IndoorLocalization.Repositories.Interfaces
{
    public interface IAssetRepository
    {
        Task<IEnumerable<Asset>> GetAllAsync();
        Task<Asset?> GetByIdAsync(long id);
        Task<Asset?> GetByNameAsync(string name);
        Task<List<Asset>> GetAssetsByFloorMapIdAsync(long floorMapId);
        Task<bool> ExistsByNameAsync(string name);
        Task<bool> AddAsync(Asset asset);
        Task<bool> UpdateNameAndColorAsync(long assetId, string name, string color);
        Task<bool> UpdateStatusAsync(long assetId, bool status);
        Task<bool> UpdateCoordinates(long assetId, double x, double y);
        Task<bool> UpdateFloorMap(long assetId, long? floorMapId);
        Task<bool> DeleteAsync(Asset asset);
        
        Task<IEnumerable<AssetPositionHistory>> GetPositionHistoryAsync(long assetId);
        Task<IEnumerable<AssetZoneHistory>> GetZoneHistoryAsync(long assetId);


    }
}
