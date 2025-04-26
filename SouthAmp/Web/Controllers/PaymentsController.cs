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
    public class PaymentsController : ControllerBase
    {
        private readonly IPaymentUseCases _useCases;
        private readonly IMapper _mapper;
        public PaymentsController(IPaymentUseCases useCases, IMapper mapper)
        {
            _useCases = useCases;
            _mapper = mapper;
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreatePayment([FromBody] PaymentDto dto)
        {
            var payment = _mapper.Map<Payment>(dto);
            var result = await _useCases.CreatePaymentAsync(payment);
            return Ok(new ApiResponse<PaymentDto>(_mapper.Map<PaymentDto>(result)));
        }

        [HttpPost("{id}/confirm")]
        [Authorize(Roles = "admin,provider")]
        public async Task<IActionResult> ConfirmPayment(int id)
        {
            await _useCases.ConfirmPaymentAsync(id);
            return Ok(new ApiResponse<string>("Payment confirmed"));
        }

        [HttpPost("{id}/refund")]
        [Authorize(Roles = "admin,provider")]
        public async Task<IActionResult> RefundPayment(int id)
        {
            await _useCases.RefundPaymentAsync(id);
            return Ok(new ApiResponse<string>("Payment refunded"));
        }

        [HttpGet("my")]
        [Authorize]
        public async Task<IActionResult> GetMyPayments()
        {
            var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userIdStr)) return Unauthorized();
            var userId = int.Parse(userIdStr);
            var payments = await _useCases.GetUserPaymentsAsync(userId);
            return Ok(new ApiResponse<IEnumerable<PaymentDto>>(_mapper.Map<IEnumerable<PaymentDto>>(payments)));
        }
    }
}