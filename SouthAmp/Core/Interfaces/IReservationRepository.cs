using SouthAmp.Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SouthAmp.Core.Interfaces
{
    public interface IReservationRepository
    {
        Task<Reservation> GetByIdAsync(int id);
        Task<IEnumerable<Reservation>> GetByUserIdAsync(int userId);
        Task AddAsync(Reservation reservation);
        Task UpdateAsync(Reservation reservation);
        Task DeleteAsync(int id);
        // ...inne metody repozytorium...
    }
}