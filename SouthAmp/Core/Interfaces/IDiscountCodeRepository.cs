using SouthAmp.Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SouthAmp.Core.Interfaces
{
    public interface IDiscountCodeRepository
    {
        Task<DiscountCode> GetByIdAsync(int id);
        Task<DiscountCode> GetByCodeAsync(string code);
        Task AddAsync(DiscountCode code);
        Task UpdateAsync(DiscountCode code);
        Task DeleteAsync(int id);
        // ...inne metody repozytorium...
    }
}