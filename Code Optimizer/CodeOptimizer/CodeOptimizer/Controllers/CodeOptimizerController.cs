using CodeOptimizer.Infrastructure.Attributes;
using CodeOptimizer.Domain.Models;
using CodeOptimizer.Domain.Services.CodeOptimizer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CodeOptimizer.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class CodeOptimizerController : ControllerBase
    {
        private readonly ICodeOptimizationService _optimizationService;

        public CodeOptimizerController(ICodeOptimizationService optimizationService)
        {
            _optimizationService = optimizationService;
        }

        [RateLimit(5,PerSeconds =300)]
        [HttpPost("optimize")]
        public async Task<ActionResult<CodeOptimizationResponse>> OptimizeCode(
            [FromBody] CodeOptimizationRequest request)
        {
            try
            {
                var result = await _optimizationService.OptimizeCodeAsync(request);
                return Ok(result);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "An error occurred during optimization", details = ex.Message });
            }
        }

        [HttpGet("supported-languages")]
        public ActionResult<IEnumerable<string>> GetSupportedLanguages()
        {
            return Ok(new[] { "javascript", "typescript", "csharp", "sql", "html" });
        }
    }
}
