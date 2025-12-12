using Azure.Core;
using CodeOptimizer.Domain.Models;
using CodeOptimizer.Domain.Repositories.Auth;
using CodeOptimizer.Domain.Services.Telemetry;
using CodeOptimizer.Infrastructure.Exceptions;
using CodeOptimizer.Infrastructure.Security.Hashing;
using CodeOptimizer.Infrastructure.Security.Jwt;

namespace CodeOptimizer.Domain.Services.Auth
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly JwtTokenService _jwtTokenService;
        private readonly PasswordHasher _passwordHasher;
        private readonly IBusinessMetrics _metrics;

        public UserService(IUserRepository userRepository,JwtTokenService jwtTokenService, PasswordHasher passwordHasher, IBusinessMetrics businessMetrics)
        {
            _userRepository = userRepository;
            _jwtTokenService = jwtTokenService;
            _passwordHasher = passwordHasher;
            _metrics = businessMetrics;
        }

        public async Task<AuthResult> AuthenticateUser(User user)
        {
            var dbUser= await _userRepository.GetUserByUsername(user.Username) ?? throw new UnauthorizedException("User Doesnt Exist");
            bool verifyPass =_passwordHasher.VerifyPassword(user.PasswordHash,dbUser!.PasswordHash);

            if (!verifyPass)
            {
                throw new UnauthorizedException("Invalid Password");
            }

            var primaryToken = _jwtTokenService.GenerateToken(user.Username);
            var owinToken = _jwtTokenService.GenerateToken(user.Username, useOwinKey: true);
            var refreshToken = _jwtTokenService.GenerateRefreshToken(user.Username);

            return new AuthResult
            {
                JwtToken= primaryToken,
                OwinToken= owinToken,
                RefreshToken = refreshToken,
            };
        }

        public async Task<AuthResult?> CreateUser(User user)
        {
            var checkUser = await _userRepository.GetUserByUsername(user.Username);
            if (checkUser != null)
            {
                throw new BadRequestException("User Already Exists");
            }
            user.PasswordHash=_passwordHasher.HashPassword(user.PasswordHash);
            int newUser= await _userRepository.CreateUser(user);

            if(newUser>0)
            {
                var primaryToken = _jwtTokenService.GenerateToken(user.Username);
                var owinToken = _jwtTokenService.GenerateToken(user.Username, useOwinKey: true);
                var refreshToken = _jwtTokenService.GenerateRefreshToken(user.Username);

                _metrics.TrackUserSignup();

                return new AuthResult
                {
                    JwtToken = primaryToken,
                    OwinToken = owinToken,
                    RefreshToken = refreshToken,
                };
            }
            return null;
        }

        public AuthResult RefreshToken(string token)
        {
            var principal = _jwtTokenService.ValidateRefreshToken(token);
            if (principal == null)
                throw new BadRequestException("Invalid or expired refresh token");

            string? username = principal.Claims.First(c => c.Type == "username").Value;

            var newAccessToken = _jwtTokenService.GenerateToken(username);
            var newOwinToken=_jwtTokenService.GenerateToken(username, useOwinKey: true);
            var newRefreshToken = _jwtTokenService.GenerateRefreshToken(username);

            return new AuthResult
            {
                JwtToken = newAccessToken,
                OwinToken = newOwinToken,
                RefreshToken = newRefreshToken
            };
        }
    }
}
