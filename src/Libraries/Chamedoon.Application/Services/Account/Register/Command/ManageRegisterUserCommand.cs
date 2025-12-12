using Chamedoon.Application.Common.Models;
using Chamedoon.Application.Services.Account.Register.ViewModel;
using Chamedoon.Application.Services.Account.Users.Query;
using Chamedoon.Application.Services.Customers.Command;
using MediatR;

namespace Chamedoon.Application.Services.Account.Register.Command;

public class ManageRegisterUserCommand : IRequest<OperationResult<long>>
{
    public required RegisterUser_VM RegisterUser { get; set; }
}

public class ManageRegisterUserCommandHandler : IRequestHandler<ManageRegisterUserCommand, OperationResult<long>>
{
    private readonly IMediator _mediator;

    public ManageRegisterUserCommandHandler(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task<OperationResult<long>> Handle(ManageRegisterUserCommand request, CancellationToken cancellationToken)
    {
        var phoneValidation = await _mediator.Send(new ValidateRegistrationPhoneQuery { PhoneNumber = request.RegisterUser.PhoneNumber }, cancellationToken);
        if (phoneValidation.IsSuccess is false)
            return OperationResult<long>.Fail(phoneValidation.Message);

        var register = await _mediator.Send(new RegisterUserCommand { RegisterUser = request.RegisterUser }, cancellationToken);
        if (register.IsSuccess is false || register.Result is 0)
            return OperationResult<long>.Fail(register.Message);

        var addCustomer = await _mediator.Send(new AddCustomerCommand { Id = register.Result }, cancellationToken);
        if (addCustomer.IsSuccess is false)
            return OperationResult<long>.Fail(addCustomer.Message);

        return OperationResult<long>.Success(register.Result);
    }
}
