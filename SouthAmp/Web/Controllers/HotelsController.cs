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
    public class HotelsController : ControllerBase
    {
        private readonly HotelUseCases _hotelUseCases;
        private readonly IMapper _mapper;
        public HotelsController(HotelUseCases hotelUseCases, IMapper mapper)
        {
            _hotelUseCases = hotelUseCases;
            _mapper = mapper;
        }

        [HttpPost]
        [Authorize(Roles = "provider,admin")]
        public async Task<IActionResult> AddHotel([FromBody] HotelDto dto)
        {
            var hotel = _mapper.Map<Hotel>(dto);
            var result = await _hotelUseCases.AddHotelAsync(hotel);
            return Ok(new ApiResponse<HotelDto>(_mapper.Map<HotelDto>(result)));
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "provider,admin")]
        public async Task<IActionResult> UpdateHotel(int id, [FromBody] HotelDto dto)
        {
            var hotel = _mapper.Map<Hotel>(dto);
            hotel.Id = id;
            await _hotelUseCases.UpdateHotelAsync(hotel);
            return Ok(new ApiResponse<string>("Hotel updated"));
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "provider,admin")]
        public async Task<IActionResult> DeleteHotel(int id)
        {
            await _hotelUseCases.DeleteHotelAsync(id);
            return Ok(new ApiResponse<string>("Hotel deleted"));
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllHotels()
        {
            var hotels = await _hotelUseCases.GetAllHotelsAsync();
            return Ok(new ApiResponse<IEnumerable<HotelDto>>(_mapper.Map<IEnumerable<HotelDto>>(hotels)));
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetHotelById(int id)
        {
            var hotel = await _hotelUseCases.GetHotelByIdAsync(id);
            if (hotel == null) return NotFound(new ApiResponse<string>("Hotel not found"));
            return Ok(new ApiResponse<HotelDto>(_mapper.Map<HotelDto>(hotel)));
        }
    }
}