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
    private readonly SubscriptionMemoryStore _store;

    public GetSubscriptionPlansQueryHandler(SubscriptionMemoryStore store)
    {
        _store = store;
    }

    public Task<IReadOnlyList<SubscriptionPlan>> Handle(GetSubscriptionPlansQuery request, CancellationToken cancellationToken)
    {
        return Task.FromResult(_store.GetPlans());
    }
}

public class GetSubscriptionStatusQueryHandler : IRequestHandler<GetSubscriptionStatusQuery, SubscriptionStatus?>
{
    private readonly SubscriptionMemoryStore _store;

    public GetSubscriptionStatusQueryHandler(SubscriptionMemoryStore store)
    {
        _store = store;
    }

    public Task<SubscriptionStatus?> Handle(GetSubscriptionStatusQuery request, CancellationToken cancellationToken)
    {
        return Task.FromResult(_store.GetCurrentSubscription(request.User));
    }
}

public class CheckSubscriptionEligibilityQueryHandler : IRequestHandler<CheckSubscriptionEligibilityQuery, SubscriptionCheckResult>
{
    private readonly SubscriptionMemoryStore _store;

    public CheckSubscriptionEligibilityQueryHandler(SubscriptionMemoryStore store)
    {
        _store = store;
    }

    public Task<SubscriptionCheckResult> Handle(CheckSubscriptionEligibilityQuery request, CancellationToken cancellationToken)
    {
        return Task.FromResult(_store.CheckEligibility(request.User));
    }
}

public class ActivateSubscriptionCommandHandler : IRequestHandler<ActivateSubscriptionCommand>
{
    private readonly SubscriptionMemoryStore _store;

    public ActivateSubscriptionCommandHandler(SubscriptionMemoryStore store)
    {
        _store = store;
    }

    public Task Handle(ActivateSubscriptionCommand request, CancellationToken cancellationToken)
    {
        _store.ActivatePlan(request.User, request.PlanId);
        return Task.CompletedTask;
    }
}

public class RegisterSubscriptionUsageCommandHandler : IRequestHandler<RegisterSubscriptionUsageCommand>
{
    private readonly SubscriptionMemoryStore _store;

    public RegisterSubscriptionUsageCommandHandler(SubscriptionMemoryStore store)
    {
        _store = store;
    }

    public Task Handle(RegisterSubscriptionUsageCommand request, CancellationToken cancellationToken)
    {
        _store.RegisterImmigrationUsage(request.User);
        return Task.CompletedTask;
    }
}
