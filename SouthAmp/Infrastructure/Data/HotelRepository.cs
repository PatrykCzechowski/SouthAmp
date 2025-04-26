using SouthAmp.Core.Entities;
using SouthAmp.Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SouthAmp.Infrastructure.Data
{
    public class HotelRepository : IHotelRepository
    {
        private readonly AppDbContext _context;
        public HotelRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task<Hotel> GetByIdAsync(int id)
        {
            return await _context.Set<Hotel>().Include(h => h.Rooms).Include(h => h.Reviews).FirstOrDefaultAsync(h => h.Id == id);
        }
        public async Task<IEnumerable<Hotel>> GetAllAsync()
        {
            return await _context.Set<Hotel>().Include(h => h.Rooms).Include(h => h.Reviews).ToListAsync();
        }
        public async Task AddAsync(Hotel hotel)
        {
            await _context.Set<Hotel>().AddAsync(hotel);
            await _context.SaveChangesAsync();
        }
        public async Task UpdateAsync(Hotel hotel)
        {
            _context.Set<Hotel>().Update(hotel);
            await _context.SaveChangesAsync();
        }
        public async Task DeleteAsync(int id)
        {
            var hotel = await _context.Set<Hotel>().FindAsync(id);
            if (hotel != null)
            {
                _context.Set<Hotel>().Remove(hotel);
                await _context.SaveChangesAsync();
            }
        }
    }
}