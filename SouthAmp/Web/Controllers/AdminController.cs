using SouthAmp.Application.UseCases;
using SouthAmp.Application.DTOs;
using SouthAmp.Core.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using SouthAmp.Web.Models;

namespace SouthAmp.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "admin")]
    public class AdminController : ControllerBase
    {
        private readonly AdminUseCases _adminUseCases;
        private readonly IMapper _mapper;
        public AdminController(AdminUseCases adminUseCases, IMapper mapper)
        {
            _adminUseCases = adminUseCases;
            _mapper = mapper;
        }

        [HttpGet("users")]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _adminUseCases.GetAllUsersAsync();
            return Ok(new ApiResponse<IEnumerable<UserDto>>(_mapper.Map<IEnumerable<UserDto>>(users)));
        }

        [HttpPost("users/{id}/ban")]
        public async Task<IActionResult> BanUser(int id)
        {
            await _adminUseCases.BanUserAsync(id);
            return Ok(new ApiResponse<string>("User banned"));
        }

        [HttpPost("users/{id}/activate")]
        public async Task<IActionResult> ActivateUser(int id)
        {
            await _adminUseCases.ActivateUserAsync(id);
            return Ok(new ApiResponse<string>("User activated"));
        }

        [HttpDelete("users/{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            await _adminUseCases.DeleteUserAsync(id);
            return Ok(new ApiResponse<string>("User deleted"));
        }

        [HttpGet("hotels")]
        public async Task<IActionResult> GetAllHotels()
        {
            var hotels = await _adminUseCases.GetAllHotelsAsync();
            return Ok(new ApiResponse<IEnumerable<HotelDto>>(_mapper.Map<IEnumerable<HotelDto>>(hotels)));
        }

        [HttpPost("hotels/{id}/moderate")]
        public async Task<IActionResult> ModerateHotel(int id, [FromQuery] bool isActive)
        {
            await _adminUseCases.ModerateHotelAsync(id, isActive);
            return Ok(new ApiResponse<string>("Hotel moderated"));
        }

        [HttpGet("reviews")]
        public async Task<IActionResult> GetAllReviews()
        {
            var reviews = await _adminUseCases.GetAllReviewsAsync();
            return Ok(new ApiResponse<IEnumerable<ReviewDto>>(_mapper.Map<IEnumerable<ReviewDto>>(reviews)));
        }

        [HttpPost("reviews/{id}/moderate")]
        public async Task<IActionResult> ModerateReview(int id, [FromQuery] bool isReported)
        {
            await _adminUseCases.ModerateReviewAsync(id, isReported);
            return Ok(new ApiResponse<string>("Review moderated"));
        }

        [HttpGet("payments")]
        public async Task<IActionResult> GetAllPayments()
        {
            var payments = await _adminUseCases.GetAllPaymentsAsync();
            return Ok(new ApiResponse<IEnumerable<PaymentDto>>(_mapper.Map<IEnumerable<PaymentDto>>(payments)));
        }
    }
}