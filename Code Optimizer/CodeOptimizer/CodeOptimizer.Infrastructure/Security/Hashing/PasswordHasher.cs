using Microsoft.Extensions.Configuration;
using System.Security.Cryptography;
using System.Text;

namespace CodeOptimizer.Infrastructure.Security.Hashing
{
    public class PasswordHasher
    {
        private readonly string key;

        public PasswordHasher(IConfiguration configuration)
        {
            key = configuration["HashKey"];
        }

        public string HashPassword(string password)
        {
            using var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(key));
            var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(hash);
        }

        public bool VerifyPassword(string password, string storedHash)
        {
            var computedHash = HashPassword(password);
            return computedHash == storedHash;
        }
    }

}
