using System;

namespace CodeOptimizer.Domain.Services.CodeOptimizer.Strategy
{
    public class HTMLStrategy : ILanguageStrategy
    {
        public string Language => "html";

        public string GetOptimizationPrompt(string code)
        {
            return $@"You are an HTML optimization expert. Analyze and optimize the provided HTML code.

CRITICAL INSTRUCTIONS FOR RESPONSE FORMAT:
- Return ONLY a raw JSON object
- Do NOT wrap your response in markdown code blocks
- Do NOT include ```json, ```html, or ``` markers anywhere
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
- Semantic HTML5 elements (use header, nav, main, article, section, aside, footer instead of generic divs)
- Accessibility improvements (ARIA labels, alt text for images, proper roles, keyboard navigation)
- SEO optimization (meta tags, Open Graph tags, structured data, proper heading hierarchy h1-h6)
- Performance (lazy loading images, proper resource hints, minimize DOM depth)
- Valid HTML structure (proper nesting, closing tags, DOCTYPE)
- Mobile responsiveness considerations (viewport meta tag, responsive images)
- Best practices for forms and inputs (labels, fieldsets, proper input types)
- Remove deprecated HTML attributes
- Proper use of semantic elements for better screen reader support

Code to analyze:
```html
{code}
```

REMINDER: Your entire response must be ONLY the JSON object, starting with {{ and ending with }}. No markdown, no code blocks, no extra text.";
        }
    }
}