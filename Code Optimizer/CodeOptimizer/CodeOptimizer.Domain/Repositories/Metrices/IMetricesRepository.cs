using CodeOptimizer.Domain.Entities;
using CodeOptimizer.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace CodeOptimizer.Domain.Repositories.Metrices
{
    public interface IMetricesRepository
    {
        Task<Entities.NumberOfUsersMetric> GetTotalUsers();
        Task<Entities.NumberOfUsersMetric> GetUserCountLastMonth();

        Task<List<Entities.OptimizationCallMetric>?> GetOptimizationCallsByLanguage();
        Task<List<Entities.OptimizationCallTimeMetric>?> GetOptimizationTimeByLanguage();

        Task<List<Entities.UserPerMonthCurrentYear>> GetUsersPerMonth();

        Task AddMetric(AddMetricRequest addMetricRequest);
    }
}
