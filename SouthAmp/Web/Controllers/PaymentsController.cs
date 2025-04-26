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
    public class PaymentsController : ControllerBase
    {
        private readonly PaymentUseCases _paymentUseCases;
        private readonly IMapper _mapper;
        public PaymentsController(PaymentUseCases paymentUseCases, IMapper mapper)
        {
            _paymentUseCases = paymentUseCases;
            _mapper = mapper;
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreatePayment([FromBody] PaymentDto dto)
        {
            var payment = _mapper.Map<Payment>(dto);
            var result = await _paymentUseCases.CreatePaymentAsync(payment);
            return Ok(new ApiResponse<PaymentDto>(_mapper.Map<PaymentDto>(result)));
        }

        [HttpPost("{id}/confirm")]
        [Authorize(Roles = "admin,provider")]
        public async Task<IActionResult> ConfirmPayment(int id)
        {
            await _paymentUseCases.ConfirmPaymentAsync(id);
            return Ok(new ApiResponse<string>("Payment confirmed"));
        }

        [HttpPost("{id}/refund")]
        [Authorize(Roles = "admin,provider")]
        public async Task<IActionResult> RefundPayment(int id)
        {
            await _paymentUseCases.RefundPaymentAsync(id);
            return Ok(new ApiResponse<string>("Payment refunded"));
        }

        [HttpGet("my")]
        [Authorize]
        public async Task<IActionResult> GetMyPayments()
        {
            var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userIdStr)) return Unauthorized();
            var userId = int.Parse(userIdStr);
            var payments = await _paymentUseCases.GetUserPaymentsAsync(userId);
            return Ok(new ApiResponse<IEnumerable<PaymentDto>>(_mapper.Map<IEnumerable<PaymentDto>>(payments)));
        }
    }
}