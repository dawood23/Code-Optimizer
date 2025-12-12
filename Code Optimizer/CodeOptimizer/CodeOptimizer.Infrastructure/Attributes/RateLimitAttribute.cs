using System;
using System.Collections.Generic;
using System.Text;

namespace CodeOptimizer.Infrastructure.Attributes
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
    public class RateLimitAttribute : Attribute
    {
        public int MaxRequests { get; }
        public int PerSeconds { get; set; }

        public RateLimitAttribute(int maxRequests)
        {
            MaxRequests = maxRequests;
            PerSeconds = 60;
        }
    }

}
