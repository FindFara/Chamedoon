using System.Security.Claims;
using MediatR;

namespace Chamedoon.Application.Services.Subscription;

public record GetSubscriptionPlansQuery() : IRequest<IReadOnlyList<SubscriptionPlan>>;

public record GetSubscriptionStatusQuery(ClaimsPrincipal User) : IRequest<SubscriptionStatus?>;

public record CheckSubscriptionEligibilityQuery(ClaimsPrincipal User) : IRequest<SubscriptionCheckResult>;

public record ActivateSubscriptionCommand(ClaimsPrincipal User, string PlanId) : IRequest;

public record RegisterSubscriptionUsageCommand(ClaimsPrincipal User) : IRequest;

public class GetSubscriptionPlansQueryHandler : IRequestHandler<GetSubscriptionPlansQuery, IReadOnlyList<SubscriptionPlan>>
{
    private readonly SubscriptionService _service;

    public GetSubscriptionPlansQueryHandler(SubscriptionService service)
    {
        _service = service;
    }

    public Task<IReadOnlyList<SubscriptionPlan>> Handle(GetSubscriptionPlansQuery request, CancellationToken cancellationToken)
    {
        return _service.GetPlansAsync();
    }
}

public class GetSubscriptionStatusQueryHandler : IRequestHandler<GetSubscriptionStatusQuery, SubscriptionStatus?>
{
    private readonly SubscriptionService _service;

    public GetSubscriptionStatusQueryHandler(SubscriptionService service)
    {
        _service = service;
    }

    public Task<SubscriptionStatus?> Handle(GetSubscriptionStatusQuery request, CancellationToken cancellationToken)
    {
        return _service.GetCurrentSubscriptionAsync(request.User, cancellationToken);
    }
}

public class CheckSubscriptionEligibilityQueryHandler : IRequestHandler<CheckSubscriptionEligibilityQuery, SubscriptionCheckResult>
{
    private readonly SubscriptionService _service;

    public CheckSubscriptionEligibilityQueryHandler(SubscriptionService service)
    {
        _service = service;
    }

    public Task<SubscriptionCheckResult> Handle(CheckSubscriptionEligibilityQuery request, CancellationToken cancellationToken)
    {
        return _service.CheckEligibilityAsync(request.User, cancellationToken);
    }
}

public class ActivateSubscriptionCommandHandler : IRequestHandler<ActivateSubscriptionCommand>
{
    private readonly SubscriptionService _service;

    public ActivateSubscriptionCommandHandler(SubscriptionService service)
    {
        _service = service;
    }

    public async Task Handle(ActivateSubscriptionCommand request, CancellationToken cancellationToken)
    {
        await _service.ActivatePlanAsync(request.User, request.PlanId, cancellationToken);
    }
}

public class RegisterSubscriptionUsageCommandHandler : IRequestHandler<RegisterSubscriptionUsageCommand>
{
    private readonly SubscriptionService _service;

    public RegisterSubscriptionUsageCommandHandler(SubscriptionService service)
    {
        _service = service;
    }

    public async Task Handle(RegisterSubscriptionUsageCommand request, CancellationToken cancellationToken)
    {
        await _service.RegisterImmigrationUsageAsync(request.User, cancellationToken);
    }
}
