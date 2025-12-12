using System.Net.Http;
using Chamedoon.Application.Common.Interfaces;
using Chamedoon.Application.Common.Models;
using Chamedoon.Domin.Configs;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Chamedoon.Infrastructure.Services;

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

    public async Task<OperationResult> SendVerificationCodeAsync(string phoneNumber, string code, CancellationToken cancellationToken)
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

            if (response.IsSuccessStatusCode)
            {
                return OperationResult.Success();
            }

            _logger.LogWarning("Melipayamak SMS send failed with status {StatusCode}: {Content}", response.StatusCode, content);
            return OperationResult.Fail("ارسال پیامک با خطا مواجه شد. لطفا دوباره تلاش کنید.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to send SMS through Melipayamak");
            return OperationResult.Fail("امکان ارسال پیامک وجود ندارد.");
        }
    }
}
