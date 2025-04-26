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
    public class ReviewsController : ControllerBase
    {
        private readonly IReviewUseCases _useCases;
        private readonly IMapper _mapper;
        public ReviewsController(IReviewUseCases useCases, IMapper mapper)
        {
            _useCases = useCases;
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
            var result = await _useCases.AddReviewAsync(review);
            return Ok(new ApiResponse<ReviewDto>(_mapper.Map<ReviewDto>(result)));
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> UpdateReview(int id, [FromBody] ReviewDto dto)
        {
            var userIdStr2 = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userIdStr2)) return Unauthorized();
            var review = _mapper.Map<Review>(dto);
            review.Id = id;
            review.UserId = int.Parse(userIdStr2);
            await _useCases.UpdateReviewAsync(review);
            return Ok(new ApiResponse<string>("Review updated"));
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteReview(int id)
        {
            await _useCases.DeleteReviewAsync(id);
            return Ok(new ApiResponse<string>("Review deleted"));
        }

        [HttpGet("hotel/{hotelId}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetReviewsByHotelId(int hotelId)
        {
            var reviews = await _useCases.GetReviewsByHotelIdAsync(hotelId);
            return Ok(new ApiResponse<IEnumerable<ReviewDto>>(_mapper.Map<IEnumerable<ReviewDto>>(reviews)));
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetReviewById(int id)
        {
            var review = await _useCases.GetReviewByIdAsync(id);
            if (review == null) return NotFound(new ApiResponse<string>("Review not found"));
            return Ok(new ApiResponse<ReviewDto>(_mapper.Map<ReviewDto>(review)));
        }

        [HttpGet("hotel/{hotelId}/average")]
        [AllowAnonymous]
        public async Task<IActionResult> GetAverageRating(int hotelId)
        {
            var avg = await _useCases.GetAverageRatingAsync(hotelId);
            return Ok(new ApiResponse<double>(avg));
        }
    }
}