using Chamedoon.Application.Common.Models;
using Chamedoon.Application.Services.Account.Register.ViewModel;
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
        //Check Duplicated Email
        var checkEmail = await mediator.Send(new CheckDuplicatedEmailQuery { Email = request.RegisterUser.Email });
        if (checkEmail.IsSuccess is false)
            return OperationResult<bool>.Fail(checkEmail.Message);

        //Register User
        var regisrer = await mediator.Send(new RegisterUserCommand { RegisterUser = request.RegisterUser });
        if (regisrer.IsSuccess is false)
            return OperationResult<bool>.Fail(regisrer.Message);

        var addCustomer = await mediator.Send(new AddCustomerCommand { Id = regisrer.Result });
        if (addCustomer.IsSuccess is false)
            return OperationResult<bool>.Fail(addCustomer.Message);

        return OperationResult<bool>.Success(true);
    }

    #endregion
}


