using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Text.Json;
using Chamedoon.Application.Common.Interfaces;
using Chamedoon.Application.Services.Subscription;
using Chamedoon.Domin.Entity.Payments;
using Chamedoon.Domin.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Chamedoon.Application.Services.Payments;

public class PaymentService
{
    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web)
    {
        PropertyNameCaseInsensitive = true
    };

    private readonly IApplicationDbContext _context;
    private readonly SubscriptionService _subscriptionService;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly PaymentGatewayOptions _options;
    private readonly ILogger<PaymentService> _logger;

    public PaymentService(
        IApplicationDbContext context,
        SubscriptionService subscriptionService,
        IHttpClientFactory httpClientFactory,
        IOptions<PaymentGatewayOptions> options,
        ILogger<PaymentService> logger)
    {
        _context = context;
        _subscriptionService = subscriptionService;
        _httpClientFactory = httpClientFactory;
        _options = options.Value;
        _logger = logger;
    }

    public async Task<PaymentRedirectResult> StartSubscriptionPaymentAsync(ClaimsPrincipal user, string planId, string callbackUrl, CancellationToken cancellationToken)
    {
        var plan = Subscription.SubscriptionPlanCatalog.Find(planId);
        if (plan is null)
        {
            return PaymentRedirectResult.Failure("پلن انتخاب شده معتبر نیست.");
        }

        var userId = GetUserId(user);
        if (!userId.HasValue)
        {
            return PaymentRedirectResult.Failure("برای خرید اشتراک لازم است وارد حساب کاربری خود شوی.");
        }

        var account = await _context.User
            .Include(u => u.Customer)
            .FirstOrDefaultAsync(u => u.Id == userId.Value, cancellationToken);

        if (account is null)
        {
            return PaymentRedirectResult.Failure("کاربر یافت نشد.");
        }

        var customer = account.Customer ?? new Domin.Entity.Customers.Customer
        {
            Id = account.Id,
            User = account,
            Gender = Gender.Unknown
        };

        if (account.Customer is null)
        {
            await _context.Customers.AddAsync(customer, cancellationToken);
        }

        var paymentRequest = new PaymentRequest
        {
            Customer = customer,
            PlanId = plan.Id,
            Amount = plan.Price,
            Description = $"خرید اشتراک {plan.Title}",
            Status = PaymentStatus.Pending,
            ReferenceCode = Guid.NewGuid().ToString("N")[..12]
        };

        await _context.PaymentRequests.AddAsync(paymentRequest, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        var callbackWithPaymentId = AppendPaymentId(callbackUrl, paymentRequest.Id);
        paymentRequest.CallbackUrl = callbackWithPaymentId;

        var client = _httpClientFactory.CreateClient(PaymentGatewayOptions.HttpClientName);
        var requestPayload = new ZibalRequestPayload
        {
            Merchant = _options.MerchantId,
            Amount = Math.Max(1, _options.AmountMultiplier) * plan.Price,
            CallbackUrl = callbackWithPaymentId,
            Description = paymentRequest.Description ?? string.Empty,
            OrderId = paymentRequest.ReferenceCode ?? paymentRequest.Id.ToString(CultureInfo.InvariantCulture),
            Mobile = NormalizePhone(account.PhoneNumber)
        };

        HttpResponseMessage? response = null;
        string responseContent = string.Empty;
        ZibalRequestResponse? gatewayResponse = null;

        try
        {
            response = await client.PostAsJsonAsync(BuildEndpoint("request"), requestPayload, cancellationToken);
            responseContent = await response.Content.ReadAsStringAsync(cancellationToken);
            gatewayResponse = JsonSerializer.Deserialize<ZibalRequestResponse>(responseContent, JsonOptions);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to request payment from gateway");
            paymentRequest.Status = PaymentStatus.Failed;
            paymentRequest.LastError = "خطا در اتصال به درگاه پرداخت.";
            await _context.SaveChangesAsync(cancellationToken);
            return PaymentRedirectResult.Failure("در حال حاضر امکان اتصال به درگاه پرداخت وجود ندارد.");
        }

        var requestResultCode = gatewayResponse?.Result ?? (response is null ? -1 : (int)response.StatusCode);
        await AddResponseAsync(paymentRequest, "request", requestResultCode, gatewayResponse?.Message, responseContent, gatewayResponse?.TrackId?.ToString(), null, null, null, null, cancellationToken);

        if (gatewayResponse?.Result == 100 && gatewayResponse.TrackId.HasValue)
        {
            paymentRequest.Status = PaymentStatus.Redirected;
            paymentRequest.GatewayTrackId = gatewayResponse.TrackId.Value.ToString(CultureInfo.InvariantCulture);
            paymentRequest.PaymentUrl = BuildStartUrl(gatewayResponse.TrackId.Value);
            paymentRequest.LastError = null;
            await _context.SaveChangesAsync(cancellationToken);
            return PaymentRedirectResult.Success(paymentRequest.PaymentUrl, paymentRequest.Id);
        }

        paymentRequest.Status = PaymentStatus.Failed;
        paymentRequest.LastError = gatewayResponse?.Message ?? "در حال حاضر امکان ایجاد تراکنش وجود ندارد.";
        await _context.SaveChangesAsync(cancellationToken);
        return PaymentRedirectResult.Failure(paymentRequest.LastError);
    }

    public async Task<PaymentVerificationResult> VerifyPaymentAsync(ClaimsPrincipal? user, long paymentRequestId, string trackId, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(trackId))
        {
            return PaymentVerificationResult.FromStatus(PaymentStatus.Failed, "شناسه تراکنش ارسال نشده است.", null, null, null);
        }

        var payment = await _context.PaymentRequests
            .Include(p => p.Customer)
                .ThenInclude(c => c.User)
            .FirstOrDefaultAsync(p => p.Id == paymentRequestId, cancellationToken);

        if (payment is null || !string.Equals(payment.GatewayTrackId, trackId, StringComparison.OrdinalIgnoreCase))
        {
            return PaymentVerificationResult.FromStatus(PaymentStatus.Failed, "تراکنش یافت نشد.", null, null, null);
        }

        var currentUserId = GetUserId(user);
        if (currentUserId.HasValue && currentUserId.Value != payment.CustomerId)
        {
            return PaymentVerificationResult.FromStatus(PaymentStatus.Failed, "این تراکنش متعلق به حساب شما نیست.", payment.Id, payment.PlanId, payment.CustomerId);
        }

        if (payment.Status == PaymentStatus.Paid)
        {
            return PaymentVerificationResult.FromStatus(PaymentStatus.Paid, "این پرداخت قبلا تایید شده است.", payment.Id, payment.PlanId, payment.CustomerId);
        }

        var client = _httpClientFactory.CreateClient(PaymentGatewayOptions.HttpClientName);
        var verifyPayload = new ZibalVerifyPayload
        {
            Merchant = _options.MerchantId,
            TrackId = trackId
        };

        HttpResponseMessage? response = null;
        string responseContent = string.Empty;
        ZibalVerifyResponse? gatewayResponse = null;

        try
        {
            response = await client.PostAsJsonAsync(BuildEndpoint("verify"), verifyPayload, cancellationToken);
            responseContent = await response.Content.ReadAsStringAsync(cancellationToken);
            gatewayResponse = JsonSerializer.Deserialize<ZibalVerifyResponse>(responseContent, JsonOptions);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to verify payment track {TrackId}", trackId);
            payment.Status = PaymentStatus.Failed;
            payment.LastError = "خطا در تایید تراکنش";
            await _context.SaveChangesAsync(cancellationToken);
            return PaymentVerificationResult.FromStatus(payment.Status, payment.LastError, payment.Id, payment.PlanId, payment.CustomerId);
        }

        var paidAt = ParsePaidAt(gatewayResponse?.PaidAt);
        var verifyResultCode = gatewayResponse?.Result ?? (response is null ? -1 : (int)response.StatusCode);
        await AddResponseAsync(payment, "verify", verifyResultCode, gatewayResponse?.Message, responseContent, trackId, gatewayResponse?.RefNumber, gatewayResponse?.CardNumber, gatewayResponse?.Amount, paidAt, cancellationToken);

        if (gatewayResponse?.Result == 100)
        {
            payment.Status = PaymentStatus.Paid;
            payment.PaidAtUtc = paidAt ?? DateTime.UtcNow;
            payment.LastError = null;

            if (!string.IsNullOrWhiteSpace(payment.PlanId))
            {
                await _subscriptionService.ActivatePlanForUserAsync(payment.CustomerId, payment.PlanId, cancellationToken);
            }
            else
            {
                await _context.SaveChangesAsync(cancellationToken);
            }

            return PaymentVerificationResult.FromStatus(payment.Status, "پرداخت با موفقیت تایید شد.", payment.Id, payment.PlanId, payment.CustomerId);
        }

        payment.Status = PaymentStatus.Failed;
        payment.LastError = gatewayResponse?.Message ?? "پرداخت تایید نشد.";
        await _context.SaveChangesAsync(cancellationToken);
        return PaymentVerificationResult.FromStatus(payment.Status, payment.LastError, payment.Id, payment.PlanId, payment.CustomerId);
    }

    private async Task AddResponseAsync(PaymentRequest request, string type, int resultCode, string? message, string rawPayload, string? trackId, string? referenceId, string? cardNumber, int? amount, DateTime? paidAt, CancellationToken cancellationToken)
    {
        var response = new PaymentResponse
        {
            PaymentRequestId = request.Id,
            Type = type,
            ResultCode = resultCode,
            Message = message,
            RawPayload = Truncate(rawPayload, 4000),
            GatewayTrackId = trackId,
            ReferenceId = referenceId,
            CardNumber = cardNumber,
            Amount = amount,
            PaidAtUtc = paidAt
        };

        await _context.PaymentResponses.AddAsync(response, cancellationToken);
    }

    private string BuildEndpoint(string relative)
    {
        if (string.IsNullOrWhiteSpace(_options.BaseUrl))
        {
            return relative;
        }

        return $"{_options.BaseUrl.TrimEnd('/')}/{relative.TrimStart('/')}";
    }

    private string BuildStartUrl(long trackId)
        => string.IsNullOrWhiteSpace(_options.StartUrl)
            ? $"https://sandbox.zibal.ir/start/{trackId}"
            : $"{_options.StartUrl.TrimEnd('/')}/{trackId}";

    private static string AppendPaymentId(string callbackUrl, long paymentId)
    {
        if (string.IsNullOrWhiteSpace(callbackUrl))
        {
            return string.Empty;
        }

        var separator = callbackUrl.Contains('?') ? '&' : '?';
        return $"{callbackUrl}{separator}paymentId={paymentId}";
    }

    private static DateTime? ParsePaidAt(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return null;
        }

        if (long.TryParse(value, out var timestamp))
        {
            // Zibal returns Unix timestamps in seconds
            return DateTimeOffset.FromUnixTimeMilliseconds(timestamp.ToString().Length > 10 ? timestamp : timestamp * 1000)
                .UtcDateTime;
        }

        if (DateTime.TryParse(value, CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal, out var parsed))
        {
            return parsed.ToUniversalTime();
        }

        return null;
    }

    private static string? NormalizePhone(string? input)
    {
        if (string.IsNullOrWhiteSpace(input))
        {
            return null;
        }

        var digits = new string(input.Where(char.IsDigit).ToArray());
        if (string.IsNullOrWhiteSpace(digits))
        {
            return null;
        }

        if (digits.StartsWith("98"))
        {
            return digits;
        }

        if (digits.StartsWith("0"))
        {
            return $"98{digits[1..]}";
        }

        return digits.Length == 10 ? $"98{digits}" : digits;
    }

    private static long? GetUserId(ClaimsPrincipal? user)
    {
        if (user is null)
        {
            return null;
        }

        var idValue = user.FindFirstValue(ClaimTypes.NameIdentifier) ?? user.Identity?.Name;
        return long.TryParse(idValue, out var id) ? id : null;
    }

    private static string? Truncate(string? value, int maxLength)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return value;
        }

        return value.Length <= maxLength ? value : value[..maxLength];
    }

    private record ZibalRequestPayload
    {
        public string Merchant { get; init; } = string.Empty;
        public int Amount { get; init; }
        public string CallbackUrl { get; init; } = string.Empty;
        public string Description { get; init; } = string.Empty;
        public string OrderId { get; init; } = string.Empty;
        public string? Mobile { get; init; }
    }

    private record ZibalRequestResponse
    {
        public int Result { get; init; }
        public string? Message { get; init; }
        public long? TrackId { get; init; }
    }

    private record ZibalVerifyPayload
    {
        public string Merchant { get; init; } = string.Empty;
        public string TrackId { get; init; } = string.Empty;
    }

    private record ZibalVerifyResponse
    {
        public int Result { get; init; }
        public string? Message { get; init; }
        public string? RefNumber { get; init; }
        public string? CardNumber { get; init; }
        public int? Amount { get; init; }
        public string? PaidAt { get; init; }
    }
}

