using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace CodeOptimizer.Infrastructure.Exceptions
{
    public class UnauthorizedException:CustomException
    {
        public UnauthorizedException(string message) :base(message,HttpStatusCode.Unauthorized) { }
    }
}
