using Chamedoon.Application.Common.Models;
using Chamedoon.Application.Services.Account.Query;
using Chamedoon.Application.Services.Account.Register.ViewModel;
using Chamedoon.Application.Services.Account.Users.Query;
using Chamedoon.Application.Services.Customers.Command;
using Chamedoon.Application.Services.Email.Query;
using MediatR;
using System;
using System.Net.Mail;

namespace Chamedoon.Application.Services.Account.Register.Command;

public class ManageRegisterUserCommand : IRequest<OperationResult<bool>>
{
    public required RegisterUser_VM RegisterUser { get; set; }
}
public class ManageRegisterUserCommandHandler : IRequestHandler<ManageRegisterUserCommand, OperationResult<bool>>
{
    #region Property
    private readonly IMediator mediator;
    #endregion

    #region Ctor
    public ManageRegisterUserCommandHandler(IMediator mediator)
    {
        this.mediator = mediator;
    }
    #endregion

    #region Method
    public async Task<OperationResult<bool>> Handle(ManageRegisterUserCommand request, CancellationToken cancellationToken)
    {
        var emailValidation = ValidateEmail(request.RegisterUser.Email);
        if (emailValidation.IsSuccess is false)
            return emailValidation;

        //Check Duplicated Email
        var checkEmail = await mediator.Send(new CheckDuplicatedEmailQuery { Email = request.RegisterUser.Email });
        if (checkEmail.IsSuccess is false)
            return OperationResult<bool>.Fail(checkEmail.Message);

        //Register User
        var regisrer = await mediator.Send(new RegisterUserCommand { RegisterUser = request.RegisterUser });
        if (regisrer.IsSuccess is false)
            return OperationResult<bool>.Fail(regisrer.Message);

        var addCustomer = await mediator.Send(new AddCustomerCommand { Id = regisrer.Result});
        if (addCustomer.IsSuccess is false)
            return OperationResult<bool>.Fail(addCustomer.Message);

        return OperationResult<bool>.Success(true);
    }

    private static OperationResult<bool> ValidateEmail(string email)
    {
        if (string.IsNullOrWhiteSpace(email) || IsValidEmailFormat(email) is false)
            return OperationResult<bool>.Fail("ایمیل وارد شده معتبر نمی باشد.");

        if (IsGmailAddress(email) is false)
            return OperationResult<bool>.Fail("ثبت نام تنها با ایمیل های Gmail امکان پذیر است.");

        return OperationResult<bool>.Success(true);
    }

    private static bool IsValidEmailFormat(string email)
    {
        try
        {
            _ = new MailAddress(email);
            return true;
        }
        catch
        {
            return false;
        }
    }

    private static bool IsGmailAddress(string email)
    {
        return email.Trim().EndsWith("@gmail.com", StringComparison.OrdinalIgnoreCase);
    }

    #endregion
}


