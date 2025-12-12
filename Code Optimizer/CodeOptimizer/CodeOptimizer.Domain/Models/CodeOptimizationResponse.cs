using System;
using System.Collections.Generic;
using System.Text;

namespace CodeOptimizer.Domain.Models
{
    public class CodeOptimizationResponse
    {
        public string OptimizedCode { get; set; }
        public string Description { get; set; }
        public List<string> ErrorsFound { get; set; }
        public List<string> OptimizationsApplied { get; set; }
    }
}
