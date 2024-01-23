using Chamedoon.Application.Services.Account.ViewModel;
using Chamedoon.Domin.Base;
using MediatR;

namespace Chamedoon.Application.Services.Account.Query;
public class ManageLoginUserQuery : IRequest<BaseResult_VM<bool>>
{
    public required LoginUser_VM LoginUser { get; set; }
}
public class ManageLoginUserQueryHandler : IRequestHandler<ManageLoginUserQuery, BaseResult_VM<bool>>
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
    public async Task<BaseResult_VM<bool>> Handle(ManageLoginUserQuery request, CancellationToken cancellationToken)
    {
        var checkUser = await mediator.Send(new CheckUserNameAndPasswordMatchQuery { LoginUser = request.LoginUser });
        if (checkUser.Code is not 0)
            return checkUser;

        return new BaseResult_VM<bool>
        {
            Result = true,
            Code = 0,
            Message = "ورود با موفقیت انجام شد",
        };
    }

    #endregion
}