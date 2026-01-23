using IndoorLocalization.Models.DTOs;
using IndoorLocalization.Repositories.Interfaces;
using IndoorLocalization.Services.Interfaces;

namespace IndoorLocalization.Services
{
    public class ReportsService : IReportsService
    {

        private readonly IReportsRepository _reportsRepository;

        public ReportsService(IReportsRepository reportsRepository)
        {
            _reportsRepository = reportsRepository;
        }

        public async Task<IEnumerable<AssetPositionHistoryResponseDto>> GetPositionHistoryByRangeAsync(long assetId, DateTime from, DateTime to)
        {
            var list = await _reportsRepository.GetPositionHistoryByRangeAsync(assetId, from, to);

            return list.Select(h => new AssetPositionHistoryResponseDto
            {
                Id = h.Id,
                AssetId = h.AssetId,
                FloorMapId = h.FloorMapId,
                X = h.X,
                Y = h.Y,
                DateTime = h.DateTime
            });
        }

        public async Task<IEnumerable<AssetPositionHistoryResponseDto>> GetPositionHistoryByFloorMapAsync(long floorMapId, DateTime from, DateTime to)
        {
            var list = await _reportsRepository.GetPositionHistoryByFloorMapAsync(floorMapId, from, to);

            return list.Select(h => new AssetPositionHistoryResponseDto
            {
                Id = h.Id,
                AssetId = h.AssetId,
                FloorMapId = h.FloorMapId,
                X = h.X,
                Y = h.Y,
                DateTime = h.DateTime
            });
        }

        public async Task<IEnumerable<AssetZoneHistoryResponseDto>> GetZoneRetentionAsync(long? assetId, long? zoneId, DateTime? from, DateTime? to)
        {
            var list = await _reportsRepository.GetZoneRetentionAsync(assetId, zoneId, from, to);

            return list.Select(h => new AssetZoneHistoryResponseDto
            {
                Id = h.Id,
                AssetId = h.AssetId,
                ZoneId = h.ZoneId,
                EnterDateTime = h.EnterDateTime,
                ExitDateTime = h.ExitDateTime,
                RetentionTime = h.RetentionTime
            });
        }
    }
}
