using Chamedoon.Application.Common.Models;
using Chamedoon.Application.Services.Account.Login.ViewModel;
using Chamedoon.Application.Services.Account.Users.Query;
using Chamedoon.Application.Services.Account.Users.ViewModel;
using MediatR;

namespace Chamedoon.Application.Services.Account.Login.Command;
public class ManageLoginUserCommand : IRequest<OperationResult<UserDetails_VM>>
{
    public required LoginUserViewModel LoginUser { get; set; }
}
public class ManageLoginUserQueryHandler : IRequestHandler<ManageLoginUserCommand, OperationResult<UserDetails_VM>>
{
    #region Property
    private readonly IMediator mediator;

    #endregion

    #region Ctor
    public ManageLoginUserQueryHandler(IMediator mediator)
    {
        this.mediator = mediator;
    }
    #endregion

    #region Method
    public async Task<OperationResult<UserDetails_VM>> Handle(ManageLoginUserCommand request, CancellationToken cancellationToken)
    {
        var user = await mediator.Send(new GetUserQuery { Email = request.LoginUser.Email });
        if (user.IsSuccess is false)
            return OperationResult<UserDetails_VM>.Fail();

        return OperationResult<UserDetails_VM>.Fail("ورود با رمز عبور غیرفعال شده است. لطفاً از کد تایید استفاده کنید.");
    }

    #endregion
}