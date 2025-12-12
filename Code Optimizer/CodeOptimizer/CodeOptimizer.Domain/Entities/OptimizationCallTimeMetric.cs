using System;
using System.Collections.Generic;
using System.Text;

namespace CodeOptimizer.Domain.Entities
{
    public class OptimizationCallTimeMetric
    {
       public string Language {  get; set; }
        public decimal AvgTimeTaken {  get; set; }
    }
}
