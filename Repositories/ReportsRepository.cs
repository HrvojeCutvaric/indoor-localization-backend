using IndoorLocalization.Data;
using IndoorLocalization.Models.Entities;
using IndoorLocalization.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace IndoorLocalization.Repositories
{
    public class ReportsRepository : IReportsRepository
    {

        private readonly IndoorLocalizationContext _context;

        public ReportsRepository(IndoorLocalizationContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<AssetPositionHistory>> GetPositionHistoryByRangeAsync(long assetId, DateTime from, DateTime to)
        {
            return await _context.Assetpositionhistories
                .Where(aph => aph.AssetId == assetId && aph.DateTime >= from && aph.DateTime <= to)
                .OrderBy(aph => aph.DateTime)
                .ToListAsync();
        }

        public async Task<IEnumerable<AssetPositionHistory>> GetPositionHistoryByFloorMapAsync(long floorMapId, DateTime from, DateTime to)
        {
            return await _context.Assetpositionhistories
                .Where(aph => aph.FloorMapId == floorMapId && aph.DateTime >= from && aph.DateTime <= to)
                .OrderBy(aph => aph.DateTime)
                .ToListAsync();
        }

        public async Task<IEnumerable<AssetZoneHistory>> GetZoneRetentionAsync(long? assetId, long? zoneId, DateTime? from, DateTime? to)
        {
            var query = _context.Assetzonehistories.AsQueryable();

            if (assetId.HasValue)
                query = query.Where(x => x.AssetId == assetId.Value);

            if (zoneId.HasValue)
                query = query.Where(x => x.ZoneId == zoneId.Value);

            if (from.HasValue)
                query = query.Where(x => x.EnterDateTime >= from.Value);

            if (to.HasValue)
                query = query.Where(x =>
                    (x.ExitDateTime ?? DateTime.UtcNow) <= to.Value
                );

            return await query
                .OrderBy(x => x.EnterDateTime)
                .ToListAsync();
        }
    }
}
