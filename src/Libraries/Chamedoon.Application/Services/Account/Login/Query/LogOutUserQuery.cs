using Chamedoon.Domin.Base;
using Chamedoon.Domin.Entity.User;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Chamedoon.Application.Services.Account.Login.Query;

public class LogOutUserQuery : IRequest<BaseResult_VM<bool>>
{
}
public class LogOutUserQueryHandler : IRequestHandler<LogOutUserQuery, BaseResult_VM<bool>>
{
    #region Property    
    private readonly SignInManager<User> signinmanager;
    #endregion

    #region Ctor
    public LogOutUserQueryHandler(SignInManager<User> signinmanager)
    {
        this.signinmanager = signinmanager;
    }
    #endregion

    #region Method
    public async Task<BaseResult_VM<bool>> Handle(LogOutUserQuery request, CancellationToken cancellationToken)
    {
        await signinmanager.SignOutAsync();
        return new BaseResult_VM<bool>
        {
            Result = true,
            Code = 0,
            Message = "عملیات با موفقیت انجام شد"
        };
    }

    #endregion
}