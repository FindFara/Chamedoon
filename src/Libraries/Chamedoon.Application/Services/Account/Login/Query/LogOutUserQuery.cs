using Chamedoon.Application.Common.Models;
using Chamedoon.Domin.Base;
using Chamedoon.Domin.Entity.Users;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Chamedoon.Application.Services.Account.Login.Query;

public class LogOutUserQuery : IRequest<OperationResult<bool>>
{
}
public class LogOutUserQueryHandler : IRequestHandler<LogOutUserQuery, OperationResult<bool>>
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
    public async Task<OperationResult<bool>> Handle(LogOutUserQuery request, CancellationToken cancellationToken)
    {
        await signinmanager.SignOutAsync();
        return OperationResult<bool>.Success(true);
    }

    #endregion
}