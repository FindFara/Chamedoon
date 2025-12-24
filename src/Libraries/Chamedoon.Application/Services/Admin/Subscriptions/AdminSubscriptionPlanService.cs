using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using Chamedoon.Application.Common.Interfaces.Admin;
using Chamedoon.Application.Common.Models;
using Chamedoon.Application.Services.Admin.Common.Models;
using Chamedoon.Domin.Entity.Subscriptions;

namespace Chamedoon.Application.Services.Admin.Subscriptions;

public class AdminSubscriptionPlanService : IAdminSubscriptionPlanService
{
    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web);

    private readonly IAdminSubscriptionPlanRepository _repository;

    public AdminSubscriptionPlanService(IAdminSubscriptionPlanRepository repository)
    {
        _repository = repository;
    }

    public async Task<OperationResult<PaginatedList<AdminSubscriptionPlanDto>>> GetPlansAsync(string? search, int page, int pageSize, CancellationToken cancellationToken)
    {
        var plans = await _repository.GetPlansAsync(search, page, pageSize, cancellationToken);
        var mapped = plans.Items.Select(MapDto).ToList();
        return OperationResult<PaginatedList<AdminSubscriptionPlanDto>>.Success(new PaginatedList<AdminSubscriptionPlanDto>(
            mapped,
            plans.TotalCount,
            plans.PageNumber,
            plans.TotalPages));
    }

    public async Task<OperationResult<AdminSubscriptionPlanDto>> GetPlanAsync(string id, CancellationToken cancellationToken)
    {
        var plan = await _repository.GetPlanAsync(id, cancellationToken);
        if (plan is null)
        {
            return OperationResult<AdminSubscriptionPlanDto>.Fail("پلن مورد نظر یافت نشد.");
        }

        return OperationResult<AdminSubscriptionPlanDto>.Success(MapDto(plan));
    }

    public async Task<OperationResult<AdminSubscriptionPlanDto>> CreatePlanAsync(AdminSubscriptionPlanInput input, CancellationToken cancellationToken)
    {
        var planId = string.IsNullOrWhiteSpace(input.Id) ? Guid.NewGuid().ToString("N") : input.Id.Trim();

        if (await _repository.ExistsAsync(planId, cancellationToken))
        {
            return OperationResult<AdminSubscriptionPlanDto>.Fail("شناسه پلن تکراری است.");
        }

        var plan = await _repository.CreatePlanAsync(BuildEntity(input with { Id = planId }), cancellationToken);
        return OperationResult<AdminSubscriptionPlanDto>.Success(MapDto(plan));
    }

    public async Task<OperationResult<AdminSubscriptionPlanDto>> UpdatePlanAsync(AdminSubscriptionPlanInput input, CancellationToken cancellationToken)
    {
        var updated = await _repository.UpdatePlanAsync(BuildEntity(input), cancellationToken);
        if (updated is null)
        {
            return OperationResult<AdminSubscriptionPlanDto>.Fail("پلن مورد نظر یافت نشد.");
        }

        return OperationResult<AdminSubscriptionPlanDto>.Success(MapDto(updated));
    }

    public async Task<OperationResult<bool>> DeactivatePlanAsync(string id, CancellationToken cancellationToken)
    {
        var updated = await _repository.DeactivatePlanAsync(id, cancellationToken);
        return updated
            ? OperationResult<bool>.Success(true)
            : OperationResult<bool>.Fail("امکان غیرفعال‌سازی پلن وجود ندارد.");
    }

    private static AdminSubscriptionPlanDto MapDto(SubscriptionPlanEntity plan)
        => new(
            plan.Id,
            plan.Title,
            plan.DurationLabel,
            plan.DurationMonths,
            plan.OriginalPrice,
            plan.Price,
            plan.EvaluationLimit,
            plan.IncludesAI,
            ParseFeatures(plan.FeaturesJson),
            plan.IsActive,
            plan.SortOrder);

    private static SubscriptionPlanEntity BuildEntity(AdminSubscriptionPlanInput input)
        => new()
        {
            Id = input.Id.Trim(),
            Title = input.Title.Trim(),
            DurationLabel = BuildDurationLabel(input.DurationMonths),
            DurationMonths = input.DurationMonths,
            OriginalPrice = input.OriginalPrice,
            Price = input.Price,
            EvaluationLimit = input.EvaluationLimit,
            IncludesAI = input.IncludesAI,
            FeaturesJson = JsonSerializer.Serialize(input.Features ?? new List<string>(), JsonOptions),
            IsActive = input.IsActive,
            SortOrder = input.SortOrder
        };

    private static string BuildDurationLabel(int durationMonths)
        => durationMonths switch
        {
            1 => "یک ماهه",
            2 => "دو ماهه",
            3 => "سه ماهه",
            6 => "شش ماهه",
            12 => "یکساله",
            _ => $"{durationMonths} ماهه"
        };

    private static IReadOnlyList<string> ParseFeatures(string? json)
    {
        if (string.IsNullOrWhiteSpace(json))
        {
            return new List<string>();
        }

        try
        {
            return JsonSerializer.Deserialize<List<string>>(json, JsonOptions) ?? new List<string>();
        }
        catch
        {
            return new List<string>();
        }
    }
}
