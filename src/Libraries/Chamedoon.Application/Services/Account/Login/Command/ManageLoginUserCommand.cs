using AutoMapper;
using Chamedoon.Application.Common.Models;
using Chamedoon.Application.Services.Account.Login.ViewModel;
using Chamedoon.Application.Services.Account.Users.ViewModel;
using Chamedoon.Domin.Entity.Users;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Chamedoon.Application.Services.Account.Login.Command;
public class ManageLoginUserCommand : IRequest<OperationResult<UserDetails_VM>>
{
    public required LoginUserViewModel LoginUser { get; set; }
}
public class ManageLoginUserQueryHandler : IRequestHandler<ManageLoginUserCommand, OperationResult<UserDetails_VM>>
{
    #region Property
    private readonly UserManager<User> userManager;
    private readonly SignInManager<User> signInManager;
    private readonly IMapper mapper;
    #endregion

    #region Ctor
    public ManageLoginUserQueryHandler(UserManager<User> userManager, SignInManager<User> signInManager, IMapper mapper)
    {
        this.userManager = userManager;
        this.signInManager = signInManager;
        this.mapper = mapper;
    }
    #endregion

    #region Method
    public async Task<OperationResult<UserDetails_VM>> Handle(ManageLoginUserCommand request, CancellationToken cancellationToken)
    {
        var identifier = request.LoginUser.UserNameOrEmail?.Trim();
        if (string.IsNullOrWhiteSpace(identifier))
            return OperationResult<UserDetails_VM>.Fail("نام کاربری یا ایمیل وارد نشده است.");

        var user = await userManager.FindByNameAsync(identifier) ?? await userManager.FindByEmailAsync(identifier);
        if (user is null)
            return OperationResult<UserDetails_VM>.Fail("کاربری با این مشخصات یافت نشد.");

        var passwordCheck = await signInManager.CheckPasswordSignInAsync(user, request.LoginUser.Password, lockoutOnFailure: false);
        if (!passwordCheck.Succeeded)
            return OperationResult<UserDetails_VM>.Fail("رمز عبور نادرست است.");

        return OperationResult<UserDetails_VM>.Success(mapper.Map<UserDetails_VM>(user));
    }

    #endregion
}
