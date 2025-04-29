using SouthAmp.Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;
using SouthAmp.Infrastructure.Identity;

namespace SouthAmp.Core.Interfaces
{
    public interface IUserRepository
    {
        Task<AppUserProfile?> GetByIdAsync(int id);
        Task<AppUserProfile?> GetByEmailAsync(string email);
        Task AddAsync(AppUserProfile user);
        Task<IEnumerable<AppUser>> GetAllAsync();
        Task UpdateAsync(AppUserProfile user);
        Task DeleteAsync(int id);
    }
}