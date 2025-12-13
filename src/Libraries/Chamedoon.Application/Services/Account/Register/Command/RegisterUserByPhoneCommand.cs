using System;
using System.Linq;
using Chamedoon.Application.Common.Models;
using Chamedoon.Application.Common.Utilities;
using Chamedoon.Domin.Entity.Users;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Chamedoon.Application.Services.Account.Register.Command;

public class RegisterUserByPhoneCommand : IRequest<OperationResult<long>>
{
    public required string PhoneNumber { get; set; }
}

public class RegisterUserByPhoneCommandHandler : IRequestHandler<RegisterUserByPhoneCommand, OperationResult<long>>
{
    private readonly UserManager<User> _userManager;

    public RegisterUserByPhoneCommandHandler(UserManager<User> userManager)
    {
        _userManager = userManager;
    }

    public async Task<OperationResult<long>> Handle(RegisterUserByPhoneCommand request, CancellationToken cancellationToken)
    {
        var normalizedPhone = PhoneNumberHelper.Normalize(request.PhoneNumber);
        if (normalizedPhone is null)
        {
            return OperationResult<long>.Fail("شماره موبایل وارد شده معتبر نیست.");
        }

        var user = new User
        {
            PhoneNumber = normalizedPhone,
            PhoneNumberConfirmed = true,
            TwoFactorEnabled = true,
            UserName = await UsernameGenerator.GenerateUniqueAsync(_userManager, cancellationToken),
            Email = $"{normalizedPhone}@chamedoon.local",
            Created = DateTime.UtcNow,
        };

        var result = await _userManager.CreateAsync(user);
        if (result.Succeeded)
        {
            await _userManager.AddToRoleAsync(user, "Member");
            return OperationResult<long>.Success(user.Id);
        }

        return OperationResult<long>.Fail(result.Errors.Select(e => e.Description).FirstOrDefault() ?? "امکان ثبت نام وجود ندارد.");
    }

}
