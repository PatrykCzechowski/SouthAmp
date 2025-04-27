using System;
using System.Threading.Tasks;
using Moq;
using SouthAmp.Application.UseCases;
using SouthAmp.Core.Entities;
using SouthAmp.Core.Interfaces;
using Xunit;

namespace SouthAmp.UnitTests
{
    public class DiscountCodeUseCasesTests
    {
        private readonly Mock<IDiscountCodeRepository> _discountRepoMock = new();
        private readonly DiscountCodeUseCases _sut;

        public DiscountCodeUseCasesTests()
        {
            _sut = new DiscountCodeUseCases(_discountRepoMock.Object);
        }

        [Fact]
        public async Task CreateDiscountCodeAsync_SetsDefaultsAndAdds()
        {
            var code = new DiscountCode { Id = 1 };
            _discountRepoMock.Setup(r => r.AddAsync(code)).Returns(Task.CompletedTask);
            var result = await _sut.CreateDiscountCodeAsync(code);
            Assert.True(result.IsActive);
            Assert.Equal(0, result.UsedCount);
        }

        [Fact]
        public async Task GetByCodeAsync_ReturnsDiscountCode()
        {
            var code = new DiscountCode { Id = 1, Code = "ABC" };
            _discountRepoMock.Setup(r => r.GetByCodeAsync("ABC")).ReturnsAsync(code);
            var result = await _sut.GetByCodeAsync("ABC");
            Assert.Equal(code, result);
        }

        [Fact]
        public async Task UseCodeAsync_Throws_WhenCodeNotFound()
        {
            _discountRepoMock.Setup(r => r.GetByCodeAsync("X")).ReturnsAsync((DiscountCode)null);
            await Assert.ThrowsAsync<Exception>(() => _sut.UseCodeAsync("X"));
        }

        [Fact]
        public async Task UseCodeAsync_Throws_WhenCodeNotActive()
        {
            var code = new DiscountCode { Code = "A", IsActive = false };
            _discountRepoMock.Setup(r => r.GetByCodeAsync("A")).ReturnsAsync(code);
            await Assert.ThrowsAsync<Exception>(() => _sut.UseCodeAsync("A"));
        }

        [Fact]
        public async Task UseCodeAsync_Throws_WhenUsageLimitReached()
        {
            var code = new DiscountCode { Code = "A", IsActive = true, UsageLimit = 1, UsedCount = 1, ValidTo = DateTime.UtcNow.AddDays(1) };
            _discountRepoMock.Setup(r => r.GetByCodeAsync("A")).ReturnsAsync(code);
            await Assert.ThrowsAsync<Exception>(() => _sut.UseCodeAsync("A"));
        }

        [Fact]
        public async Task UseCodeAsync_Throws_WhenCodeExpired()
        {
            var code = new DiscountCode { Code = "A", IsActive = true, UsedCount = 0, ValidTo = DateTime.UtcNow.AddDays(-1) };
            _discountRepoMock.Setup(r => r.GetByCodeAsync("A")).ReturnsAsync(code);
            await Assert.ThrowsAsync<Exception>(() => _sut.UseCodeAsync("A"));
        }

        [Fact]
        public async Task UseCodeAsync_IncrementsUsedCount_WhenValid()
        {
            var code = new DiscountCode { Code = "A", IsActive = true, UsedCount = 0, ValidTo = DateTime.UtcNow.AddDays(1) };
            _discountRepoMock.Setup(r => r.GetByCodeAsync("A")).ReturnsAsync(code);
            _discountRepoMock.Setup(r => r.UpdateAsync(code)).Returns(Task.CompletedTask);
            await _sut.UseCodeAsync("A");
            Assert.Equal(1, code.UsedCount);
        }
    }
}
