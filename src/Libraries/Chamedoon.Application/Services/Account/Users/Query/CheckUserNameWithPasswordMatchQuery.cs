using Chamedoon.Application.Common.Models;
using Chamedoon.Application.Services.Account.Login.ViewModel;
using Chamedoon.Domin.Entity.Users;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Chamedoon.Application.Services.Account.Users.Query;

public class CheckUserNameAndPasswordMatchQuery : IRequest<OperationResult<bool>>
{
    public required LoginUserViewModel LoginUser { get; set; }
    public required string UserName { get; set; }

}
public class CheckUserNameAndPasswordMatchHandler : IRequestHandler<CheckUserNameAndPasswordMatchQuery, OperationResult<bool>>
{
    #region Property
    private readonly SignInManager<User> signInManager;
    private readonly UserManager<User> userManager;
    #endregion

    #region Ctor
    public CheckUserNameAndPasswordMatchHandler(SignInManager<User> signInManager, UserManager<User> userManager)
    {
        this.signInManager = signInManager;
        this.userManager = userManager;
    }
    #endregion

    #region Method
    public async Task<OperationResult<bool>> Handle(CheckUserNameAndPasswordMatchQuery request, CancellationToken cancellationToken)
    {
        var user = await userManager.FindByNameAsync(request.UserName);
        if (user is null)
            return OperationResult<bool>.Fail("کاربری با این مشخصات یافت نشد.");

        var checkPassword = await signInManager.CheckPasswordSignInAsync(user, request.LoginUser.Password, lockoutOnFailure: false);
        return checkPassword.Succeeded
            ? OperationResult<bool>.Success(true)
            : OperationResult<bool>.Fail("رمز عبور نادرست است.");
    }

    #endregion
}
