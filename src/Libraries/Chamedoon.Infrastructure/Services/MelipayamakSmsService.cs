using Chamedoon.Application.Common.Interfaces;
using Chamedoon.Application.Common.Models;
using Chamedoon.Domin.Configs;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;

public class MelipayamakSmsService : ISmsService
{
    private const string DefaultHttpClientName = "MelipayamakSms";

    private readonly IHttpClientFactory _httpClientFactory;
    private readonly MelipayamakConfig _options;
    private readonly ILogger<MelipayamakSmsService> _logger;

    public MelipayamakSmsService(
        IHttpClientFactory httpClientFactory,
        IOptions<MelipayamakConfig> options,
        ILogger<MelipayamakSmsService> logger)
    {
        _httpClientFactory = httpClientFactory;
        _logger = logger;
        _options = options.Value;
    }

    private sealed class MelipayamakResponse
    {
        [JsonPropertyName("code")]
        public string? Code { get; set; }

        [JsonPropertyName("status")]
        public string? Status { get; set; }
    }

    private sealed class LegacyMelipayamakResponse
    {
        [JsonPropertyName("Value")]
        public string? Value { get; set; }

        [JsonPropertyName("RetStatus")]
        public int RetStatus { get; set; }

        [JsonPropertyName("StrRetStatus")]
        public string? StrRetStatus { get; set; }
    }

    public async Task<OperationResult<string>> SendVerificationCodeAsync(
        string phoneNumber, string code, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(_options.OtpTemplateCode))
        {
            _logger.LogError("Melipayamak OTP template code is not configured.");
            return OperationResult<string>.Fail("امکان ارسال پیامک وجود ندارد.");
        }

        var client = _httpClientFactory.CreateClient(DefaultHttpClientName);
        var endpoint = $"api/send/otp/{_options.OtpTemplateCode}";
        var payload = new { to = phoneNumber };

        try
        {
            using var response = await client.PostAsJsonAsync(endpoint, payload, cancellationToken);
            var content = await response.Content.ReadAsStringAsync(cancellationToken);

            if (response.IsSuccessStatusCode)
            {
                var dto = TryParseMelipayamakResponse(content);
                var sentCode = dto?.Code ?? code;
                if (string.IsNullOrWhiteSpace(sentCode) is false)
                {
                    return OperationResult<string>.Success(sentCode);
                }
            }

            _logger.LogError(
                "Melipayamak SMS send failed. Http={StatusCode}, Raw={Content}",
                response.StatusCode,
                content);

            return OperationResult<string>.Fail("ارسال پیامک با خطا مواجه شد. لطفا دوباره تلاش کنید.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to send SMS through Melipayamak");
            return OperationResult<string>.Fail("امکان ارسال پیامک وجود ندارد.");
        }
    }

    private static MelipayamakResponse? TryParseMelipayamakResponse(string content)
    {
        if (string.IsNullOrWhiteSpace(content)) return null;

        content = content.Trim();

        try
        {
            if (content.Length >= 2 && content[0] == '"' && content[^1] == '"')
            {
                var inner = JsonSerializer.Deserialize<string>(content);
                if (!string.IsNullOrWhiteSpace(inner))
                    content = inner;
            }

            return JsonSerializer.Deserialize<MelipayamakResponse>(
                content,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }
        catch (JsonException)
        {
            try
            {
                var legacyResponse = JsonSerializer.Deserialize<LegacyMelipayamakResponse>(
                    content,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                if (legacyResponse is not null && legacyResponse.RetStatus == 1)
                {
                    return new MelipayamakResponse
                    {
                        Code = legacyResponse.Value,
                        Status = legacyResponse.StrRetStatus
                    };
                }
            }
            catch (JsonException)
            {
                // ignored
            }

            return null;
        }
    }
}
