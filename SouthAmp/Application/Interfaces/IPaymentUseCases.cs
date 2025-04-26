using System.Collections.Generic;
using System.Threading.Tasks;
using SouthAmp.Core.Entities;

namespace SouthAmp.Application.Interfaces
{
    public interface IPaymentUseCases
    {
        Task<Payment> CreatePaymentAsync(Payment payment);
        Task ConfirmPaymentAsync(int paymentId);
        Task RefundPaymentAsync(int paymentId);
        Task<IEnumerable<Payment>> GetUserPaymentsAsync(int userId);
    }
}
