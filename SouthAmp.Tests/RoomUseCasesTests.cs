using System.Collections.Generic;
using System.Threading.Tasks;
using Moq;
using SouthAmp.Application.UseCases;
using SouthAmp.Core.Entities;
using SouthAmp.Core.Interfaces;
using Xunit;

namespace SouthAmp.Tests
{
    public class RoomUseCasesTests
    {
        private readonly Mock<IRoomRepository> _roomRepoMock = new();
        private readonly RoomUseCases _sut;

        public RoomUseCasesTests()
        {
            _sut = new RoomUseCases(_roomRepoMock.Object);
        }

        [Fact]
        public async Task AddRoomAsync_AddsRoom()
        {
            var room = new Room { Id = 1 };
            _roomRepoMock.Setup(r => r.AddAsync(room)).Returns(Task.CompletedTask);
            var result = await _sut.AddRoomAsync(room);
            Assert.Equal(room, result);
        }

        [Fact]
        public async Task UpdateRoomAsync_UpdatesRoom()
        {
            var room = new Room { Id = 1 };
            _roomRepoMock.Setup(r => r.UpdateAsync(room)).Returns(Task.CompletedTask);
            await _sut.UpdateRoomAsync(room);
            _roomRepoMock.Verify(r => r.UpdateAsync(room), Times.Once);
        }

        [Fact]
        public async Task DeleteRoomAsync_DeletesRoom()
        {
            _roomRepoMock.Setup(r => r.DeleteAsync(1)).Returns(Task.CompletedTask);
            await _sut.DeleteRoomAsync(1);
            _roomRepoMock.Verify(r => r.DeleteAsync(1), Times.Once);
        }

        [Fact]
        public async Task GetRoomByIdAsync_ReturnsRoom()
        {
            var room = new Room { Id = 1 };
            _roomRepoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(room);
            var result = await _sut.GetRoomByIdAsync(1);
            Assert.Equal(room, result);
        }

        [Fact]
        public async Task GetRoomsByHotelIdAsync_ReturnsRooms()
        {
            var rooms = new List<Room> { new Room { Id = 1 } };
            _roomRepoMock.Setup(r => r.GetByHotelIdAsync(1)).ReturnsAsync(rooms);
            var result = await _sut.GetRoomsByHotelIdAsync(1);
            Assert.Single(result);
        }
    }
}
