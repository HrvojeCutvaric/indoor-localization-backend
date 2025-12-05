using IndoorLocalization.Data;
using IndoorLocalization.Models.Entities;
using IndoorLocalization.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace IndoorLocalization.Repositories
{
    public class FloorMapRepository : IFloorMapRepository
    {
        private readonly IndoorLocalizationContext _context;

        public FloorMapRepository(IndoorLocalizationContext context)
        {
            _context = context;
        }

        public async Task<List<FloorMap>> GetAllAsync()
        {
            return await _context.Floormaps.ToListAsync();
        }

        public async Task<FloorMap?> GetByIdAsync(long id)
        {
            return await _context.Floormaps.FirstOrDefaultAsync(fm => fm.Id == id);
        }

        public async Task<FloorMap?> GetByNameAsync(string name)
        {
            return await _context.Floormaps.FirstOrDefaultAsync(fm => fm.Name == name);
        }

        public async Task<bool> ExistsByNameAsync(string name)
        {
            return await _context.Floormaps.AnyAsync(fm => fm.Name == name);
        }

        public async Task AddAsync(FloorMap map)
        {
            _context.Floormaps.Add(map);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(FloorMap map)
        {
            _context.Floormaps.Update(map);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(FloorMap map)
        {
            _context.Floormaps.Remove(map);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Asset>> GetAssetsByFloorMapIdAsync(long floorMapId)
        {
            return await _context.Assets
                .Where(a => a.FloorMapId == floorMapId)
                .ToListAsync();
        }
    }
}
