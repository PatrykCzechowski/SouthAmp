using Microsoft.AspNetCore.Mvc;
using SouthAmp.Application.Interfaces;
using SouthAmp.Application.UseCases;
using SouthAmp.Application.DTOs;
using SouthAmp.Core.Entities;
using Microsoft.AspNetCore.Authorization;
using AutoMapper;
using SouthAmp.Web.Models;

namespace SouthAmp.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DiscountCodesController : ControllerBase
    {
        private readonly IDiscountCodeUseCases _useCases;
        private readonly IMapper _mapper;
        public DiscountCodesController(IDiscountCodeUseCases useCases, IMapper mapper)
        {
            _useCases = useCases;
            _mapper = mapper;
        }

        [HttpPost]
        [Authorize(Roles = "provider,admin")]
        public async Task<IActionResult> CreateDiscountCode([FromBody] DiscountCodeDto dto)
        {
            var code = _mapper.Map<DiscountCode>(dto);
            var result = await _useCases.CreateDiscountCodeAsync(code);
            return Ok(new ApiResponse<DiscountCodeDto>(_mapper.Map<DiscountCodeDto>(result)));
        }

        [HttpGet("verify/{code}")]
        [AllowAnonymous]
        public async Task<IActionResult> VerifyCode(string code)
        {
            var discount = await _useCases.GetByCodeAsync(code);
            if (discount == null) return NotFound(new ApiResponse<string>("Code not found"));
            return Ok(new ApiResponse<DiscountCodeDto>(_mapper.Map<DiscountCodeDto>(discount)));
        }

        [HttpPost("use/{code}")]
        [Authorize]
        public async Task<IActionResult> UseCode(string code)
        {
            await _useCases.UseCodeAsync(code);
            return Ok(new ApiResponse<string>("Code used"));
        }
    }
}