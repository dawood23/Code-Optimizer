using System;
using System.Collections.Generic;
using System.Text;

namespace CodeOptimizer.Domain.Services.CodeOptimizer.Strategy
{
    public class JavaScriptStrategy : ILanguageStrategy
    {
        public string Language => "javascript";

        public string GetOptimizationPrompt(string code)
        {
            return $@"You are a JavaScript code optimization expert. Analyze and optimize the provided JavaScript code.

CRITICAL INSTRUCTIONS FOR RESPONSE FORMAT:
- Return ONLY a raw JSON object
- Do NOT wrap your response in markdown code blocks
- Do NOT include ```json, ```javascript, or ``` markers anywhere
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
- ES6+ modern syntax (arrow functions, destructuring, spread operator, template literals)
- Async/await instead of callback hell or promise chains
- Performance improvements (avoid unnecessary loops, use efficient array methods like map/filter/reduce)
- Security issues (XSS prevention, injection vulnerabilities, input validation)
- Code readability and maintainability
- Best practices (use const/let instead of var, strict equality ===, avoid implicit type coercion)
- Error handling (try-catch blocks, proper error messages)
- Avoid global variables and namespace pollution
- Use strict mode ('use strict')
- Proper function and variable naming conventions
- Remove unused variables and dead code

Code to analyze:
```javascript
{code}
```

REMINDER: Your entire response must be ONLY the JSON object, starting with {{ and ending with }}. No markdown, no code blocks, no extra text.";
        }
    }
}