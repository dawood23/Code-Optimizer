using System;
using System.Collections.Generic;
using System.Text;
using CodeOptimizer.Domain.Models;

namespace CodeOptimizer.Domain.Repositories.Auth
{
    public interface IUserRepository
    {
        Task<int> CreateUser(CodeOptimizer.Domain.Models.User user);
        Task<CodeOptimizer.Domain.Entities.User?> GetUserByUsername(string username);
    }
}
