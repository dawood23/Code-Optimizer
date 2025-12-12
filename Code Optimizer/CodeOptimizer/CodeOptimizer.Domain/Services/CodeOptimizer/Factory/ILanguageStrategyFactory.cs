using CodeOptimizer.Domain.Services.CodeOptimizer.Strategy;
using System;
using System.Collections.Generic;
using System.Text;

namespace CodeOptimizer.Domain.Services.CodeOptimizer.Factory
{
    public interface ILanguageStrategyFactory
    {
        ILanguageStrategy GetStrategy(string language);
    }
}
