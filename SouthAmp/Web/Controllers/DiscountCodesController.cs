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
    public class DiscountCodesController(IDiscountCodeUseCases useCases, IMapper mapper) : ControllerBase
    {
        [HttpPost]
        [Authorize(Roles = "provider,admin")]
        public async Task<IActionResult> CreateDiscountCode([FromBody] DiscountCodeDto dto)
        {
            var code = mapper.Map<DiscountCode>(dto);
            var result = await useCases.CreateDiscountCodeAsync(code);
            return Ok(new ApiResponse<DiscountCodeDto>(mapper.Map<DiscountCodeDto>(result)));
        }

        [HttpGet("verify/{code}")]
        [AllowAnonymous]
        public async Task<IActionResult> VerifyCode(string code)
        {
            var discount = await useCases.GetByCodeAsync(code);
            return Ok(new ApiResponse<DiscountCodeDto>(mapper.Map<DiscountCodeDto>(discount)));
        }

        [HttpPost("use/{code}")]
        [Authorize]
        public async Task<IActionResult> UseCode(string code)
        {
            await useCases.UseCodeAsync(code);
            return Ok(new ApiResponse<string>("Code used"));
        }
    }
}