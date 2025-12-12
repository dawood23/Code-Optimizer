using CodeOptimizer.Domain.Models;
using CodeOptimizer.Domain.Services.CodeOptimizer.Factory;
using CodeOptimizer.Domain.Services.Groq;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Text.Json;
using CodeOptimizer.Domain.Services.Metrices;
using Microsoft.Extensions.Configuration;
using CodeOptimizer.Infrastructure.Cache;
using Azure;
using CodeOptimizer.Domain.Services.Telemetry;

namespace CodeOptimizer.Domain.Services.CodeOptimizer
{
    public class CodeOptimizationService : ICodeOptimizationService
    {
        private readonly IGroqService _groqService;
        private readonly ILanguageStrategyFactory _strategyFactory;
        private readonly IMetricesService _metricesService;
        private readonly IBusinessMetrics _metrics;
        public CodeOptimizationService(
            IGroqService groqService,
            ILanguageStrategyFactory strategyFactory,
            IMetricesService metricesService,
            IBusinessMetrics metrics)
        {
            _groqService = groqService;
            _strategyFactory = strategyFactory;
            _metricesService = metricesService;
            _metrics = metrics;
        }

        public async Task<CodeOptimizationResponse> OptimizeCodeAsync(CodeOptimizationRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Code))
                throw new ArgumentException("Code cannot be empty", nameof(request.Code));
          
            var sw = Stopwatch.StartNew();
            var strategy = _strategyFactory.GetStrategy(request.Language);
            var prompt = strategy.GetOptimizationPrompt(request.Code);
            var groqResponse = await _groqService.GenerateResponseByPrompt(prompt);
            var response = ParseGroqResponse(groqResponse);
            sw.Stop();

            //_metricesService?.AddMetric(new AddMetricRequest
            //{
            //    MetricName = "Code-Optimization",
            //    Language = request.Language,
            //    TimeTaken = sw.ElapsedMilliseconds
            //});

            _metrics.TrackOptimizationCall(request.Language);
            return response;
        }

        private CodeOptimizationResponse ParseGroqResponse(string groqResponse)
        {
            try
            {
                string cleanedJson = ExtractJsonFromResponse(groqResponse);

                if (string.IsNullOrWhiteSpace(cleanedJson))
                    throw new JsonException("No JSON found in response.");

                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };

                var parsed = JsonSerializer.Deserialize<CodeOptimizationResponse>(cleanedJson, options);

                if (parsed == null)
                    throw new JsonException("Failed to deserialize JSON.");

                // Unescape the optimized code if it's JSON-escaped
                if (!string.IsNullOrEmpty(parsed.OptimizedCode))
                {
                    parsed.OptimizedCode = UnescapeJsonString(parsed.OptimizedCode);
                }

                return parsed;
            }
            catch (Exception ex)
            {
                // Log the exception if you have logging
                // _logger?.LogError(ex, "Failed to parse Groq response");

                return new CodeOptimizationResponse
                {
                    OptimizedCode = groqResponse,
                    Description = "Unable to parse AI response in expected format",
                    ErrorsFound = new List<string> { $"Response parsing failed: {ex.Message}" },
                    OptimizationsApplied = new List<string> { "See optimized code for details" }
                };
            }
        }

        private string ExtractJsonFromResponse(string raw)
        {
            if (string.IsNullOrWhiteSpace(raw))
                return string.Empty;

            // Remove all markdown code fences (with optional language identifiers)
            // This regex removes ```json, ```csharp, ``` etc. along with any trailing whitespace
            raw = Regex.Replace(raw, @"```[a-zA-Z]*\s*", "", RegexOptions.Multiline);
            raw = raw.Replace("```", "").Trim();

            // Find the first '{' and last '}'
            int startIndex = raw.IndexOf('{');
            int endIndex = raw.LastIndexOf('}');

            if (startIndex == -1 || endIndex == -1 || startIndex >= endIndex)
                return string.Empty;

            // Extract the substring between first { and last }
            string jsonCandidate = raw.Substring(startIndex, endIndex - startIndex + 1);

            // Validate it has balanced braces
            if (IsValidJsonStructure(jsonCandidate))
                return jsonCandidate;

            return string.Empty;
        }

        private bool IsValidJsonStructure(string json)
        {
            int braceCount = 0;
            int bracketCount = 0;
            bool inString = false;
            bool escaped = false;

            foreach (char c in json)
            {
                if (escaped)
                {
                    escaped = false;
                    continue;
                }

                if (c == '\\')
                {
                    escaped = true;
                    continue;
                }

                if (c == '"' && !escaped)
                    inString = !inString;

                if (!inString)
                {
                    if (c == '{') braceCount++;
                    if (c == '}') braceCount--;
                    if (c == '[') bracketCount++;
                    if (c == ']') bracketCount--;
                }
            }

            return braceCount == 0 && bracketCount == 0;
        }

        private string UnescapeJsonString(string escapedString)
        {
            if (string.IsNullOrEmpty(escapedString))
                return escapedString;

            try
            {
                // Use Regex.Unescape to handle \n, \t, \", etc.
                return Regex.Unescape(escapedString);
            }
            catch
            {
                // If unescaping fails, return original
                return escapedString;
            }
        }
    }
}