using System;
using System.Collections.Generic;
using System.Text;

namespace CodeOptimizer.Domain.Models
{
    public class OptimizationCallTimeMetric
    {
        public string Language { get; set; }
        public decimal AvgTimeTaken { get; set; }
    }
}
