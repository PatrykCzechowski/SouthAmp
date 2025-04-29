using SouthAmp.Core.Entities;
using SouthAmp.Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using SouthAmp.Infrastructure.Identity;

namespace SouthAmp.Infrastructure.Data
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;
        public UserRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task<AppUserProfile?> GetByIdAsync(int id)
        {
            return await _context.AppUserProfiles.FindAsync(id);
        }
        public async Task<AppUserProfile?> GetByEmailAsync(string email)
        {
            return await _context.AppUserProfiles.FirstOrDefaultAsync(u => u.Email == email);
        }
        public async Task AddAsync(AppUserProfile user)
        {
            await _context.AppUserProfiles.AddAsync(user);
            await _context.SaveChangesAsync();
        }
        public async Task<IEnumerable<AppUser>> GetAllAsync()
        {
            return await _context.Users.ToListAsync();
        }
        public async Task UpdateAsync(AppUserProfile user)
        {
            _context.AppUserProfiles.Update(user);
            await _context.SaveChangesAsync();
        }
        public async Task DeleteAsync(int id)
        {
            var user = await _context.AppUserProfiles.FindAsync(id);
            if (user != null)
            {
                _context.AppUserProfiles.Remove(user);
                await _context.SaveChangesAsync();
            }
        }
    }
}