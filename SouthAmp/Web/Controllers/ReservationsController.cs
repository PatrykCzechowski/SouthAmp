using SouthAmp.Application.UseCases;
using SouthAmp.Application.DTOs;
using SouthAmp.Core.Entities;
using Microsoft.AspNetCore.Authorization;
using AutoMapper;
using SouthAmp.Web.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;

namespace SouthAmp.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReservationsController : ControllerBase
    {
        private readonly ReservationUseCases _reservationUseCases;
        private readonly IMapper _mapper;
        public ReservationsController(ReservationUseCases reservationUseCases, IMapper mapper)
        {
            _reservationUseCases = reservationUseCases;
            _mapper = mapper;
        }

        [HttpPost]
        [Authorize(Roles = "guest,provider,admin")]
        public async Task<IActionResult> CreateReservation([FromBody] ReservationDto dto)
        {
            var reservation = _mapper.Map<Reservation>(dto);
            var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userIdStr)) return Unauthorized();
            reservation.UserId = int.Parse(userIdStr);
            var result = await _reservationUseCases.CreateReservationAsync(reservation);
            return Ok(new ApiResponse<ReservationDto>(_mapper.Map<ReservationDto>(result)));
        }

        [HttpGet("my")]
        [Authorize]
        public async Task<IActionResult> GetMyReservations()
        {
            var userIdStr2 = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userIdStr2)) return Unauthorized();
            var userId = int.Parse(userIdStr2);
            var reservations = await _reservationUseCases.GetUserReservationsAsync(userId);
            return Ok(new ApiResponse<IEnumerable<ReservationDto>>(_mapper.Map<IEnumerable<ReservationDto>>(reservations)));
        }

        [HttpPost("check-availability")]
        [AllowAnonymous]
        public async Task<IActionResult> CheckAvailability([FromBody] CheckAvailabilityRequest req)
        {
            var available = await _reservationUseCases.CheckAvailabilityAsync(req.RoomId, req.StartDate, req.EndDate);
            return Ok(new ApiResponse<bool>(available));
        }

        [HttpPost("{id}/cancel")]
        [Authorize]
        public async Task<IActionResult> CancelReservation(int id)
        {
            await _reservationUseCases.CancelReservationAsync(id);
            return Ok(new ApiResponse<string>("Reservation cancelled"));
        }

        [HttpPost("{id}/change-date")]
        [Authorize]
        public async Task<IActionResult> ChangeReservationDate(int id, [FromBody] ChangeDateRequest req)
        {
            await _reservationUseCases.ChangeReservationDateAsync(id, req.NewStart, req.NewEnd);
            return Ok(new ApiResponse<string>("Reservation date changed"));
        }

        public class CheckAvailabilityRequest
        {
            public int RoomId { get; set; }
            public DateTime StartDate { get; set; }
            public DateTime EndDate { get; set; }
        }
        public class ChangeDateRequest
        {
            public DateTime NewStart { get; set; }
            public DateTime NewEnd { get; set; }
        }
    }
}