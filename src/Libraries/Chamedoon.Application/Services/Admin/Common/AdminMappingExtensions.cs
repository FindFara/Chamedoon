using System.Collections.Generic;
using System.Linq;
using Chamedoon.Application.Services.Admin.Common.Models;
using Chamedoon.Domin.Entity.Blogs;
using Chamedoon.Domin.Entity.Countries;
using Chamedoon.Domin.Entity.Customers;
using Chamedoon.Domin.Entity.Permissions;
using Chamedoon.Domin.Entity.Payments;
using Chamedoon.Domin.Entity.Users;

namespace Chamedoon.Application.Services.Admin.Common;

internal static class AdminMappingExtensions
{
    public static AdminUserDto ToAdminUserDto(this User user, IReadOnlyDictionary<string, string>? planTitles = null)
    {
        var fullName = user.Customer is Customer customer
            ? string.Join(" ", new[] { customer.FirstName, customer.LastName }.Where(s => !string.IsNullOrWhiteSpace(s)))
            : null;

        fullName = string.IsNullOrWhiteSpace(fullName) ? null : fullName;

        var role = user.UserRoles?.FirstOrDefault();

        var isActive = !user.LockoutEnd.HasValue || user.LockoutEnd.Value <= DateTimeOffset.Now;

        var planId = user.Customer?.SubscriptionPlanId;
        var planTitle = !string.IsNullOrWhiteSpace(planId) && planTitles is not null && planTitles.TryGetValue(planId, out var title)
            ? title
            : null;

        return new AdminUserDto(
            user.Id,
            user.Email ?? string.Empty,
            user.UserName ?? string.Empty,
            fullName,
            user.PhoneNumber,
            role?.RoleId,
            role?.Role?.Name,
            isActive,
            user.Created,
            planId,
            planTitle,
            user.Customer?.SubscriptionStartDateUtc,
            user.Customer?.SubscriptionEndDateUtc,
            user.Customer?.UsedEvaluations ?? 0);
    }

    public static AdminBlogPostDto ToAdminBlogPostDto(this Article article)
        => new(
            article.Id,
            article.ArticleTitle,
            article.Writer,
            article.ShortDescription,
            article.ArticleDescription,
            article.ArticleImageName,
            article.VisitCount,
            article.Created,
            article.LastModified);

    public static AdminRoleDto ToAdminRoleDto(this Role role)
    {
        var permissions = role.RolePermissions?
            .Where(p => !string.IsNullOrWhiteSpace(p.PermissionName))
            .Select(p => p.PermissionName!.Trim())
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .OrderBy(p => p)
            .ToList()
            ?? new List<string>();

        return new AdminRoleDto(role.Id, role.Name ?? string.Empty, permissions);
    }

    public static AdminPaymentDto ToAdminPaymentDto(this PaymentRequest payment)
    {
        var fullName = payment.Customer is Customer customer
            ? string.Join(" ", new[] { customer.FirstName, customer.LastName }.Where(s => !string.IsNullOrWhiteSpace(s)))
            : null;

        var fallbackName = payment.Customer?.User?.UserName ?? payment.Customer?.User?.Email ?? "کاربر";

        var finalAmount = payment.FinalAmount <= 0 ? payment.Amount : payment.FinalAmount;

        return new AdminPaymentDto(
            payment.Id,
            payment.PlanId,
            payment.Amount,
            finalAmount,
            payment.DiscountCode,
            payment.DiscountAmount,
            payment.Status,
            payment.CreatedAtUtc,
            payment.PaidAtUtc,
            payment.GatewayTrackId,
            payment.ReferenceCode,
            payment.Description,
            string.IsNullOrWhiteSpace(fullName) ? fallbackName : fullName!,
            payment.Customer?.User?.UserName,
            payment.Customer?.User?.Email);
    }

    public static AdminDiscountCodeDto ToAdminDiscountCodeDto(this DiscountCode code)
        => new(
            code.Id,
            code.Code,
            code.Type,
            code.Value,
            code.IsActive,
            code.CreatedAtUtc,
            code.ExpiresAtUtc,
            code.Description);

    public static AdminCountryDto ToAdminCountryDto(this Country country)
        => new(
            country.Id,
            country.Key,
            country.Name,
            country.InvestmentAmount,
            country.InvestmentCurrency,
            country.InvestmentNotes,
            country.AdditionalInfo,
            country.MaritalStatusImpact,
            country.LivingCosts.Select(cost => cost.ToAdminCountryLivingCostDto()).ToList(),
            country.Restrictions.Select(restriction => restriction.ToAdminCountryRestrictionDto()).ToList(),
            country.Jobs.Select(job => job.ToAdminCountryJobDto()).ToList(),
            country.Educations.Select(education => education.ToAdminCountryEducationDto()).ToList());

    public static AdminCountryLivingCostDto ToAdminCountryLivingCostDto(this CountryLivingCost cost)
        => new(cost.Id, cost.CountryId, cost.Type, cost.Value);

    public static AdminCountryRestrictionDto ToAdminCountryRestrictionDto(this CountryRestriction restriction)
        => new(restriction.Id, restriction.CountryId, restriction.Description);

    public static AdminCountryJobDto ToAdminCountryJobDto(this CountryJob job)
        => new(job.Id, job.CountryId, job.Title, job.Description, job.Score, job.ExperienceImpact);

    public static AdminCountryEducationDto ToAdminCountryEducationDto(this CountryEducation education)
        => new(education.Id, education.CountryId, education.FieldName, education.Description, education.Score, education.Level, education.LanguageRequirement);
}
