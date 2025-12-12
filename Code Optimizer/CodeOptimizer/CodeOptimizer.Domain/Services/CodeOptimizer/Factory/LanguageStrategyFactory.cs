using CodeOptimizer.Domain.Services.CodeOptimizer.Strategy;
using CodeOptimizer.Infrastructure.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace CodeOptimizer.Domain.Services.CodeOptimizer.Factory
{
    public class LanguageStrategyFactory : ILanguageStrategyFactory
    {
        private readonly Dictionary<string, ILanguageStrategy> _strategies;

        public LanguageStrategyFactory()
        {
            _strategies = new Dictionary<string, ILanguageStrategy>(StringComparer.OrdinalIgnoreCase)
            {
                { "javascript", new JavaScriptStrategy() },
                { "typescript", new TypeScriptStrategy() },
                { "csharp", new CSharpStrategy() },
                { "sql", new SQLStrategy() },
                { "html", new HTMLStrategy() }
            };
        }

        public ILanguageStrategy GetStrategy(string language)
        {
            if (_strategies.TryGetValue(language, out var strategy))
            {
                return strategy;
            }
            throw new BadRequestException($"Unsupported language: {language}");
        }
    }
}
