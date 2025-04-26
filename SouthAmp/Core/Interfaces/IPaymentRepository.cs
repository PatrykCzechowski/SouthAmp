using SouthAmp.Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SouthAmp.Core.Interfaces
{
    public interface IPaymentRepository
    {
        Task<Payment> GetByIdAsync(int id);
        Task<IEnumerable<Payment>> GetByUserIdAsync(int userId);
        Task<IEnumerable<Payment>> GetAllAsync();
        Task AddAsync(Payment payment);
        Task UpdateAsync(Payment payment);
        Task DeleteAsync(int id);
        // ...inne metody repozytorium...
    }
}