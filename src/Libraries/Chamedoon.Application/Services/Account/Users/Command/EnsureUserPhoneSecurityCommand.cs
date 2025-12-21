using Chamedoon.Application.Common.Models;
using Chamedoon.Application.Common.Utilities;
using Chamedoon.Domin.Entity.Users;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System.Linq;

namespace Chamedoon.Application.Services.Account.Users.Command;

public class EnsureUserPhoneSecurityCommand : IRequest<OperationResult>
{
    public required long UserId { get; set; }
    public required string PhoneNumber { get; set; }
}

public class EnsureUserPhoneSecurityCommandHandler : IRequestHandler<EnsureUserPhoneSecurityCommand, OperationResult>
{
    private readonly UserManager<User> _userManager;

    public EnsureUserPhoneSecurityCommandHandler(UserManager<User> userManager)
    {
        _userManager = userManager;
    }

    public async Task<OperationResult> Handle(EnsureUserPhoneSecurityCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByIdAsync(request.UserId.ToString());
        if (user is null)
        {
            return OperationResult.Fail("کاربر یافت نشد.");
        }

        var normalizedPhone = PhoneNumberHelper.Normalize(request.PhoneNumber);
        if (normalizedPhone is null)
        {
            return OperationResult.Fail("شماره موبایل معتبر نیست.");
        }

        var requiresUpdate = false;

        if (!string.Equals(user.PhoneNumber, normalizedPhone, StringComparison.Ordinal))
        {
            user.PhoneNumber = normalizedPhone;
            requiresUpdate = true;
        }

        if (user.PhoneNumberConfirmed is false)
        {
            user.PhoneNumberConfirmed = true;
            requiresUpdate = true;
        }

        if (user.TwoFactorEnabled is false)
        {
            user.TwoFactorEnabled = true;
            requiresUpdate = true;
        }

        if (requiresUpdate)
        {
            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                return OperationResult.Fail(result.Errors.Select(e => e.Description).FirstOrDefault() ?? "امکان بروزرسانی کاربر نیست.");
            }
        }

        return OperationResult.Success();
    }
}
