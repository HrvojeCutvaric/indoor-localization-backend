using IndoorLocalization.Data;
using IndoorLocalization.Models.Entities;
using IndoorLocalization.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace IndoorLocalization.Repositories
{
    public class ZoneRepository : IZoneRepository
    {
        private readonly IndoorLocalizationContext _context;

        public ZoneRepository(IndoorLocalizationContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Zone>> GetAllAsync()
        {
            return await _context.Zones.ToListAsync();
        }

        public async Task<IEnumerable<Zone>> GetByFloorMapAsync(long floorMapId)
        {
            return await _context.Zones
                .Where(z => z.FloorMapId == floorMapId)
                .ToListAsync();
        }

        public async Task<Zone?> GetByIdAsync(long id)
        {
            return await _context.Zones.FirstOrDefaultAsync(z => z.Id == id);
        }

        public async Task<bool> AddAsync(Zone zone)
        {
            _context.Zones.Add(zone);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> UpdateAsync(Zone zone)
        {
            _context.Zones.Update(zone);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteAsync(Zone zone)
        {
            _context.Zones.Remove(zone);
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
