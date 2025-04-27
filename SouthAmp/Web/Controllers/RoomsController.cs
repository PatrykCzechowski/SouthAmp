using Microsoft.AspNetCore.Mvc;
using SouthAmp.Application.DTOs;
using SouthAmp.Core.Entities;
using Microsoft.AspNetCore.Authorization;
using AutoMapper;
using SouthAmp.Web.Models;
using SouthAmp.Application.Interfaces;

namespace SouthAmp.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RoomsController(IRoomUseCases useCases, IMapper mapper) : ControllerBase
    {
        [HttpPost]
        [Authorize(Roles = "provider,admin")]
        public async Task<IActionResult> AddRoom([FromBody] RoomDto dto)
        {
            var room = mapper.Map<Room>(dto);
            var result = await useCases.AddRoomAsync(room);
            return Ok(new ApiResponse<RoomDto>(mapper.Map<RoomDto>(result)));
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "provider,admin")]
        public async Task<IActionResult> UpdateRoom(int id, [FromBody] RoomDto dto)
        {
            var room = mapper.Map<Room>(dto);
            room.Id = id;
            await useCases.UpdateRoomAsync(room);
            return Ok(new ApiResponse<string>("Room updated"));
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "provider,admin")]
        public async Task<IActionResult> DeleteRoom(int id)
        {
            await useCases.DeleteRoomAsync(id);
            return Ok(new ApiResponse<string>("Room deleted"));
        }

        [HttpGet("hotel/{hotelId}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetRoomsByHotelId(int hotelId)
        {
            var rooms = await useCases.GetRoomsByHotelIdAsync(hotelId);
            return Ok(new ApiResponse<IEnumerable<RoomDto>>(mapper.Map<IEnumerable<RoomDto>>(rooms)));
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetRoomById(int id)
        {
            var room = await useCases.GetRoomByIdAsync(id);
            if (room == null)
                return NotFound(new ApiResponse<string>("Room not found"));
            return Ok(new ApiResponse<RoomDto>(mapper.Map<RoomDto>(room)));
        }
    }
}