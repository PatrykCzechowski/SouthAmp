using Microsoft.AspNetCore.Mvc;
using SouthAmp.Application.UseCases;
using SouthAmp.Application.DTOs;
using SouthAmp.Core.Entities;
using Microsoft.AspNetCore.Authorization;
using AutoMapper;
using SouthAmp.Web.Models;

namespace SouthAmp.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RoomsController : ControllerBase
    {
        private readonly RoomUseCases _roomUseCases;
        private readonly IMapper _mapper;
        public RoomsController(RoomUseCases roomUseCases, IMapper mapper)
        {
            _roomUseCases = roomUseCases;
            _mapper = mapper;
        }

        [HttpPost]
        [Authorize(Roles = "provider,admin")]
        public async Task<IActionResult> AddRoom([FromBody] RoomDto dto)
        {
            var room = _mapper.Map<Room>(dto);
            var result = await _roomUseCases.AddRoomAsync(room);
            return Ok(new ApiResponse<RoomDto>(_mapper.Map<RoomDto>(result)));
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "provider,admin")]
        public async Task<IActionResult> UpdateRoom(int id, [FromBody] RoomDto dto)
        {
            var room = _mapper.Map<Room>(dto);
            room.Id = id;
            await _roomUseCases.UpdateRoomAsync(room);
            return Ok(new ApiResponse<string>("Room updated"));
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "provider,admin")]
        public async Task<IActionResult> DeleteRoom(int id)
        {
            await _roomUseCases.DeleteRoomAsync(id);
            return Ok(new ApiResponse<string>("Room deleted"));
        }

        [HttpGet("hotel/{hotelId}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetRoomsByHotelId(int hotelId)
        {
            var rooms = await _roomUseCases.GetRoomsByHotelIdAsync(hotelId);
            return Ok(new ApiResponse<IEnumerable<RoomDto>>(_mapper.Map<IEnumerable<RoomDto>>(rooms)));
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetRoomById(int id)
        {
            var room = await _roomUseCases.GetRoomByIdAsync(id);
            if (room == null) return NotFound(new ApiResponse<string>("Room not found"));
            return Ok(new ApiResponse<RoomDto>(_mapper.Map<RoomDto>(room)));
        }
    }
}