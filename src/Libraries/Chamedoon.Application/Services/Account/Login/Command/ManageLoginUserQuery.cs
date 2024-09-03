using AutoMapper;
using Chamedoon.Application.Common.Models;
using Chamedoon.Application.Services.Account.Login.ViewModel;
using Chamedoon.Application.Services.Account.Users.Query;
using Chamedoon.Application.Services.Account.Users.ViewModel;
using Chamedoon.Domin.Base;
using MediatR;

namespace Chamedoon.Application.Services.Account.Login.Command;
public class ManageLoginUserQuery : IRequest<OperationResult<UserDetails_VM>>
{
    public required LoginUser_VM LoginUser { get; set; }
}
public class ManageLoginUserQueryHandler : IRequestHandler<ManageLoginUserQuery, OperationResult<UserDetails_VM>>
{
    #region Property
    private readonly IMediator mediator;
    private readonly IMapper mapper;

    #endregion

    #region Ctor
    public ManageLoginUserQueryHandler(IMediator mediator, IMapper mapper)
    {
        this.mediator = mediator;
        this.mapper = mapper;
    }
    #endregion

    #region Method
    public async Task<OperationResult<UserDetails_VM>> Handle(ManageLoginUserQuery request, CancellationToken cancellationToken)
    {
        var user = await mediator.Send(new GetUserQuery { Email = request.LoginUser.Email });
        if (user.IsSuccess is false)
            return OperationResult<UserDetails_VM>.Fail();

        OperationResult<bool> checkUser = await mediator.Send(new CheckUserNameAndPasswordMatchQuery { LoginUser = request.LoginUser, UserName =user.Result.UserName});
        if (checkUser.IsSuccess is false)
            return OperationResult<UserDetails_VM>.Fail(checkUser.Message);

        return OperationResult<UserDetails_VM>.Success(mapper.Map<UserDetails_VM>(user.Result));
    }

    #endregion
}