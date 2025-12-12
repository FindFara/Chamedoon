using Chamedoon.Application.Common.Models;
using Chamedoon.Application.Services.Account.Login.ViewModel;
using Chamedoon.Application.Services.Account.Users.Query;
using MediatR;

namespace Chamedoon.Application.Services.Account.Login.Command;

public class ManageLoginUserCommand : IRequest<OperationResult<long>>
{
    public required LoginUserViewModel LoginUser { get; set; }
}

public class ManageLoginUserQueryHandler : IRequestHandler<ManageLoginUserCommand, OperationResult<long>>
{
    private readonly IMediator _mediator;

    public ManageLoginUserQueryHandler(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task<OperationResult<long>> Handle(ManageLoginUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _mediator.Send(new GetUserQuery { PhoneNumber = request.LoginUser.PhoneNumber }, cancellationToken);
        if (user.IsSuccess is false || user.Result is null)
            return OperationResult<long>.Fail(user.Message);

        var checkUser = await _mediator.Send(new CheckUserNameAndPasswordMatchQuery
        {
            LoginUser = request.LoginUser,
            UserName = user.Result.UserName
        }, cancellationToken);

        if (checkUser.IsSuccess is false)
            return OperationResult<long>.Fail(checkUser.Message);

        return OperationResult<long>.Success(user.Result.Id);
    }
}
