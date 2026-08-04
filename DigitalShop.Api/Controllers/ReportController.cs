using DigitalShop.Application.DTOs.Report;
using DigitalShop.Application.Services.Interface;
using DigitalShop.Application.Services.Interface.Report;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DigitalShop.Controllers
{
    [Authorize(Roles = "Admin")] // Only admin can view the report
    [Route("api/[controller]")]
    [ApiController]
    public class ReportController : ControllerBase
    {
        private readonly IReportService _reportService;

        public ReportController(IReportService reportService)
        {
            _reportService = reportService;
        }

        [HttpGet]
        public async Task<ActionResult<ReportResponseDTO>> GetReport()
        {
            var report = await _reportService.GetReportApp();
            return Ok(report);
        }
    }
}