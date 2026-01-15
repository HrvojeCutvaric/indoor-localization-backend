using IndoorLocalization.Models.Entities;

namespace IndoorLocalization.Services.Interfaces
{
    public interface IAssetZoneHistoryService
    {
        Task<IEnumerable<AssetZoneHistory>> GetActiveZonesAsync(long assetId);
        Task<IEnumerable<AssetZoneHistory>> GetHistoryByAssetAsync(long assetId);
        Task AddEnterAsync(long assetId, long zoneId, DateTime timestamp);
        Task CloseAsync(AssetZoneHistory history, DateTime exitTime);
    }
}
