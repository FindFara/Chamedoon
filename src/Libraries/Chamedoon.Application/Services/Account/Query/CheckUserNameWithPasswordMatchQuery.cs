using Chamedoon.Application.Common.Interfaces;
using Chamedoon.Application.Services.Account.ViewModel;
using Chamedoon.Domin.Base;
using Chamedoon.Domin.Entity.User;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Chamedoon.Application.Services.Account.Query;

public class CheckUserNameAndPasswordMatchQuery : IRequest<BaseResult_VM<bool>>
{
    public LoginUser_VM LoginUser_VM { get; set; }
}
public class CheckUserNameAndPasswordMatchHandler : IRequestHandler<CheckUserNameAndPasswordMatchQuery, BaseResult_VM<bool>>
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
    public async Task<BaseResult_VM<bool>> Handle(CheckUserNameAndPasswordMatchQuery request, CancellationToken cancellationToken)
    {
        var loginUser = await signinmanager.PasswordSignInAsync(
             request.LoginUser_VM.UserName,
             request.LoginUser_VM.Password,
             request.LoginUser_VM.RememberMe,
             true);

        if (loginUser.IsLockedOut)
            return new BaseResult_VM<bool>
            { Code = -1, Message = "اکانت شما به دلیل ورود های ناموفق قفل شده است ، چند دقیقه دیگر دوباره امتحلن کنید", };

        if (loginUser.Succeeded)
            return new BaseResult_VM<bool>
            { Result = true, Code = 0, Message = "ورود با موفقیت انجام شد", };

        return new BaseResult_VM<bool>
        { Code = -2, Message = "مشکلی در ورود رخ داده است ، لطفا چند دقیقه دیگر دوباره امتحان کنید", };
    }

    #endregion
}
