using SouthAmp.Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SouthAmp.Core.Interfaces
{
    public interface ILocationRepository
    {
        Task<Location> GetByIdAsync(int id);
        Task<IEnumerable<Location>> GetAllAsync();
        Task AddAsync(Location location);
        Task UpdateAsync(Location location);
        Task DeleteAsync(int id);
        // ...inne metody repozytorium...
    }
}