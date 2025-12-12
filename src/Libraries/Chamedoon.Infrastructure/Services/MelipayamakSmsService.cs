using Chamedoon.Application.Common.Interfaces;
using Chamedoon.Application.Common.Models;
using Chamedoon.Domin.Configs;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
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
        [JsonPropertyName("Value")]
        public string? Value { get; set; }

        [JsonPropertyName("RetStatus")]
        public int RetStatus { get; set; }

        [JsonPropertyName("StrRetStatus")]
        public string? StrRetStatus { get; set; }
    }

    public async Task<OperationResult> SendVerificationCodeAsync(
        string phoneNumber, string code, CancellationToken cancellationToken)
    {
        var client = _httpClientFactory.CreateClient(DefaultHttpClientName);
        var message = $"کد تایید چمدون: {code}";

        var payload = new FormUrlEncodedContent(new Dictionary<string, string>
        {
            ["username"] = _options.Username,
            ["password"] = _options.Password,
            ["from"] = _options.From,
            ["to"] = phoneNumber,
            ["text"] = message,
        });

        try
        {
            using var response = await client.PostAsync(_options.BaseUrl, payload, cancellationToken);
            var content = await response.Content.ReadAsStringAsync(cancellationToken);

            MelipayamakResponse? dto = TryParseMelipayamakResponse(content);

            if (response.IsSuccessStatusCode && dto is not null && dto.RetStatus == 1)
                return OperationResult.Success();

            _logger.LogError(
                "Melipayamak SMS send failed. Http={StatusCode}, RetStatus={RetStatus}, StrRetStatus={StrRetStatus}, Raw={Content}",
                response.StatusCode,
                dto?.RetStatus,
                dto?.StrRetStatus,
                content);

            return OperationResult.Fail("ارسال پیامک با خطا مواجه شد. لطفا دوباره تلاش کنید.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to send SMS through Melipayamak");
            return OperationResult.Fail("امکان ارسال پیامک وجود ندارد.");
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
            return null;
        }
    }
}
