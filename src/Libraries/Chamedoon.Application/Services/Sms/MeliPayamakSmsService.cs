using System.Net.Http.Json;
using Chamedoon.Domin.Configs;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Chamedoon.Application.Services.Sms;

public interface ISmsService
{
    Task SendVerificationCodeAsync(string phoneNumber, string code, CancellationToken cancellationToken);
}

public class MeliPayamakSmsService : ISmsService
{
    private readonly HttpClient _httpClient;
    private readonly SmsConfig _smsConfig;
    private readonly ILogger<MeliPayamakSmsService> _logger;

    public MeliPayamakSmsService(HttpClient httpClient, IOptions<SmsConfig> smsOptions, ILogger<MeliPayamakSmsService> logger)
    {
        _httpClient = httpClient;
        _smsConfig = smsOptions.Value;
        _logger = logger;
    }

    public async Task SendVerificationCodeAsync(string phoneNumber, string code, CancellationToken cancellationToken)
    {
        var endpoint = string.IsNullOrWhiteSpace(_smsConfig.OtpEndpoint)
            ? "https://console.melipayamak.com/api/send/otp"
            : _smsConfig.OtpEndpoint;

        var payload = new
        {
            mobile = phoneNumber,
            templateId = _smsConfig.TemplateId ?? "login-code",
            parameters = new[]
            {
                new { name = "code", value = code }
            }
        };

        using var request = new HttpRequestMessage(HttpMethod.Post, endpoint)
        {
            Content = JsonContent.Create(payload)
        };
        request.Headers.Add("x-api-key", _smsConfig.ApiKey);

        try
        {
            _logger.LogInformation("Sending OTP via SMS to {PhoneNumber}", phoneNumber);
            using var response = await _httpClient.SendAsync(request, cancellationToken);
            response.EnsureSuccessStatusCode();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to send verification SMS to {PhoneNumber}", phoneNumber);
            throw;
        }
    }
}
