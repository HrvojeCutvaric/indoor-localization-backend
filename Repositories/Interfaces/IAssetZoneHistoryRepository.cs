using IndoorLocalization.Models.Entities;

namespace IndoorLocalization.Repositories.Interfaces
{
    public interface IAssetZoneHistoryRepository
    {
        Task<IEnumerable<AssetZoneHistory>> GetActiveZonesAsync(long assetId);
        Task AddAsync(AssetZoneHistory history);
        Task UpdateAsync(AssetZoneHistory history);
        Task<IEnumerable<AssetZoneHistory>> GetByAssetIdAsync(long assetId);
    }
}
