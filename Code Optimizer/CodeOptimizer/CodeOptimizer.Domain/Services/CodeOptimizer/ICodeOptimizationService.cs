using CodeOptimizer.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace CodeOptimizer.Domain.Services.CodeOptimizer
{
    public interface ICodeOptimizationService
    {
        Task<CodeOptimizationResponse> OptimizeCodeAsync(CodeOptimizationRequest request);
    }
}
