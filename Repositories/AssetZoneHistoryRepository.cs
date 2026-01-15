using IndoorLocalization.Data;
using IndoorLocalization.Models.Entities;
using IndoorLocalization.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace IndoorLocalization.Repositories
{
    public class AssetZoneHistoryRepository : IAssetRepository
    {
        private readonly IndoorLocalizationContext _context;

        public AssetZoneHistoryRepository(IndoorLocalizationContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<AssetZoneHistory>> GetActiveZonesAsync(long assetId)
        {
            return await _context.Assetzonehistories
                .Where(z => z.AssetId == assetId && z.ExitDateTime == null)
                .ToListAsync();
        }

        public async Task<IEnumerable<AssetZoneHistory>> GetByAssetIdAsync(long assetId)
        {
            return await _context.Assetzonehistories
                .Where(z => z.AssetId == assetId)
                .OrderByDescending(z => z.EnterDateTime)
                .ToListAsync();
        }

        public async Task AddAsync(AssetZoneHistory history)
        {
            _context.Assetzonehistories.Add(history);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(AssetZoneHistory history)
        {
            _context.Assetzonehistories.Update(history);
            await _context.SaveChangesAsync();
        }
    }
}
