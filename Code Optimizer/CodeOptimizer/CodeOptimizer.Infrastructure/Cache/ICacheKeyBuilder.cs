using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace CodeOptimizer.Infrastructure.Cache
{
    public interface ICacheKeyBuilder
    {
        string? BuildCacheKey(HttpContext context);
    }
}
