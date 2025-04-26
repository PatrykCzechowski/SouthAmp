using SouthAmp.Core.Entities;
using SouthAmp.Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SouthAmp.Infrastructure.Data
{
    public class ReviewRepository : IReviewRepository
    {
        private readonly AppDbContext _context;
        public ReviewRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task<Review> GetByIdAsync(int id)
        {
            return await _context.Set<Review>().Include(r => r.User).Include(r => r.Hotel).FirstOrDefaultAsync(r => r.Id == id);
        }
        public async Task<IEnumerable<Review>> GetByHotelIdAsync(int hotelId)
        {
            return await _context.Set<Review>().Where(r => r.HotelId == hotelId).Include(r => r.User).ToListAsync();
        }
        public async Task<IEnumerable<Review>> GetAllAsync()
        {
            return await _context.Set<Review>().Include(r => r.User).Include(r => r.Hotel).ToListAsync();
        }
        public async Task AddAsync(Review review)
        {
            await _context.Set<Review>().AddAsync(review);
            await _context.SaveChangesAsync();
        }
        public async Task UpdateAsync(Review review)
        {
            _context.Set<Review>().Update(review);
            await _context.SaveChangesAsync();
        }
        public async Task DeleteAsync(int id)
        {
            var review = await _context.Set<Review>().FindAsync(id);
            if (review != null)
            {
                _context.Set<Review>().Remove(review);
                await _context.SaveChangesAsync();
            }
        }
    }
}