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
    public class LocationsController : ControllerBase
    {
        private readonly ILocationUseCases _useCases;
        private readonly IMapper _mapper;
        public LocationsController(ILocationUseCases useCases, IMapper mapper)
        {
            _useCases = useCases;
            _mapper = mapper;
        }

        [HttpPost]
        [Authorize(Roles = "admin,provider")]
        public async Task<IActionResult> AddLocation([FromBody] LocationDto dto)
        {
            var location = _mapper.Map<Location>(dto);
            var result = await _useCases.AddLocationAsync(location);
            return Ok(new ApiResponse<LocationDto>(_mapper.Map<LocationDto>(result)));
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllLocations()
        {
            var locations = await _useCases.GetAllLocationsAsync();
            return Ok(new ApiResponse<IEnumerable<LocationDto>>(_mapper.Map<IEnumerable<LocationDto>>(locations)));
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetLocationById(int id)
        {
            var location = await _useCases.GetLocationByIdAsync(id);
            if (location == null) return NotFound(new ApiResponse<string>("Location not found"));
            return Ok(new ApiResponse<LocationDto>(_mapper.Map<LocationDto>(location)));
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "admin,provider")]
        public async Task<IActionResult> UpdateLocation(int id, [FromBody] LocationDto dto)
        {
            var location = _mapper.Map<Location>(dto);
            location.Id = id;
            await _useCases.UpdateLocationAsync(location);
            return Ok(new ApiResponse<string>("Location updated"));
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "admin,provider")]
        public async Task<IActionResult> DeleteLocation(int id)
        {
            await _useCases.DeleteLocationAsync(id);
            return Ok(new ApiResponse<string>("Location deleted"));
        }
    }
}