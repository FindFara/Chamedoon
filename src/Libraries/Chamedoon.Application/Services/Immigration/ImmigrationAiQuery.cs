using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Chamedoon.Application.Services.Immigration
{
    public class GroqOptions
    {
        public const string SectionName = "Groq";

        public string ApiKey { get; set; } = string.Empty;
        public string BaseUrl { get; set; } = "https://api.groq.com/openai/v1/";
        public string Model { get; set; } = string.Empty;
    }

    public class ImmigrationAiQuery : IRequest<ImmigrationResult>
    {
        public ImmigrationInput Input { get; set; } = new ImmigrationInput();
    }

    public class ImmigrationAiQueryHandler : IRequestHandler<ImmigrationAiQuery, ImmigrationResult>
    {
        private static readonly JsonSerializerOptions SerializerOptions = new()
        {
            PropertyNameCaseInsensitive = true,
            Converters = { new JsonStringEnumConverter() }
        };

        private readonly IHttpClientFactory _httpClientFactory;
        private readonly GroqOptions _options;
        private readonly ILogger<ImmigrationAiQueryHandler> _logger;

        public ImmigrationAiQueryHandler(
            IHttpClientFactory httpClientFactory,
            IOptions<GroqOptions> options,
            ILogger<ImmigrationAiQueryHandler> logger)
        {
            _httpClientFactory = httpClientFactory;
            _options = options.Value;
            _logger = logger;
        }

        public async Task<ImmigrationResult> Handle(ImmigrationAiQuery request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(_options.ApiKey) ||
                string.IsNullOrWhiteSpace(_options.BaseUrl) ||
                string.IsNullOrWhiteSpace(_options.Model))
            {
                _logger.LogWarning("Groq options are not configured.");
                return new ImmigrationResult();
            }

            var systemPrompt = BuildSystemPrompt();
            var userPrompt = BuildUserPrompt(request.Input);

            var content = await RequestCompletionAsync(systemPrompt, userPrompt, cancellationToken);
            if (TryParseResult(content, out var parsed))
            {
                return parsed;
            }

            var repairPrompt = BuildRepairPrompt(content);
            var repaired = await RequestCompletionAsync(systemPrompt, repairPrompt, cancellationToken);
            if (TryParseResult(repaired, out parsed))
            {
                return parsed;
            }

            _logger.LogWarning("Groq response invalid after repair attempt.");
            return new ImmigrationResult();
        }

        private async Task<string?> RequestCompletionAsync(
            string systemPrompt,
            string userPrompt,
            CancellationToken cancellationToken)
        {
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_options.BaseUrl, UriKind.Absolute);

            using var request = new HttpRequestMessage(HttpMethod.Post, "chat/completions");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _options.ApiKey);

            var payload = new
            {
                model = _options.Model,
                temperature = 0.2,
                messages = new[]
                {
                    new { role = "system", content = systemPrompt },
                    new { role = "user", content = userPrompt }
                }
            };

            request.Content = new StringContent(
                JsonSerializer.Serialize(payload, SerializerOptions),
                Encoding.UTF8,
                "application/json");

            using var response = await client.SendAsync(request, cancellationToken);
            if (!response.IsSuccessStatusCode)
            {
                _logger.LogWarning("Groq request failed with status {StatusCode}.", response.StatusCode);
                return null;
            }

            var raw = await response.Content.ReadAsStringAsync(cancellationToken);
            return ExtractAssistantContent(raw);
        }

        private static string BuildSystemPrompt()
        {
            return string.Join(" ", new[]
            {
                "You are an immigration recommendation engine for Iranian users.",
                "Return ONLY a single JSON object with the exact schema of ImmigrationResult.",
                "TopCountries must have exactly 5 items ranked best to worst.",
                "Country enum values must be exact enum names.",
                "Job, Field, DegreeLevel enum values must be exact enum names when used.",
                "Scores must be between 0 and 100.",
                "All human-readable text fields must be written in Persian (Farsi).",
                "JSON property names must remain in English as specified by the schema.",
                "Do not include markdown, code fences, or extra text.",
                "Keep responses concise and within field length limits."
            });
        }

        private static string BuildUserPrompt(ImmigrationInput input)
        {
            var serializedInput = JsonSerializer.Serialize(input, SerializerOptions);
            return string.Join("\n", new[]
            {
                "Generate recommendations for the following input JSON:",
                serializedInput,
                "Ensure all required fields are present and valid."
            });
        }

        private static string BuildRepairPrompt(string? previousResponse)
        {
            return string.Join("\n", new[]
            {
                "Your previous response was invalid. Return ONLY corrected JSON matching the schema.",
                "Do not include markdown or any extra text.",
                "Previous response:",
                previousResponse ?? string.Empty
            });
        }

        private static string? ExtractAssistantContent(string rawResponse)
        {
            try
            {
                using var document = JsonDocument.Parse(rawResponse);
                if (document.RootElement.TryGetProperty("choices", out var choices) &&
                    choices.ValueKind == JsonValueKind.Array &&
                    choices.GetArrayLength() > 0)
                {
                    var message = choices[0].GetProperty("message");
                    if (message.TryGetProperty("content", out var contentElement))
                    {
                        return contentElement.GetString();
                    }
                }
            }
            catch (JsonException)
            {
                return null;
            }

            return null;
        }

        private static bool TryParseResult(string? content, out ImmigrationResult result)
        {
            result = new ImmigrationResult();

            if (string.IsNullOrWhiteSpace(content))
            {
                return false;
            }

            try
            {
                var parsed = JsonSerializer.Deserialize<ImmigrationResult>(content, SerializerOptions);
                if (parsed?.TopCountries is null || parsed.TopCountries.Count != 5)
                {
                    return false;
                }

                result = parsed;
                return true;
            }
            catch (JsonException)
            {
                return false;
            }
        }
    }
}
