
using Microsoft.Extensions.Configuration;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace CodeOptimizer.Domain.Services.Groq
{
    public class GroqService:IGroqService
    {
        private readonly HttpClient _http;
        private readonly string _apiKey;

        public GroqService(IConfiguration config)
        {
            _http = new HttpClient();
            _apiKey = config["Groq:ApiKey"];

            if (_apiKey == null)
                throw new Exception("Groq API key is missing! Add it to appsettings.json");
        }

        public async Task<string> GenerateHelloWorld()
        {
            _http.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", _apiKey);

            var body = new
            {
                model = "llama-3.3-70b-versatile",
                messages = new[]
                {
            new {
                role = "user",
                content = "Write a complete C# Hello World program."
            }
        }
            };

            var jsonBody = JsonSerializer.Serialize(body);

            var response = await _http.PostAsync(
                "https://api.groq.com/openai/v1/chat/completions",
                new StringContent(jsonBody, Encoding.UTF8, "application/json")
            );

            var json = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
                throw new Exception($"Groq Error: {json}");

            using var doc = JsonDocument.Parse(json);

            var result = doc.RootElement
                .GetProperty("choices")[0]
                .GetProperty("message")
                .GetProperty("content")
                .GetString();

            return result;
        }

        public async Task<string> GenerateResponseByPrompt(string Prompt)
        {
            _http.DefaultRequestHeaders.Authorization =
               new AuthenticationHeaderValue("Bearer", _apiKey);

            var body = new
            {
                model = "llama-3.3-70b-versatile",
                messages = new[]
                {
            new {
                role = "user",
                content = Prompt
            }
        }
            };

            var jsonBody = JsonSerializer.Serialize(body);

            var response = await _http.PostAsync(
                "https://api.groq.com/openai/v1/chat/completions",
                new StringContent(jsonBody, Encoding.UTF8, "application/json")
            );

            var json = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
                throw new Exception($"Groq Error: {json}");

            using var doc = JsonDocument.Parse(json);

            var result = doc.RootElement
                .GetProperty("choices")[0]
                .GetProperty("message")
                .GetProperty("content")
                .GetString();

            return result;
        }
    }
}
