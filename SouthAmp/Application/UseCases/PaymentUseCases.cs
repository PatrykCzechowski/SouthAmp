using System.Collections.Generic;
using System.Threading.Tasks;
using SouthAmp.Core.Entities;
using SouthAmp.Core.Interfaces;

namespace SouthAmp.Application.UseCases
{
    public class PaymentUseCases
    {
        private readonly IPaymentRepository _paymentRepository;
        public PaymentUseCases(IPaymentRepository paymentRepository)
        {
            _paymentRepository = paymentRepository;
        }
        public async Task<Payment> CreatePaymentAsync(Payment payment)
        {
            payment.Status = PaymentStatus.Pending;
            payment.CreatedAt = System.DateTime.UtcNow;
            await _paymentRepository.AddAsync(payment);
            return payment;
        }
        public async Task ConfirmPaymentAsync(int paymentId)
        {
            var payment = await _paymentRepository.GetByIdAsync(paymentId);
            if (payment == null) throw new System.Exception("Payment not found");
            payment.Status = PaymentStatus.Confirmed;
            payment.ConfirmedAt = System.DateTime.UtcNow;
            await _paymentRepository.UpdateAsync(payment);
        }
        public async Task RefundPaymentAsync(int paymentId)
        {
            var payment = await _paymentRepository.GetByIdAsync(paymentId);
            if (payment == null) throw new System.Exception("Payment not found");
            payment.Status = PaymentStatus.Refunded;
            payment.RefundedAt = System.DateTime.UtcNow;
            await _paymentRepository.UpdateAsync(payment);
        }
        public async Task<IEnumerable<Payment>> GetUserPaymentsAsync(int userId)
        {
            return await _paymentRepository.GetByUserIdAsync(userId);
        }
    }
}