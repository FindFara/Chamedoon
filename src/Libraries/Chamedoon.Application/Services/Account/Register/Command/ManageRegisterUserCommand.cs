using Chamedoon.Application.Common.Models;
using Chamedoon.Application.Services.Account.Register.ViewModel;
using Chamedoon.Application.Services.Account.Login.Command;
using Chamedoon.Application.Services.Account.Users.Query;
using Chamedoon.Application.Services.Customers.Command;
using MediatR;

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
        var emailValidation = await mediator.Send(new ValidateRegistrationEmailQuery { Email = request.RegisterUser.Email }, cancellationToken);
        if (emailValidation.IsSuccess is false)
            return emailValidation;

        //Register User
        var regisrer = await mediator.Send(new RegisterUserCommand { RegisterUser = request.RegisterUser }, cancellationToken);
        if (regisrer.IsSuccess is false)
            return OperationResult<bool>.Fail(regisrer.Message);

        var addCustomer = await mediator.Send(new AddCustomerCommand { Id = regisrer.Result }, cancellationToken);
        if (addCustomer.IsSuccess is false)
            return OperationResult<bool>.Fail(addCustomer.Message);

        var signIn = await mediator.Send(new SignInUserCommand
        {
            UserId = regisrer.Result.ToString(),
            IsPersistent = false
        }, cancellationToken);
        if (signIn.IsSuccess is false)
            return OperationResult<bool>.Fail(signIn.Message);

        return OperationResult<bool>.Success(true);
    }

    #endregion
}
