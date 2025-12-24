using System.Collections.Generic;

namespace Chamedoon.Application.Services.Admin.Common.Models;

public record AdminSubscriptionPlanDto(
    string Id,
    string Title,
    string DurationLabel,
    int OriginalPrice,
    int Price,
    int? EvaluationLimit,
    bool IncludesAI,
    IReadOnlyList<string> Features,
    bool IsActive,
    int SortOrder);

public record AdminSubscriptionPlanInput(
    string Id,
    string Title,
    string DurationLabel,
    int OriginalPrice,
    int Price,
    int? EvaluationLimit,
    bool IncludesAI,
    IReadOnlyList<string> Features,
    bool IsActive,
    int SortOrder);
