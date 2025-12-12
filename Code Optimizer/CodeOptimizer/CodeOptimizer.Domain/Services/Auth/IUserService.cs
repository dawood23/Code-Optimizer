using CodeOptimizer.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace CodeOptimizer.Domain.Services.Auth
{
    public interface IUserService
    {
        Task<AuthResult?> CreateUser(Models.User user);

        Task<AuthResult> AuthenticateUser(Models.User user);

        AuthResult RefreshToken(string token);
    }
}
