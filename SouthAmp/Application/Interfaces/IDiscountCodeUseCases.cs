using System.Threading.Tasks;
using SouthAmp.Core.Entities;

namespace SouthAmp.Application.Interfaces
{
    public interface IDiscountCodeUseCases
    {
        Task<DiscountCode> CreateDiscountCodeAsync(DiscountCode code);
        Task<DiscountCode> GetByCodeAsync(string code);
        Task UseCodeAsync(string code);
    }
}
