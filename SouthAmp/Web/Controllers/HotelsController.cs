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
    public class HotelsController(HotelUseCases hotelUseCases, IMapper mapper) : ControllerBase
    {
        [HttpPost]
        [Authorize(Roles = "provider,admin")]
        public async Task<IActionResult> AddHotel([FromBody] HotelDto dto)
        {
            var hotel = mapper.Map<Hotel>(dto);
            var result = await hotelUseCases.AddHotelAsync(hotel);
            return Ok(new ApiResponse<HotelDto>(mapper.Map<HotelDto>(result)));
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "provider,admin")]
        public async Task<IActionResult> UpdateHotel(int id, [FromBody] HotelDto dto)
        {
            var hotel = mapper.Map<Hotel>(dto);
            hotel.Id = id;
            await hotelUseCases.UpdateHotelAsync(hotel);
            return Ok(new ApiResponse<string>("Hotel updated"));
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "provider,admin")]
        public async Task<IActionResult> DeleteHotel(int id)
        {
            await hotelUseCases.DeleteHotelAsync(id);
            return Ok(new ApiResponse<string>("Hotel deleted"));
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllHotels()
        {
            var hotels = await hotelUseCases.GetAllHotelsAsync();
            return Ok(new ApiResponse<IEnumerable<HotelDto>>(mapper.Map<IEnumerable<HotelDto>>(hotels)));
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetHotelById(int id)
        {
            var hotel = await hotelUseCases.GetHotelByIdAsync(id);
            return Ok(new ApiResponse<HotelDto>(mapper.Map<HotelDto>(hotel)));
        }
    }
}