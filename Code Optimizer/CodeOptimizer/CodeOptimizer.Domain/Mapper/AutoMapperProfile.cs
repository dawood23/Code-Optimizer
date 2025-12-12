using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace CodeOptimizer.Domain.Mapper
{
    public class AutoMapperProfile:Profile
    {
       public AutoMapperProfile()
        {
            CreateMap<Entities.NumberOfUsersMetric, Models.NumberOfUsersMetric>();
            CreateMap<Entities.OptimizationCallMetric, Models.OptimizationCallMetric>();
            CreateMap<Entities.OptimizationCallTimeMetric,Models.OptimizationCallTimeMetric>();
            CreateMap<Entities.UserPerMonthCurrentYear,Models.UserPerMonthCurrentYear>();
        }
    }
}
