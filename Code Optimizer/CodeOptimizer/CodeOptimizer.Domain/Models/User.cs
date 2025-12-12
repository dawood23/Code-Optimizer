using System;
using System.Collections.Generic;
using System.Text;

namespace CodeOptimizer.Domain.Models
{
    public class User
    {
        public string Username  { get; set; }
        public string PasswordHash { get; set; }
    }
}
