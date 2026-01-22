using IndoorLocalization.Models.DTOs;

namespace IndoorLocalization.Services.Interfaces
{
    public interface IReportsService
    {
        Task<IEnumerable<AssetPositionHistoryResponseDto>> GetPositionHistoryByRangeAsync(long assetId, DateTime from, DateTime to);
        Task<IEnumerable<AssetPositionHistoryResponseDto>> GetPositionHistoryByFloorMapAsync(long floorMapId, DateTime from, DateTime to);
        Task<IEnumerable<AssetZoneHistoryResponseDto>> GetZoneRetentionAsync(long? assetId, long? zoneId, DateTime? from, DateTime? to);
    }
}
