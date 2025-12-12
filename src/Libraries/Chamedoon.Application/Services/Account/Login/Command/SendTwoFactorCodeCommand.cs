using Chamedoon.Application.Common.Models;
using Chamedoon.Application.Services.Account.Users.Query;
using Chamedoon.Application.Services.Sms;
using Chamedoon.Domin.Entity.Users;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Chamedoon.Application.Services.Account.Login.Command;

public class SendTwoFactorCodeCommand : IRequest<OperationResult>
{
    public required long UserId { get; set; }
}

public class SendTwoFactorCodeCommandHandler : IRequestHandler<SendTwoFactorCodeCommand, OperationResult>
{
    private readonly UserManager<User> _userManager;
    private readonly ISmsService _smsService;
    private readonly IMediator _mediator;

    public SendTwoFactorCodeCommandHandler(UserManager<User> userManager, ISmsService smsService, IMediator mediator)
    {
        _userManager = userManager;
        _smsService = smsService;
        _mediator = mediator;
    }

    public async Task<OperationResult> Handle(SendTwoFactorCodeCommand request, CancellationToken cancellationToken)
    {
        var user = await _mediator.Send(new GetUserQuery { Id = request.UserId }, cancellationToken);
        if (user.IsSuccess is false || user.Result is null)
            return OperationResult.Fail(user.Message);

        if (string.IsNullOrWhiteSpace(user.Result.PhoneNumber))
            return OperationResult.Fail("شماره موبایل برای این حساب ثبت نشده است.");

        if (!user.Result.TwoFactorEnabled)
        {
            user.Result.TwoFactorEnabled = true;
            await _userManager.UpdateAsync(user.Result);
        }

        var token = await _userManager.GenerateTwoFactorTokenAsync(user.Result, TokenOptions.DefaultPhoneProvider);
        await _smsService.SendVerificationCodeAsync(user.Result.PhoneNumber, token, cancellationToken);

        return OperationResult.Success("کد تایید برای شما پیامک شد.");
    }
}
