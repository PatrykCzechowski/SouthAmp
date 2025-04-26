using SouthAmp.Core.Entities;
using SouthAmp.Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SouthAmp.Infrastructure.Data
{
    public class ReportRepository : IReportRepository
    {
        private readonly AppDbContext _context;
        public ReportRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task<Report> GetByIdAsync(int id)
        {
            return await _context.Set<Report>().FirstOrDefaultAsync(r => r.Id == id);
        }
        public async Task<IEnumerable<Report>> GetByUserIdAsync(int userId)
        {
            return await _context.Set<Report>().Where(r => r.UserId == userId).ToListAsync();
        }
        public async Task AddAsync(Report report)
        {
            await _context.Set<Report>().AddAsync(report);
            await _context.SaveChangesAsync();
        }
        public async Task UpdateAsync(Report report)
        {
            _context.Set<Report>().Update(report);
            await _context.SaveChangesAsync();
        }
        public async Task DeleteAsync(int id)
        {
            var report = await _context.Set<Report>().FindAsync(id);
            if (report != null)
            {
                _context.Set<Report>().Remove(report);
                await _context.SaveChangesAsync();
            }
        }
    }
}