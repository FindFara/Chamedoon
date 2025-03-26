using Chamedoon.Application.Common.Models;
using Chamedoon.Application.Services.Account.Users.ViewModel;
using Chamedoon.Domin.Entity.Users;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Chamedoon.Application.Services.Account.Users.Command;

public class ChangePasswordCommand : IRequest<OperationResult>
{
    public required ChangePasswordViewModel ChangePasswordViewModel { get; set; }
}
public class ChangePasswordCommandHandler : IRequestHandler<ChangePasswordCommand, OperationResult>
{
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;


    public ChangePasswordCommandHandler(UserManager<User> userManager, SignInManager<User> signInManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
    }
    public async Task<OperationResult> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByIdAsync(request.ChangePasswordViewModel.UserId);
        if (user == null)
        {
            return OperationResult.Fail("کاربر یافت نشد.");
        }
        var result = await _userManager.ChangePasswordAsync(
            user, 
            request.ChangePasswordViewModel.CurrentPassword, 
            request.ChangePasswordViewModel.NewPassword);

        if (!result.Succeeded)
            return OperationResult.Fail(result.Errors.Select(e => e.Description).ToArray().First());

        await _signInManager.SignOutAsync();
        return OperationResult.Success();
    }
}
