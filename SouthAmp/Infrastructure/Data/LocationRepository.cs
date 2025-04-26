using SouthAmp.Core.Entities;
using SouthAmp.Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SouthAmp.Infrastructure.Data
{
    public class LocationRepository : ILocationRepository
    {
        private readonly AppDbContext _context;
        public LocationRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task<Location> GetByIdAsync(int id)
        {
            return await _context.Set<Location>().FirstOrDefaultAsync(l => l.Id == id);
        }
        public async Task<IEnumerable<Location>> GetAllAsync()
        {
            return await _context.Set<Location>().ToListAsync();
        }
        public async Task AddAsync(Location location)
        {
            await _context.Set<Location>().AddAsync(location);
            await _context.SaveChangesAsync();
        }
        public async Task UpdateAsync(Location location)
        {
            _context.Set<Location>().Update(location);
            await _context.SaveChangesAsync();
        }
        public async Task DeleteAsync(int id)
        {
            var location = await _context.Set<Location>().FindAsync(id);
            if (location != null)
            {
                _context.Set<Location>().Remove(location);
                await _context.SaveChangesAsync();
            }
        }
    }
}