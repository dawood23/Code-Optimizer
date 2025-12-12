using CodeOptimizer.Infrastructure.Database;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace CodeOptimizer.Domain.Repositories.Auth
{
    public class UserRepository : IUserRepository
    {
        private readonly DataBaseConnection _db;

        public UserRepository(DataBaseConnection db)
        {
            _db = db;
        }

        public async Task<int> CreateUser(CodeOptimizer.Domain.Models.User user)
        {
            using var conn = _db.GetConnection();
            conn.Open();

            using var cmd = new SqlCommand("sp_CreateUser", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@Username", user.Username);
            cmd.Parameters.AddWithValue("@PasswordHash", user.PasswordHash);

            int id=await cmd.ExecuteNonQueryAsync();

            return id;
        }

        public async Task<CodeOptimizer.Domain.Entities.User?> GetUserByUsername(string username)
        {
            using var conn = _db.GetConnection();
            conn.Open();

            using var cmd = new SqlCommand("sp_GetUserByUsername", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@Username", username);

            using var reader = await cmd.ExecuteReaderAsync();
            if (!reader.Read()) return null;

            return new CodeOptimizer.Domain.Entities.User
            {
                Username = reader["Username"].ToString()!,
                PasswordHash = reader["PasswordHash"].ToString()!,
                DateCreated = (DateTime)reader["DateCreated"],
            };
        }

    }
}
