using SouthAmp.Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SouthAmp.Core.Interfaces
{
    public interface IHotelRepository
    {
        Task<Hotel> GetByIdAsync(int id);
        Task<IEnumerable<Hotel>> GetAllAsync();
        Task AddAsync(Hotel hotel);
        Task UpdateAsync(Hotel hotel);
        Task DeleteAsync(int id);
        // ...inne metody repozytorium...
    }
}