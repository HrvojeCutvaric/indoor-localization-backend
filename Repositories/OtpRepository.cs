using IndoorLocalization.Data;
using IndoorLocalization.Models.Entities;
using IndoorLocalization.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace IndoorLocalization.Repositories
{
    public class OtpRepository : IOtpRepository
    {
        private readonly IndoorLocalizationContext _context;
        public OtpRepository(IndoorLocalizationContext context)
        {
            _context = context;
        }

        public async Task AddAsync(OtpCode otp)
        {
            _context.OtpCodes.Add(otp);
            await _context.SaveChangesAsync();
        }

        public async Task<OtpCode?> GetLatestByUserIdAsync(long userId)
        {
            return await _context.OtpCodes
                .Where(o => o.UserId == userId)
                .OrderByDescending(o => o.Id)
                .FirstOrDefaultAsync();
        }

        public async Task UpdateAsync(OtpCode otp)
        {
            _context.OtpCodes.Update(otp);
            await _context.SaveChangesAsync();
        }
    }
}
