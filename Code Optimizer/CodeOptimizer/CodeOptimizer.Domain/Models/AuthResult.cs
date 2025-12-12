using System;
using System.Collections.Generic;
using System.Text;

namespace CodeOptimizer.Domain.Models
{
    public class AuthResult
    {
        public string JwtToken { get; set; }
        public string OwinToken { get; set; }

        public string RefreshToken { get; set; }
    }
}
