using System.Collections.Generic;
using System.Linq;
using Chamedoon.Application.Services.Admin.Common.Models;
using Chamedoon.Application.Services.Subscription;
using Chamedoon.Domin.Entity.Blogs;
using Chamedoon.Domin.Entity.Customers;
using Chamedoon.Domin.Entity.Permissions;
using Chamedoon.Domin.Entity.Payments;
using Chamedoon.Domin.Entity.Users;

namespace Chamedoon.Application.Services.Admin.Common;

internal static class AdminMappingExtensions
{
    public static AdminUserDto ToAdminUserDto(this User user)
    {
        var fullName = user.Customer is Customer customer
            ? string.Join(" ", new[] { customer.FirstName, customer.LastName }.Where(s => !string.IsNullOrWhiteSpace(s)))
            : null;

        fullName = string.IsNullOrWhiteSpace(fullName) ? null : fullName;

        var role = user.UserRoles?.FirstOrDefault();

        var isActive = !user.LockoutEnd.HasValue || user.LockoutEnd.Value <= DateTimeOffset.UtcNow;

        var plan = SubscriptionPlanCatalog.Find(user.Customer?.SubscriptionPlanId);

        return new AdminUserDto(
            user.Id,
            user.Email ?? string.Empty,
            user.UserName ?? string.Empty,
            fullName,
            role?.RoleId,
            role?.Role?.Name,
            isActive,
            user.Created,
            plan?.Id,
            plan?.Title,
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
}
