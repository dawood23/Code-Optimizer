using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace CodeOptimizer.Infrastructure.Exceptions
{
    public class BadRequestException:CustomException
    {
        public BadRequestException(string message):base(message,HttpStatusCode.BadRequest) { }
    }
}
