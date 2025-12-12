using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Text;

namespace CodeOptimizer.Domain.Services.Groq
{
    public interface IGroqService
    {
        Task<string> GenerateResponseByPrompt(string Prompt);
        Task<string> GenerateHelloWorld();
    }
}
