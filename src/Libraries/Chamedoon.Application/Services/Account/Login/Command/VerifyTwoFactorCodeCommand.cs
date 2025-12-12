using Chamedoon.Application.Common.Models;
using Chamedoon.Application.Services.Account.Login.ViewModel;
using Chamedoon.Application.Services.Account.Users.Query;
using Chamedoon.Domin.Entity.Users;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Chamedoon.Application.Services.Account.Login.Command;

public class VerifyTwoFactorCodeCommand : IRequest<OperationResult>
{
    public required VerifyTwoFactorCodeViewModel Verification { get; set; }
}

public class VerifyTwoFactorCodeCommandHandler : IRequestHandler<VerifyTwoFactorCodeCommand, OperationResult>
{
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;
    private readonly IMediator _mediator;

    public VerifyTwoFactorCodeCommandHandler(UserManager<User> userManager, SignInManager<User> signInManager, IMediator mediator)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _mediator = mediator;
    }

    public async Task<OperationResult> Handle(VerifyTwoFactorCodeCommand request, CancellationToken cancellationToken)
    {
        var user = await _mediator.Send(new GetUserQuery { Id = request.Verification.UserId }, cancellationToken);
        if (user.IsSuccess is false || user.Result is null)
            return OperationResult.Fail(user.Message);

        var isValid = await _userManager.VerifyTwoFactorTokenAsync(user.Result, TokenOptions.DefaultPhoneProvider, request.Verification.Code);
        if (!isValid)
            return OperationResult.Fail("کد وارد شده صحیح نیست یا منقضی شده است.");

        await _signInManager.SignInAsync(user.Result, request.Verification.RememberMe);
        return OperationResult.Success();
    }
}
