using IndoorLocalization.Data;
using IndoorLocalization.Models.Entities;
using IndoorLocalization.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace IndoorLocalization.Repositories
{
    public class AssetRepository : IAssetRepository
    {
        private readonly IndoorLocalizationContext _context;

        public AssetRepository(IndoorLocalizationContext context)
        {
            _context = context;
        }

        public async Task<bool> AddAsync(Asset asset)
        {
            _context.Assets.Add(asset);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteAsync(Asset asset)
        {
            _context.Assets.Remove(asset);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> ExistsByNameAsync(string name)
        {
            return await _context.Assets.AnyAsync(a => a.Name == name);
        }

        public async Task<IEnumerable<Asset>> GetAllAsync()
        {
            return await _context.Assets.ToListAsync();
        }

        public async Task<Asset?> GetByIdAsync(long id)
        {
            return await _context.Assets.FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<Asset?> GetByNameAsync(string name)
        {
            return await _context.Assets.FirstOrDefaultAsync(a => a.Name == name);
        }

        public async Task<bool> UpdateCoordinates(long assetId, double x, double y)
        {
            var asset = await _context.Assets.FirstOrDefaultAsync(a => a.Id == assetId);

            if (asset == null) return false;
            
            asset.X = x;
            asset.Y = y;
            asset.LastSync = DateTime.UtcNow;

            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> UpdateFloorMap(long assetId, long? floorMapId)
        {
            var asset = await _context.Assets.FirstOrDefaultAsync(a => a.Id == assetId);

            if (asset == null) return false;

            asset.FloorMapId = floorMapId;

            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> UpdateNameAndColorAsync(long assetId, string name, string color)
        {
            var asset = await _context.Assets.FirstOrDefaultAsync(a => a.Id == assetId);

            if (asset == null) return false;

            if (!string.IsNullOrWhiteSpace(name))
                asset.Name = name;

            if (!string.IsNullOrWhiteSpace(color))
                asset.Color = color;

            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> UpdateStatusAsync(long assetId, bool status)
        {
            var asset = await _context.Assets.FirstOrDefaultAsync(a => a.Id == assetId);

            if (asset == null) return false;

            asset.Active = status;

            return await _context.SaveChangesAsync() > 0;
        }
    }
}
