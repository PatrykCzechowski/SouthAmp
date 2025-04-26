using SouthAmp.Core.Entities;
using SouthAmp.Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SouthAmp.Infrastructure.Data
{
    public class RoomRepository : IRoomRepository
    {
        private readonly AppDbContext _context;
        public RoomRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task<Room> GetByIdAsync(int id)
        {
            return await _context.Set<Room>().Include(r => r.Reservations).Include(r => r.Photos).FirstOrDefaultAsync(r => r.Id == id);
        }
        public async Task<IEnumerable<Room>> GetByHotelIdAsync(int hotelId)
        {
            return await _context.Set<Room>().Where(r => r.HotelId == hotelId).Include(r => r.Photos).ToListAsync();
        }
        public async Task AddAsync(Room room)
        {
            await _context.Set<Room>().AddAsync(room);
            await _context.SaveChangesAsync();
        }
        public async Task UpdateAsync(Room room)
        {
            _context.Set<Room>().Update(room);
            await _context.SaveChangesAsync();
        }
        public async Task DeleteAsync(int id)
        {
            var room = await _context.Set<Room>().FindAsync(id);
            if (room != null)
            {
                _context.Set<Room>().Remove(room);
                await _context.SaveChangesAsync();
            }
        }
    }
}