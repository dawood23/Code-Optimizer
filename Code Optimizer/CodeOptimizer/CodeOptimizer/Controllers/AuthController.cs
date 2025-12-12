using CodeOptimizer.Domain.Models;
using CodeOptimizer.Domain.Services.Auth;
using CodeOptimizer.Infrastructure.Security.Jwt;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace CodeOptimizer.API.Controllers
{
    [ApiController]
    [Route("auth")]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;

        public AuthController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] User user) 
        {
            var tokens =await _userService.AuthenticateUser(user);

            return Ok(tokens);
        }

        [HttpPost]
        [Route("signup")]
        public async Task<IActionResult> CreateUser([FromBody] User user)
        {
            var tokens=await _userService.CreateUser(user);

            return Ok(tokens);
        }

        [HttpPost("refresh-token")]
        public async Task<IActionResult> Refresh([FromBody] RefreshRequest request)
        {
            var tokens = _userService.RefreshToken(request.RefreshToken);

            return Ok(tokens);
        }

    }
}
