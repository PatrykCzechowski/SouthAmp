using Microsoft.AspNetCore.Mvc;
using SouthAmp.Application.Interfaces;
using SouthAmp.Application.DTOs;
using SouthAmp.Core.Entities;
using Microsoft.AspNetCore.Authorization;
using AutoMapper;
using SouthAmp.Web.Models;
using System.Security.Claims;

namespace SouthAmp.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReviewsController(IReviewUseCases useCases, IMapper mapper) : ControllerBase
    {
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddReview([FromBody] ReviewDto dto)
        {
            var review = mapper.Map<Review>(dto);
            var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userIdStr)) return Unauthorized();
            review.UserId = int.Parse(userIdStr);
            var result = await useCases.AddReviewAsync(review);
            return Ok(new ApiResponse<ReviewDto>(mapper.Map<ReviewDto>(result)));
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> UpdateReview(int id, [FromBody] ReviewDto dto)
        {
            var userIdStr2 = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userIdStr2)) return Unauthorized();
            var review = mapper.Map<Review>(dto);
            review.Id = id;
            review.UserId = int.Parse(userIdStr2);
            await useCases.UpdateReviewAsync(review);
            return Ok(new ApiResponse<string>("Review updated"));
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteReview(int id)
        {
            await useCases.DeleteReviewAsync(id);
            return Ok(new ApiResponse<string>("Review deleted"));
        }

        [HttpGet("hotel/{hotelId}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetReviewsByHotelId(int hotelId)
        {
            var reviews = await useCases.GetReviewsByHotelIdAsync(hotelId);
            return Ok(new ApiResponse<IEnumerable<ReviewDto>>(mapper.Map<IEnumerable<ReviewDto>>(reviews)));
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetReviewById(int id)
        {
            var review = await useCases.GetReviewByIdAsync(id);
            return Ok(new ApiResponse<ReviewDto>(mapper.Map<ReviewDto>(review)));
        }

        [HttpGet("hotel/{hotelId}/average")]
        [AllowAnonymous]
        public async Task<IActionResult> GetAverageRating(int hotelId)
        {
            var avg = await useCases.GetAverageRatingAsync(hotelId);
            return Ok(new ApiResponse<double>(avg));
        }
    }
}