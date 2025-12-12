using AutoMapper;
using CodeOptimizer.Domain.Entities;
using CodeOptimizer.Domain.Models;
using CodeOptimizer.Domain.Repositories.Metrices;
using System;
using System.Collections.Generic;
using System.Text;

namespace CodeOptimizer.Domain.Services.Metrices
{
    public class MetricesService : IMetricesService
    {
        private readonly IMetricesRepository _metricesRepository;
        private readonly IMapper _mapper;

        public MetricesService(IMetricesRepository metricesRepository, IMapper mapper)
        {
            _metricesRepository = metricesRepository;
            _mapper = mapper;
        }

        public async Task AddMetric(AddMetricRequest addMetricRequest)
            => await _metricesRepository.AddMetric(addMetricRequest);

        public async Task<List<Models.OptimizationCallMetric>?> GetOptimizationCallsByLanguage()
        {
            var data = await _metricesRepository.GetOptimizationCallsByLanguage();
            return _mapper.Map<List<Models.OptimizationCallMetric>>(data);
        }

        public async Task<List<Models.OptimizationCallTimeMetric>?> GetOptimizationTimeByLanguage()
        {
            var data = await _metricesRepository.GetOptimizationTimeByLanguage();
            return _mapper.Map<List<Models.OptimizationCallTimeMetric>>(data);
        }

        public async Task<Models.NumberOfUsersMetric> GetTotalUsers()
        {
            return _mapper.Map<Models.NumberOfUsersMetric>(
                await _metricesRepository.GetTotalUsers()
            );
        }

        public async Task<Models.NumberOfUsersMetric> GetUserCountLastMonth()
        {
            return _mapper.Map<Models.NumberOfUsersMetric>(
                await _metricesRepository.GetUserCountLastMonth()
            );
        }

        public async Task<List<Models.UserPerMonthCurrentYear>> UserPerMonthCurrentYears()
        {
            var data = await _metricesRepository.GetUsersPerMonth();
            return _mapper.Map<List<Models.UserPerMonthCurrentYear>>(data);
        }
    }
}
