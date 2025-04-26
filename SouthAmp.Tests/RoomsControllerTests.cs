using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SouthAmp.Application.DTOs;
using SouthAmp.Application.Interfaces;
using SouthAmp.Application.UseCases;
using SouthAmp.Core.Entities;
using SouthAmp.Web.Controllers;
using SouthAmp.Web.Models;
using Xunit;

namespace SouthAmp.Tests
{
    public class RoomsControllerTests
    {
        private readonly Mock<IRoomUseCases> _useCasesMock = new(MockBehavior.Strict);
        private readonly Mock<IMapper> _mapperMock = new();
        private readonly RoomsController _controller;
        public RoomsControllerTests()
        {
            _controller = new RoomsController(_useCasesMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async Task AddRoom_ReturnsOk()
        {
            var dto = new RoomDto { HotelId = 2 };
            var room = new Room { HotelId = 2 };
            var resultRoom = new Room { Id = 1, HotelId = 2 };
            var resultDto = new RoomDto { Id = 1, HotelId = 2 };
            _mapperMock.Setup(m => m.Map<Room>(dto)).Returns(room);
            _useCasesMock.Setup(u => u.AddRoomAsync(room)).ReturnsAsync(resultRoom);
            _mapperMock.Setup(m => m.Map<RoomDto>(resultRoom)).Returns(resultDto);
            var result = await _controller.AddRoom(dto);
            var ok = Assert.IsType<OkObjectResult>(result);
            var apiResponse = Assert.IsType<ApiResponse<RoomDto>>(ok.Value);
            Assert.Equal(resultDto.Id, apiResponse.Data.Id);
        }

        [Fact]
        public async Task UpdateRoom_ReturnsOk()
        {
            var dto = new RoomDto { Id = 1, HotelId = 2 };
            var room = new Room { Id = 1, HotelId = 2 };
            _mapperMock.Setup(m => m.Map<Room>(dto)).Returns(room);
            _useCasesMock.Setup(u => u.UpdateRoomAsync(room)).Returns(Task.CompletedTask);
            var result = await _controller.UpdateRoom(1, dto);
            var ok = Assert.IsType<OkObjectResult>(result);
            var apiResponse = Assert.IsType<ApiResponse<string>>(ok.Value);
            Assert.Equal("Room updated", apiResponse.Message);
        }

        [Fact]
        public async Task DeleteRoom_ReturnsOk()
        {
            _useCasesMock.Setup(u => u.DeleteRoomAsync(1)).Returns(Task.CompletedTask);
            var result = await _controller.DeleteRoom(1);
            var ok = Assert.IsType<OkObjectResult>(result);
            var apiResponse = Assert.IsType<ApiResponse<string>>(ok.Value);
            Assert.Equal("Room deleted", apiResponse.Message);
        }

        [Fact]
        public async Task GetRoomsByHotelId_ReturnsOk()
        {
            var rooms = new List<Room> { new Room { Id = 1, HotelId = 2 } };
            var dtos = new List<RoomDto> { new RoomDto { Id = 1, HotelId = 2 } };
            _useCasesMock.Setup(u => u.GetRoomsByHotelIdAsync(2)).ReturnsAsync(rooms);
            _mapperMock.Setup(m => m.Map<IEnumerable<RoomDto>>(rooms)).Returns(dtos);
            var result = await _controller.GetRoomsByHotelId(2);
            var ok = Assert.IsType<OkObjectResult>(result);
            var apiResponse = Assert.IsType<ApiResponse<IEnumerable<RoomDto>>>(ok.Value);
            Assert.Single(apiResponse.Data);
        }

        [Fact]
        public async Task GetRoomById_ReturnsOk_WhenFound()
        {
            var room = new Room { Id = 1 };
            var dto = new RoomDto { Id = 1 };
            _useCasesMock.Setup(u => u.GetRoomByIdAsync(1)).ReturnsAsync(room);
            _mapperMock.Setup(m => m.Map<RoomDto>(room)).Returns(dto);
            var result = await _controller.GetRoomById(1);
            var ok = Assert.IsType<OkObjectResult>(result);
            var apiResponse = Assert.IsType<ApiResponse<RoomDto>>(ok.Value);
            Assert.Equal(dto.Id, apiResponse.Data.Id);
        }

        [Fact]
        public async Task GetRoomById_ReturnsNotFound_WhenNull()
        {
            _useCasesMock.Setup(u => u.GetRoomByIdAsync(1)).ReturnsAsync((Room)null);
            var result = await _controller.GetRoomById(1);
            var notFound = Assert.IsType<NotFoundObjectResult>(result);
            var apiResponse = Assert.IsType<ApiResponse<string>>(notFound.Value);
            Assert.Equal("Room not found", apiResponse.Message);
        }
    }
}
