using System;
using System.Collections.Generic;
using System.Text;

namespace CodeOptimizer.Domain.Models
{
    public class CodeOptimizationRequest
    {
        public string Language { get; set; }
        public string Code { get; set; }
    }

}
