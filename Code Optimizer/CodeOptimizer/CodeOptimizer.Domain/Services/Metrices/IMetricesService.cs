using CodeOptimizer.Domain.Entities;
using CodeOptimizer.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace CodeOptimizer.Domain.Services.Metrices
{
    public interface IMetricesService
    {
            Task<Models.NumberOfUsersMetric> GetTotalUsers();
            Task<Models.NumberOfUsersMetric> GetUserCountLastMonth();

            Task<List<Models.OptimizationCallMetric>?> GetOptimizationCallsByLanguage();
            Task<List<Models.OptimizationCallTimeMetric>?> GetOptimizationTimeByLanguage();
            
            Task<List<Models.UserPerMonthCurrentYear>> UserPerMonthCurrentYears();
            Task AddMetric(AddMetricRequest addMetricRequest);
        
    }
}
