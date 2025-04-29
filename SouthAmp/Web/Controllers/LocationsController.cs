using Microsoft.AspNetCore.Mvc;
using SouthAmp.Application.Interfaces;
using SouthAmp.Application.DTOs;
using SouthAmp.Core.Entities;
using Microsoft.AspNetCore.Authorization;
using AutoMapper;
using SouthAmp.Web.Models;

namespace SouthAmp.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LocationsController(ILocationUseCases useCases, IMapper mapper) : ControllerBase
    {
        [HttpPost]
        [Authorize(Roles = "admin,provider")]
        public async Task<IActionResult> AddLocation([FromBody] LocationDto dto)
        {
            var location = mapper.Map<Location>(dto);
            var result = await useCases.AddLocationAsync(location);
            return Ok(new ApiResponse<LocationDto>(mapper.Map<LocationDto>(result)));
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllLocations()
        {
            var locations = await useCases.GetAllLocationsAsync();
            return Ok(new ApiResponse<IEnumerable<LocationDto>>(mapper.Map<IEnumerable<LocationDto>>(locations)));
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetLocationById(int id)
        {
            var location = await useCases.GetLocationByIdAsync(id);
            return Ok(new ApiResponse<LocationDto>(mapper.Map<LocationDto>(location)));
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "admin,provider")]
        public async Task<IActionResult> UpdateLocation(int id, [FromBody] LocationDto dto)
        {
            var location = mapper.Map<Location>(dto);
            location.Id = id;
            await useCases.UpdateLocationAsync(location);
            return Ok(new ApiResponse<string>("Location updated"));
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "admin,provider")]
        public async Task<IActionResult> DeleteLocation(int id)
        {
            await useCases.DeleteLocationAsync(id);
            return Ok(new ApiResponse<string>("Location deleted"));
        }
    }
}