using System.Linq;
using Chamedoon.Application.Common.Models;
using Chamedoon.Domin.Entity.Users;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Chamedoon.Application.Services.Account.Users.Command;

public class EnsureUserRoleCommand : IRequest<OperationResult>
{
    public required long UserId { get; set; }
    public required string RoleName { get; set; }
}

public class EnsureUserRoleCommandHandler : IRequestHandler<EnsureUserRoleCommand, OperationResult>
{
    private readonly UserManager<User> _userManager;

    public EnsureUserRoleCommandHandler(UserManager<User> userManager)
    {
        _userManager = userManager;
    }

    public async Task<OperationResult> Handle(EnsureUserRoleCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByIdAsync(request.UserId.ToString());
        if (user is null)
        {
            return OperationResult.Fail("کاربر یافت نشد.");
        }

        var roleName = request.RoleName?.Trim();
        if (string.IsNullOrWhiteSpace(roleName))
        {
            return OperationResult.Fail("نقش مشخص نشده است.");
        }

        if (await _userManager.IsInRoleAsync(user, roleName))
        {
            return OperationResult.Success();
        }

        var result = await _userManager.AddToRoleAsync(user, roleName);
        if (!result.Succeeded)
        {
            return OperationResult.Fail(result.Errors.Select(e => e.Description).FirstOrDefault() ?? "امکان افزودن نقش وجود ندارد.");
        }

        return OperationResult.Success();
    }
}
