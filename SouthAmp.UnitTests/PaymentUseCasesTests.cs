using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Moq;
using SouthAmp.Application.UseCases;
using SouthAmp.Core.Entities;
using SouthAmp.Core.Interfaces;
using Xunit;

namespace SouthAmp.UnitTests
{
    public class PaymentUseCasesTests
    {
        private readonly Mock<IPaymentRepository> _paymentRepoMock = new();
        private readonly PaymentUseCases _sut;

        public PaymentUseCasesTests()
        {
            _sut = new PaymentUseCases(_paymentRepoMock.Object);
        }

        [Fact]
        public async Task CreatePaymentAsync_SetsDefaultsAndAdds()
        {
            var payment = new Payment { Id = 1 };
            _paymentRepoMock.Setup(r => r.AddAsync(payment)).Returns(Task.CompletedTask);
            var result = await _sut.CreatePaymentAsync(payment);
            Assert.Equal(PaymentStatus.Pending, result.Status);
            Assert.True((DateTime.UtcNow - result.CreatedAt).TotalSeconds < 5);
        }

        [Fact]
        public async Task ConfirmPaymentAsync_Throws_WhenNotFound()
        {
            _paymentRepoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync((Payment)null);
            await Assert.ThrowsAsync<Exception>(() => _sut.ConfirmPaymentAsync(1));
        }

        [Fact]
        public async Task ConfirmPaymentAsync_UpdatesStatus()
        {
            var payment = new Payment { Id = 1, Status = PaymentStatus.Pending };
            _paymentRepoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(payment);
            _paymentRepoMock.Setup(r => r.UpdateAsync(payment)).Returns(Task.CompletedTask);
            await _sut.ConfirmPaymentAsync(1);
            Assert.Equal(PaymentStatus.Confirmed, payment.Status);
            Assert.True((DateTime.UtcNow - payment.ConfirmedAt.Value).TotalSeconds < 5);
        }

        [Fact]
        public async Task RefundPaymentAsync_Throws_WhenNotFound()
        {
            _paymentRepoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync((Payment)null);
            await Assert.ThrowsAsync<Exception>(() => _sut.RefundPaymentAsync(1));
        }

        [Fact]
        public async Task RefundPaymentAsync_UpdatesStatus()
        {
            var payment = new Payment { Id = 1, Status = PaymentStatus.Confirmed };
            _paymentRepoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(payment);
            _paymentRepoMock.Setup(r => r.UpdateAsync(payment)).Returns(Task.CompletedTask);
            await _sut.RefundPaymentAsync(1);
            Assert.Equal(PaymentStatus.Refunded, payment.Status);
            Assert.True((DateTime.UtcNow - payment.RefundedAt.Value).TotalSeconds < 5);
        }

        [Fact]
        public async Task GetUserPaymentsAsync_ReturnsPayments()
        {
            var payments = new List<Payment> { new Payment { Id = 1 } };
            _paymentRepoMock.Setup(r => r.GetByUserIdAsync(1)).ReturnsAsync(payments);
            var result = await _sut.GetUserPaymentsAsync(1);
            Assert.Single(result);
        }
    }
}
