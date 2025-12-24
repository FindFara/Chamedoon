using System;
using System.Linq;
using System.Text;
using Chamedoon.Application.Common.Interfaces.Admin;
using Chamedoon.Application.Common.Models;
using Chamedoon.Application.Services.Admin.Common;
using Chamedoon.Application.Services.Admin.Common.Models;
using Chamedoon.Application.Services.Subscription;
using Chamedoon.Domin.Entity.Customers;
using Chamedoon.Domin.Entity.Users;
using Microsoft.AspNetCore.Identity;

namespace Chamedoon.Application.Services.Admin.Users;

public class AdminUserService : IAdminUserService
{
    private readonly IAdminUserRepository _userRepository;
    private readonly IAdminRoleRepository _roleRepository;
    private readonly SubscriptionService _subscriptionService;

    public AdminUserService(
        IAdminUserRepository userRepository,
        IAdminRoleRepository roleRepository,
        SubscriptionService subscriptionService)
    {
        _userRepository = userRepository;
        _roleRepository = roleRepository;
        _subscriptionService = subscriptionService;
    }

    public async Task<OperationResult<PaginatedList<AdminUserDto>>> GetUsersAsync(string? search, long? roleId, int pageNumber, int pageSize, CancellationToken cancellationToken)
    {
        var users = await _userRepository.GetUsersAsync(search, roleId, pageNumber, pageSize, cancellationToken);
        var planTitles = await _subscriptionService.GetPlanTitleLookupAsync(cancellationToken);
        var mappedItems = users.Items.Select(user => user.ToAdminUserDto(planTitles)).ToList();
        var paginated = new PaginatedList<AdminUserDto>(mappedItems, users.TotalCount, users.PageNumber, pageSize);

        return OperationResult<PaginatedList<AdminUserDto>>.Success(paginated);
    }

    public async Task<OperationResult<AdminUserDto>> GetUserAsync(long id, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetUserAsync(id, cancellationToken);
        if (user is null)
        {
            return OperationResult<AdminUserDto>.Fail("کاربر مورد نظر یافت نشد.");
        }

        var planTitles = await _subscriptionService.GetPlanTitleLookupAsync(cancellationToken);
        return OperationResult<AdminUserDto>.Success(user.ToAdminUserDto(planTitles));
    }

    public async Task<OperationResult<AdminUserDto>> CreateUserAsync(AdminUserInput input, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(input.Password))
        {
            return OperationResult<AdminUserDto>.Fail("وارد کردن کلمه عبور الزامی است.");
        }

        var user = BuildUserEntity(input);
        var customer = BuildCustomer(input);

        var result = await _userRepository.CreateUserAsync(user, input.Password!, input.RoleId, customer, cancellationToken);
        if (!result.Result.Succeeded)
        {
            return OperationResult<AdminUserDto>.Fail(BuildIdentityErrors(result.Result));
        }

        var createdUser = await _userRepository.GetUserAsync(result.Entity!.Id, cancellationToken);
        if (createdUser is null)
        {
            return OperationResult<AdminUserDto>.Fail("کاربر ایجاد شد اما اطلاعات آن قابل بازیابی نیست.");
        }

        var planTitles = await _subscriptionService.GetPlanTitleLookupAsync(cancellationToken);
        return OperationResult<AdminUserDto>.Success(createdUser.ToAdminUserDto(planTitles));
    }

    public async Task<OperationResult<AdminUserDto>> UpdateUserAsync(AdminUserInput input, CancellationToken cancellationToken)
    {
        if (!input.Id.HasValue)
        {
            return OperationResult<AdminUserDto>.Fail("شناسه کاربر ارسال نشده است.");
        }

        var user = BuildUserEntity(input);
        user.Id = input.Id.Value;

        var customer = BuildCustomer(input);

        var result = await _userRepository.UpdateUserAsync(user, input.RoleId, input.Password, customer, cancellationToken);
        if (!result.Succeeded)
        {
            return OperationResult<AdminUserDto>.Fail(BuildIdentityErrors(result));
        }

        var updatedUser = await _userRepository.GetUserAsync(input.Id.Value, cancellationToken);
        if (updatedUser is null)
        {
            return OperationResult<AdminUserDto>.Fail("کاربر مورد نظر یافت نشد.");
        }

        var planTitles = await _subscriptionService.GetPlanTitleLookupAsync(cancellationToken);
        return OperationResult<AdminUserDto>.Success(updatedUser.ToAdminUserDto(planTitles));
    }

    public async Task<OperationResult<bool>> DeleteUserAsync(long id, CancellationToken cancellationToken)
    {
        var result = await _userRepository.DeleteUserAsync(id, cancellationToken);
        if (!result.Succeeded)
        {
            return OperationResult<bool>.Fail(BuildIdentityErrors(result));
        }

        return OperationResult<bool>.Success(true);
    }

    public async Task<OperationResult<IReadOnlyList<AdminRoleDto>>> GetRolesAsync(CancellationToken cancellationToken)
    {
        var roles = await _roleRepository.GetRolesAsync(cancellationToken);
        var mapped = roles.Select(role => role.ToAdminRoleDto()).ToList();
        return OperationResult<IReadOnlyList<AdminRoleDto>>.Success(mapped);
    }

    private static User BuildUserEntity(AdminUserInput input)
    {
        var now = DateTime.UtcNow;
        var userName = string.IsNullOrWhiteSpace(input.UserName) ? input.Email : input.UserName;

        return new User
        {
            Email = input.Email,
            UserName = userName,
            PhoneNumber = input.PhoneNumber,
            PhoneNumberConfirmed = !string.IsNullOrWhiteSpace(input.PhoneNumber),
            EmailConfirmed = true,
            Created = input.Id.HasValue ? default : now,
            LastModified = now,
            LockoutEnabled = true,
            LockoutEnd = input.IsActive ? null : DateTimeOffset.UtcNow.AddYears(10)
        };
    }

    private static Customer? BuildCustomer(AdminUserInput input)
    {
        if (string.IsNullOrWhiteSpace(input.FullName) && string.IsNullOrWhiteSpace(input.SubscriptionPlanId))
        {
            return null;
        }

        var parts = (input.FullName ?? string.Empty).Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
        var firstName = parts.Length > 0 ? parts[0] : input.FullName;
        var lastName = parts.Length > 1 ? string.Join(' ', parts.Skip(1)) : string.Empty;

        var startDate = input.SubscriptionStartDateUtc;
        if (!string.IsNullOrWhiteSpace(input.SubscriptionPlanId) && startDate is null)
        {
            startDate = DateTime.UtcNow;
        }

        var endDate = input.SubscriptionEndDateUtc;
        if (!string.IsNullOrWhiteSpace(input.SubscriptionPlanId) && endDate is null)
        {
            endDate = (startDate ?? DateTime.UtcNow).AddMonths(1);
        }

        return new Customer
        {
            FirstName = firstName,
            LastName = lastName,
            SubscriptionPlanId = string.IsNullOrWhiteSpace(input.SubscriptionPlanId) ? null : input.SubscriptionPlanId,
            SubscriptionStartDateUtc = startDate,
            SubscriptionEndDateUtc = endDate,
            UsedEvaluations = input.UsedEvaluations
        };
    }

    private static string BuildIdentityErrors(IdentityResult result)
    {
        if (result.Errors is null)
        {
            return "عملیات با خطا مواجه شد.";
        }

        var builder = new StringBuilder();
        foreach (var error in result.Errors)
        {
            if (builder.Length > 0)
            {
                builder.AppendLine();
            }

            builder.Append(error.Description);
        }

        return builder.ToString();
    }
}
