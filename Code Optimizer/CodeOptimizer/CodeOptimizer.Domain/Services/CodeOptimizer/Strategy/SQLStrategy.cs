using System;

namespace CodeOptimizer.Domain.Services.CodeOptimizer.Strategy
{
    public class SQLStrategy : ILanguageStrategy
    {
        public string Language => "sql";

        public string GetOptimizationPrompt(string code)
        {
            return $@"You are a SQL optimization expert. Analyze and optimize the provided SQL code.

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
- Query performance (proper indexing suggestions, avoid SELECT *)
- JOIN optimization (use appropriate JOIN types)
- Subquery optimization (consider CTEs or JOINs instead)
- Avoid N+1 query problems
- Proper use of WHERE clauses and filtering
- SQL injection prevention (parameterization)
- Transaction management
- Readability and formatting

Code to analyze:
```sql
{code}
```

Remember: Return ONLY the raw JSON object, nothing else.";
        }
    }
}