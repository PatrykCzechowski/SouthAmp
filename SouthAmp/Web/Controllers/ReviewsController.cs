using Microsoft.AspNetCore.Mvc;
using SouthAmp.Application.UseCases;
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
    public class ReviewsController : ControllerBase
    {
        private readonly ReviewUseCases _reviewUseCases;
        private readonly IMapper _mapper;
        public ReviewsController(ReviewUseCases reviewUseCases, IMapper mapper)
        {
            _reviewUseCases = reviewUseCases;
            _mapper = mapper;
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddReview([FromBody] ReviewDto dto)
        {
            var review = _mapper.Map<Review>(dto);
            var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userIdStr)) return Unauthorized();
            review.UserId = int.Parse(userIdStr);
            var result = await _reviewUseCases.AddReviewAsync(review);
            return Ok(new ApiResponse<ReviewDto>(_mapper.Map<ReviewDto>(result)));
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> UpdateReview(int id, [FromBody] ReviewDto dto)
        {
            var review = _mapper.Map<Review>(dto);
            review.Id = id;
            var userIdStr2 = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userIdStr2)) return Unauthorized();
            review.UserId = int.Parse(userIdStr2);
            await _reviewUseCases.UpdateReviewAsync(review);
            return Ok(new ApiResponse<string>("Review updated"));
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteReview(int id)
        {
            await _reviewUseCases.DeleteReviewAsync(id);
            return Ok(new ApiResponse<string>("Review deleted"));
        }

        [HttpGet("hotel/{hotelId}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetReviewsByHotelId(int hotelId)
        {
            var reviews = await _reviewUseCases.GetReviewsByHotelIdAsync(hotelId);
            return Ok(new ApiResponse<IEnumerable<ReviewDto>>(_mapper.Map<IEnumerable<ReviewDto>>(reviews)));
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetReviewById(int id)
        {
            var review = await _reviewUseCases.GetReviewByIdAsync(id);
            if (review == null) return NotFound(new ApiResponse<string>("Review not found"));
            return Ok(new ApiResponse<ReviewDto>(_mapper.Map<ReviewDto>(review)));
        }

        [HttpGet("hotel/{hotelId}/average")]
        [AllowAnonymous]
        public async Task<IActionResult> GetAverageRating(int hotelId)
        {
            var avg = await _reviewUseCases.GetAverageRatingAsync(hotelId);
            return Ok(new ApiResponse<double>(avg));
        }
    }
}