using SouthAmp.Core.Entities;
using SouthAmp.Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SouthAmp.Infrastructure.Data
{
    public class NotificationRepository : INotificationRepository
    {
        private readonly AppDbContext _context;
        public NotificationRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task<Notification> GetByIdAsync(int id)
        {
            return await _context.Set<Notification>().Include(n => n.User).FirstOrDefaultAsync(n => n.Id == id);
        }
        public async Task<IEnumerable<Notification>> GetByUserIdAsync(int userId)
        {
            return await _context.Set<Notification>().Where(n => n.UserId == userId).ToListAsync();
        }
        public async Task AddAsync(Notification notification)
        {
            await _context.Set<Notification>().AddAsync(notification);
            await _context.SaveChangesAsync();
        }
        public async Task UpdateAsync(Notification notification)
        {
            _context.Set<Notification>().Update(notification);
            await _context.SaveChangesAsync();
        }
        public async Task DeleteAsync(int id)
        {
            var notification = await _context.Set<Notification>().FindAsync(id);
            if (notification != null)
            {
                _context.Set<Notification>().Remove(notification);
                await _context.SaveChangesAsync();
            }
        }
    }
}