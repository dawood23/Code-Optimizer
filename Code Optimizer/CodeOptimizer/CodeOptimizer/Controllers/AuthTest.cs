using CodeOptimizer.Domain.Services.Groq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CodeOptimizer.API.Controllers
{
    [Route("test")]
    public class AuthTest : ControllerBase
    {
        private readonly IGroqService _service;
        public AuthTest(IGroqService service)
        {
            _service = service;
        }


        [HttpGet("Groq")]
        public async Task<IActionResult> Get()
        {
            var response = await _service.GenerateHelloWorld();

            return Ok(response);
        }

        [HttpPost("Groq/prompt")]
        public async Task<IActionResult> GetByPrompt([FromBody] GroqRequest req)
        {
            var response = await _service.GenerateResponseByPrompt(req.Prompt);

            return Ok(response);
        }

        [Authorize]
        [HttpGet]
       public IActionResult Test()
        {
            return Ok("Authorized");
        }
    }

    public class GroqRequest
    {
       public string Prompt { get; set; }
    }
}
