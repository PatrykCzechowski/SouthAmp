using SouthAmp.Core.Entities;
using SouthAmp.Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SouthAmp.Infrastructure.Data
{
    public class ReservationRepository : IReservationRepository
    {
        private readonly AppDbContext _context;
        public ReservationRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task<Reservation> GetByIdAsync(int id)
        {
            return await _context.Set<Reservation>().Include(r => r.Room).Include(r => r.User).FirstOrDefaultAsync(r => r.Id == id);
        }
        public async Task<IEnumerable<Reservation>> GetByUserIdAsync(int userId)
        {
            return await _context.Set<Reservation>().Where(r => r.UserId == userId).Include(r => r.Room).ToListAsync();
        }
        public async Task AddAsync(Reservation reservation)
        {
            await _context.Set<Reservation>().AddAsync(reservation);
            await _context.SaveChangesAsync();
        }
        public async Task UpdateAsync(Reservation reservation)
        {
            _context.Set<Reservation>().Update(reservation);
            await _context.SaveChangesAsync();
        }
        public async Task DeleteAsync(int id)
        {
            var reservation = await _context.Set<Reservation>().FindAsync(id);
            if (reservation != null)
            {
                _context.Set<Reservation>().Remove(reservation);
                await _context.SaveChangesAsync();
            }
        }
    }
}