using SouthAmp.Core.Entities;
using SouthAmp.Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SouthAmp.Infrastructure.Data
{
    public class DiscountCodeRepository : IDiscountCodeRepository
    {
        private readonly AppDbContext _context;
        public DiscountCodeRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task<DiscountCode> GetByIdAsync(int id)
        {
            return await _context.Set<DiscountCode>().FirstOrDefaultAsync(d => d.Id == id);
        }
        public async Task<DiscountCode> GetByCodeAsync(string code)
        {
            return await _context.Set<DiscountCode>().FirstOrDefaultAsync(d => d.Code == code);
        }
        public async Task AddAsync(DiscountCode code)
        {
            await _context.Set<DiscountCode>().AddAsync(code);
            await _context.SaveChangesAsync();
        }
        public async Task UpdateAsync(DiscountCode code)
        {
            _context.Set<DiscountCode>().Update(code);
            await _context.SaveChangesAsync();
        }
        public async Task DeleteAsync(int id)
        {
            var code = await _context.Set<DiscountCode>().FindAsync(id);
            if (code != null)
            {
                _context.Set<DiscountCode>().Remove(code);
                await _context.SaveChangesAsync();
            }
        }
    }
}