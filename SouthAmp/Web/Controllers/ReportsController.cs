using Microsoft.AspNetCore.Mvc;
using SouthAmp.Application.Interfaces;
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
    public class ReportsController : ControllerBase
    {
        private readonly IReportUseCases _useCases;
        private readonly IMapper _mapper;
        public ReportsController(IReportUseCases useCases, IMapper mapper)
        {
            _useCases = useCases;
            _mapper = mapper;
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddReport([FromBody] ReportDto dto)
        {
            var report = _mapper.Map<Report>(dto);
            var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userIdStr)) return Unauthorized();
            report.UserId = int.Parse(userIdStr);
            var result = await _useCases.AddReportAsync(report);
            return Ok(new ApiResponse<ReportDto>(_mapper.Map<ReportDto>(result)));
        }

        [HttpGet("my")]
        [Authorize]
        public async Task<IActionResult> GetMyReports()
        {
            var userIdStr2 = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userIdStr2)) return Unauthorized();
            var userId = int.Parse(userIdStr2);
            var reports = await _useCases.GetUserReportsAsync(userId);
            return Ok(new ApiResponse<IEnumerable<ReportDto>>(_mapper.Map<IEnumerable<ReportDto>>(reports)));
        }

        [HttpGet]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> GetAllReports()
        {
            var reports = await _useCases.GetAllReportsAsync();
            return Ok(new ApiResponse<IEnumerable<ReportDto>>(_mapper.Map<IEnumerable<ReportDto>>(reports)));
        }

        [HttpPost("{id}/respond")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> RespondToReport(int id, [FromBody] RespondRequest req)
        {
            if (req.Response == null)
                return BadRequest(new ApiResponse<string>("Response cannot be null"));
            await _useCases.RespondToReportAsync(id, req.Response);
            return Ok(new ApiResponse<string>("Report responded"));
        }

        public class RespondRequest
        {
            public string? Response { get; set; }
        }
    }
}