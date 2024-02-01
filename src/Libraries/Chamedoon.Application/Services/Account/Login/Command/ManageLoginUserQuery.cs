using AutoMapper;
using Chamedoon.Application.Services.Account.Login.ViewModel;
using Chamedoon.Application.Services.Account.Users.Query;
using Chamedoon.Application.Services.Account.Users.ViewModel;
using Chamedoon.Domin.Base;
using MediatR;

namespace Chamedoon.Application.Services.Account.Login.Command;
public class ManageLoginUserQuery : IRequest<BaseResult_VM<UserDetails_VM>>
{
    public required LoginUser_VM LoginUser { get; set; }
}
public class ManageLoginUserQueryHandler : IRequestHandler<ManageLoginUserQuery, BaseResult_VM<UserDetails_VM>>
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
    public async Task<BaseResult_VM<UserDetails_VM>> Handle(ManageLoginUserQuery request, CancellationToken cancellationToken)
    {
        var user = await mediator.Send(new GetUserQuery { UserName = request.LoginUser.UserName });
        if (user.Result is null)
            return new BaseResult_VM<UserDetails_VM> { Code = user.Code, Message = user.Message };

        var checkUser = await mediator.Send(new CheckUserNameAndPasswordMatchQuery { LoginUser = request.LoginUser });
        if (checkUser.Code is not 0)
            return new BaseResult_VM<UserDetails_VM> { Code = checkUser.Code, Message = checkUser.Message };

        return new BaseResult_VM<UserDetails_VM>
        {
            Result = mapper.Map<UserDetails_VM>(user.Result),
            Code = 0,
            Message = "ورود با موفقیت انجام شد",
        };
    }

    #endregion
}