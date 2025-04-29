using SouthAmp.Core.Entities;
using SouthAmp.Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace SouthAmp.Infrastructure.Data
{
    public class AuditLogRepository : IAuditLogRepository
    {
        private readonly AppDbContext _context;
        public AuditLogRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task<AuditLog?> GetByIdAsync(int id)
        {
            return await _context.Set<AuditLog>().FirstOrDefaultAsync(a => a.Id == id);
        }
        public async Task<IEnumerable<AuditLog>> GetByUserIdAsync(int userId)
        {
            return await _context.Set<AuditLog>().Where(a => a.UserId == userId).ToListAsync();
        }
        public async Task AddAsync(AuditLog log)
        {
            await _context.Set<AuditLog>().AddAsync(log);
            await _context.SaveChangesAsync();
        }
    }
}