using IndoorLocalization.Models.Entities;

namespace IndoorLocalization.Repositories.Interfaces
{
    public interface IReportsRepository
    {
        Task<IEnumerable<AssetPositionHistory>> GetPositionHistoryByRangeAsync(long assetId, DateTime from, DateTime to);
        Task<IEnumerable<AssetPositionHistory>> GetPositionHistoryByFloorMapAsync(long floorMapId, DateTime from, DateTime to);
        Task<IEnumerable<AssetZoneHistory>> GetZoneRetentionAsync(long? assetId, long? zoneId, DateTime? from, DateTime? to);
    }
}
