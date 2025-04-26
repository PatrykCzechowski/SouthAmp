using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Moq;
using SouthAmp.Application.UseCases;
using SouthAmp.Core.Entities;
using SouthAmp.Core.Interfaces;
using Xunit;

namespace SouthAmp.Tests
{
    public class ReservationUseCasesTests
    {
        private readonly Mock<IReservationRepository> _reservationRepoMock = new();
        private readonly Mock<IRoomRepository> _roomRepoMock = new();
        private readonly ReservationUseCases _sut;

        public ReservationUseCasesTests()
        {
            _sut = new ReservationUseCases(_reservationRepoMock.Object, _roomRepoMock.Object);
        }

        [Fact]
        public async Task CheckAvailabilityAsync_ReturnsFalse_WhenRoomNotFound()
        {
            _roomRepoMock.Setup(r => r.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((Room?)null);
            var result = await _sut.CheckAvailabilityAsync(1, DateTime.Now, DateTime.Now.AddDays(1));
            Assert.False(result);
        }

        [Fact]
        public async Task CheckAvailabilityAsync_ReturnsFalse_WhenRoomNotAvailable()
        {
            _roomRepoMock.Setup(r => r.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(new Room { IsAvailable = false });
            var result = await _sut.CheckAvailabilityAsync(1, DateTime.Now, DateTime.Now.AddDays(1));
            Assert.False(result);
        }

        [Fact]
        public async Task CheckAvailabilityAsync_ReturnsTrue_WhenNoConflictingReservations()
        {
            var room = new Room { IsAvailable = true, Reservations = new List<Reservation>() };
            _roomRepoMock.Setup(r => r.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(room);
            var result = await _sut.CheckAvailabilityAsync(1, DateTime.Now, DateTime.Now.AddDays(1));
            Assert.True(result);
        }

        [Fact]
        public async Task CheckAvailabilityAsync_ReturnsFalse_WhenConflictingReservationExists()
        {
            var start = DateTime.Now;
            var end = start.AddDays(2);
            var room = new Room
            {
                IsAvailable = true,
                Reservations = new List<Reservation>
                {
                    new Reservation { StartDate = start.AddHours(-1), EndDate = end.AddHours(1), Status = ReservationStatus.Confirmed }
                }
            };
            _roomRepoMock.Setup(r => r.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(room);
            var result = await _sut.CheckAvailabilityAsync(1, start, end);
            Assert.False(result);
        }

        [Fact]
        public async Task CreateReservationAsync_Throws_WhenRoomNotAvailable()
        {
            var reservation = new Reservation { RoomId = 1, StartDate = DateTime.Now, EndDate = DateTime.Now.AddDays(1) };
            _roomRepoMock.Setup(r => r.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(new Room { IsAvailable = false });
            await Assert.ThrowsAsync<Exception>(() => _sut.CreateReservationAsync(reservation));
        }

        [Fact]
        public async Task CreateReservationAsync_AddsReservation_WhenAvailable()
        {
            var reservation = new Reservation { RoomId = 1, StartDate = DateTime.Now, EndDate = DateTime.Now.AddDays(1) };
            _roomRepoMock.Setup(r => r.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(new Room { IsAvailable = true, Reservations = new List<Reservation>() });
            _reservationRepoMock.Setup(r => r.AddAsync(It.IsAny<Reservation>())).Returns(Task.CompletedTask);
            var result = await _sut.CreateReservationAsync(reservation);
            Assert.Equal(ReservationStatus.Confirmed, result.Status);
            Assert.True((DateTime.UtcNow - result.CreatedAt).TotalSeconds < 5);
        }

        [Fact]
        public async Task GetUserReservationsAsync_ReturnsReservations()
        {
            var reservations = new List<Reservation> { new Reservation { Id = 1 } };
            _reservationRepoMock.Setup(r => r.GetByUserIdAsync(1)).ReturnsAsync(reservations);
            var result = await _sut.GetUserReservationsAsync(1);
            Assert.Single(result);
        }

        [Fact]
        public async Task CancelReservationAsync_Throws_WhenNotFound()
        {
            _reservationRepoMock.Setup(r => r.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((Reservation?)null);
            await Assert.ThrowsAsync<Exception>(() => _sut.CancelReservationAsync(1));
        }

        [Fact]
        public async Task CancelReservationAsync_UpdatesStatus()
        {
            var reservation = new Reservation { Id = 1, Status = ReservationStatus.Confirmed };
            _reservationRepoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(reservation);
            _reservationRepoMock.Setup(r => r.UpdateAsync(reservation)).Returns(Task.CompletedTask);
            await _sut.CancelReservationAsync(1);
            Assert.Equal(ReservationStatus.Cancelled, reservation.Status);
            Assert.True((DateTime.UtcNow - reservation.CancelledAt.Value).TotalSeconds < 5);
        }

        [Fact]
        public async Task ChangeReservationDateAsync_Throws_WhenNotFound()
        {
            _reservationRepoMock.Setup(r => r.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((Reservation?)null);
            await Assert.ThrowsAsync<Exception>(() => _sut.ChangeReservationDateAsync(1, DateTime.Now, DateTime.Now.AddDays(1)));
        }

        [Fact]
        public async Task ChangeReservationDateAsync_Throws_WhenRoomNotAvailable()
        {
            var reservation = new Reservation { Id = 1, RoomId = 2 };
            _reservationRepoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(reservation);
            _roomRepoMock.Setup(r => r.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(new Room { IsAvailable = false });
            await Assert.ThrowsAsync<Exception>(() => _sut.ChangeReservationDateAsync(1, DateTime.Now, DateTime.Now.AddDays(1)));
        }

        [Fact]
        public async Task ChangeReservationDateAsync_UpdatesDates_WhenAvailable()
        {
            var reservation = new Reservation { Id = 1, RoomId = 2, StartDate = DateTime.Now, EndDate = DateTime.Now.AddDays(1) };
            _reservationRepoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(reservation);
            _roomRepoMock.Setup(r => r.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(new Room { IsAvailable = true, Reservations = new List<Reservation>() });
            _reservationRepoMock.Setup(r => r.UpdateAsync(reservation)).Returns(Task.CompletedTask);
            var newStart = DateTime.Now.AddDays(2);
            var newEnd = DateTime.Now.AddDays(3);
            await _sut.ChangeReservationDateAsync(1, newStart, newEnd);
            Assert.Equal(newStart, reservation.StartDate);
            Assert.Equal(newEnd, reservation.EndDate);
        }
    }
}
