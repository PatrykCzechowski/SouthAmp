using SouthAmp.Application.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;
using SouthAmp.Core.Entities;
using SouthAmp.Core.Interfaces;

namespace SouthAmp.Application.UseCases
{
    public class DiscountCodeUseCases : IDiscountCodeUseCases
    {
        private readonly IDiscountCodeRepository _discountCodeRepository;
        public DiscountCodeUseCases(IDiscountCodeRepository discountCodeRepository)
        {
            _discountCodeRepository = discountCodeRepository;
        }
        public async Task<DiscountCode> CreateDiscountCodeAsync(DiscountCode code)
        {
            code.IsActive = true;
            code.UsedCount = 0;
            await _discountCodeRepository.AddAsync(code);
            return code;
        }
        public async Task<DiscountCode> GetByCodeAsync(string code)
        {
            return await _discountCodeRepository.GetByCodeAsync(code);
        }
        public async Task UseCodeAsync(string code)
        {
            var discount = await _discountCodeRepository.GetByCodeAsync(code);
            if (discount == null || !discount.IsActive) throw new System.Exception("Invalid code");
            if (discount.UsageLimit.HasValue && discount.UsedCount >= discount.UsageLimit.Value) throw new System.Exception("Code usage limit reached");
            if (discount.ValidTo < System.DateTime.UtcNow) throw new System.Exception("Code expired");
            discount.UsedCount++;
            await _discountCodeRepository.UpdateAsync(discount);
        }
    }
}