using System;

namespace CodeOptimizer.Domain.Services.CodeOptimizer.Strategy
{
    public class CSharpStrategy : ILanguageStrategy
    {
        public string Language => "csharp";

        public string GetOptimizationPrompt(string code)
        {
            return $@"You are a C# code optimization expert. Analyze and optimize the provided C# code.

CRITICAL INSTRUCTIONS:
- Return ONLY a raw JSON object
- Do NOT wrap your response in markdown code blocks
- Do NOT include ```json or ``` markers
- Do NOT add any text before or after the JSON
- Ensure all strings in the JSON are properly escaped (use \n for newlines, \"" for quotes, etc.)

Required JSON structure:
{{
  ""optimizedCode"": ""the complete optimized code with proper JSON escaping"",
  ""errorsFound"": [""list of errors or issues found""],
  ""optimizationsApplied"": [""list of optimizations applied""],
  ""description"": ""brief summary of changes made""
}}

Optimization focus areas:
- Modern C# features (latest version: records, pattern matching, init-only properties)
- LINQ optimizations and performance
- Async/await best practices
- Memory efficiency (Span<T>, stackalloc where appropriate)
- Nullable reference types
- Proper exception handling
- Code readability and maintainability

Code to analyze:
```csharp
{code}
```

Remember: Return ONLY the raw JSON object, nothing else.";
        }
    }
}