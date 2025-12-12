using System;
using System.Collections.Generic;
using System.Text;

namespace CodeOptimizer.Domain.Services.CodeOptimizer.Strategy
{
    public interface ILanguageStrategy
    {
        string GetOptimizationPrompt(string code);
        string Language { get; }
    }
}
