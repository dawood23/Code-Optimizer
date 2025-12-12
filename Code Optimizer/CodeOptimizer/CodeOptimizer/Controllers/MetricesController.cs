using CodeOptimizer.Domain.Models;
using CodeOptimizer.Domain.Services.Metrices;
using Microsoft.AspNetCore.Mvc;
using CodeOptimizer.Domain;

namespace CodeOptimizer.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MetricesController : ControllerBase
    {
        private readonly IMetricesService _metricesService;

        public MetricesController(IMetricesService metricesService)
        {
            _metricesService = metricesService;
        }

        [HttpPost("add")]
        public async Task<IActionResult> AddMetric([FromBody] AddMetricRequest request)
        {
            if (request == null)
                return BadRequest("Request cannot be null");

            await _metricesService.AddMetric(request);
            return Ok(new { message = "Metric added successfully" });
        }

        [HttpGet("total-users")]
        public async Task<IActionResult> GetTotalUsers()
        {
            var result = await _metricesService.GetTotalUsers();
            return Ok(result);
        }

        [HttpGet("users-last-month")]
        public async Task<IActionResult> GetUserCountLastMonth()
        {
            var result = await _metricesService.GetUserCountLastMonth();
            return Ok(result);
        }

        [HttpGet("calls-by-language")]
        public async Task<IActionResult> GetOptimizationCallsByLanguage()
        {
            var result = await _metricesService.GetOptimizationCallsByLanguage();
            return Ok(result);
        }

        [HttpGet("avg-time-by-language")]
        public async Task<IActionResult> GetOptimizationTimeByLanguage()
        {
            var result = await _metricesService.GetOptimizationTimeByLanguage();
            return Ok(result);
        }

        [HttpGet("user-per-month")]
        public async Task<IActionResult> GetUsersPerMonth()
        {
            var result=await _metricesService.UserPerMonthCurrentYears();
            return Ok(result);
        }
    }
}
