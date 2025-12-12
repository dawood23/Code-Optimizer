using System;
using System.Collections.Generic;
using System.Text;

namespace CodeOptimizer.Domain.Models
{
    public class AddMetricRequest
    {
        public string MetricName { get; set; }
        public string Language {  get; set; }

        public decimal TimeTaken { get; set; }
    }
}
