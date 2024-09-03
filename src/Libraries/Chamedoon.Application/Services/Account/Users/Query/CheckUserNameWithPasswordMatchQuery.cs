using Chamedoon.Application.Common.Interfaces;
using Chamedoon.Application.Common.Models;
using Chamedoon.Application.Services.Account.Login.ViewModel;
using Chamedoon.Domin.Base;
using Chamedoon.Domin.Entity.Users;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Chamedoon.Application.Services.Account.Users.Query;

public class CheckUserNameAndPasswordMatchQuery : IRequest<OperationResult<bool>>
{
    public required LoginUser_VM LoginUser { get; set; }
}
public class CheckUserNameAndPasswordMatchHandler : IRequestHandler<CheckUserNameAndPasswordMatchQuery, OperationResult<bool>>
{
    #region Property
    private readonly IApplicationDbContext context;
    private readonly SignInManager<User> signinmanager;
    #endregion

    #region Ctor
    public CheckUserNameAndPasswordMatchHandler(IApplicationDbContext context, SignInManager<User> signinmanager)
    {
        this.context = context;
        this.signinmanager = signinmanager;
    }
    #endregion

    #region Method
    public async Task<OperationResult<bool>> Handle(CheckUserNameAndPasswordMatchQuery request, CancellationToken cancellationToken)
    {
        var loginUser = await signinmanager.PasswordSignInAsync(
             request.LoginUser.Email,
             request.LoginUser.Password,
             request.LoginUser.RememberMe,
             true);

        if (loginUser.IsLockedOut)
            return OperationResult<bool>
                .Fail("اکانت شما به دلیل ورود های ناموفق قفل شده است ، چند دقیقه دیگر دوباره امتحان کنید");

        if (loginUser.Succeeded)
            return OperationResult<bool>.Success(true);

        return OperationResult<bool>.Fail("مشکلی در ورود رخ داده است ، لطفا چند دقیقه دیگر دوباره امتحان کنید");
    }

    #endregion
}
