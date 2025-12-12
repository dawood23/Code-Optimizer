using System;

namespace CodeOptimizer.Domain.Services.CodeOptimizer.Strategy
{
    public class TypeScriptStrategy : ILanguageStrategy
    {
        public string Language => "typescript";

        public string GetOptimizationPrompt(string code)
        {
            return $@"You are a TypeScript code optimization expert. Analyze and optimize the provided TypeScript code.

CRITICAL INSTRUCTIONS FOR RESPONSE FORMAT:
- Return ONLY a raw JSON object
- Do NOT wrap your response in markdown code blocks
- Do NOT include ```json, ```typescript, or ``` markers anywhere
- Do NOT add any explanatory text before or after the JSON
- The very first character of your response must be {{ and the last character must be }}
- Ensure all strings in the JSON are properly escaped (use \n for newlines, \"" for quotes, \\ for backslashes)

Required JSON structure (use this exact format):
{{
  ""optimizedCode"": ""the complete optimized code with proper JSON escaping"",
  ""errorsFound"": [""error 1"", ""error 2""],
  ""optimizationsApplied"": [""optimization 1"", ""optimization 2""],
  ""description"": ""brief summary of changes made""
}}

Optimization focus areas:
- Type safety improvements (avoid 'any', use proper types and interfaces)
- TypeScript best practices
- Modern TypeScript features (optional chaining ?., nullish coalescing ??)
- Interface and type definitions (prefer interfaces for objects)
- Null safety (strict null checks)
- Generic types where appropriate
- Union types and type guards
- Readonly properties and immutability
- Async/await patterns
- Performance optimizations
- Code organization and maintainability

Code to analyze:
```typescript
{code}
```

REMINDER: Your entire response must be ONLY the JSON object, starting with {{ and ending with }}. No markdown, no code blocks, no extra text.";
        }
    }
}