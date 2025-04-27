using SouthAmp.Application.Interfaces;
using SouthAmp.Application.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using SouthAmp.Web.Models;

namespace SouthAmp.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "admin")]
    public class AdminController(IAdminUseCases useCases, IMapper mapper) : ControllerBase
    {
        [HttpGet("users")]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await useCases.GetAllUsersAsync();
            return Ok(new ApiResponse<IEnumerable<UserDto>>(mapper.Map<IEnumerable<UserDto>>(users)));
        }

        [HttpPost("users/{id}/ban")]
        public async Task<IActionResult> BanUser(int id)
        {
            await useCases.BanUserAsync(id);
            return Ok(new ApiResponse<string>("User banned"));
        }

        [HttpPost("users/{id}/activate")]
        public async Task<IActionResult> ActivateUser(int id)
        {
            await useCases.ActivateUserAsync(id);
            return Ok(new ApiResponse<string>("User activated"));
        }

        [HttpDelete("users/{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            await useCases.DeleteUserAsync(id);
            return Ok(new ApiResponse<string>("User deleted"));
        }

        [HttpGet("hotels")]
        public async Task<IActionResult> GetAllHotels()
        {
            var hotels = await useCases.GetAllHotelsAsync();
            return Ok(new ApiResponse<IEnumerable<HotelDto>>(mapper.Map<IEnumerable<HotelDto>>(hotels)));
        }

        [HttpPost("hotels/{id}/moderate")]
        public async Task<IActionResult> ModerateHotel(int id, [FromQuery] bool isActive)
        {
            await useCases.ModerateHotelAsync(id, isActive);
            return Ok(new ApiResponse<string>("Hotel moderated"));
        }

        [HttpGet("reviews")]
        public async Task<IActionResult> GetAllReviews()
        {
            var reviews = await useCases.GetAllReviewsAsync();
            return Ok(new ApiResponse<IEnumerable<ReviewDto>>(mapper.Map<IEnumerable<ReviewDto>>(reviews)));
        }

        [HttpPost("reviews/{id}/moderate")]
        public async Task<IActionResult> ModerateReview(int id, [FromQuery] bool isReported)
        {
            await useCases.ModerateReviewAsync(id, isReported);
            return Ok(new ApiResponse<string>("Review moderated"));
        }

        [HttpGet("payments")]
        public async Task<IActionResult> GetAllPayments()
        {
            var payments = await useCases.GetAllPaymentsAsync();
            return Ok(new ApiResponse<IEnumerable<PaymentDto>>(mapper.Map<IEnumerable<PaymentDto>>(payments)));
        }
    }
}