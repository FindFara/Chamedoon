using AutoMapper;
using Chamedoon.Application.Services.Account.ViewModel;
using Chamedoon.Domin.Base;
using Chamedoon.Domin.Entity.User;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Chamedoon.Application.Services.Account.Command;

public class RegisterUserCommand : IRequest<BaseResult_VM<bool>>
{
    public RegisterUser_VM RegisterUser_VM { get; set; }
}
public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, BaseResult_VM<bool>>
{
    #region Property
    private readonly IMapper mapper;
    private readonly UserManager<User> userManager;
    #endregion

    #region Ctor
    public RegisterUserCommandHandler(IMapper mapper, UserManager<User> userManager)
    {
        this.mapper = mapper;
        this.userManager = userManager;
    }
    #endregion

    #region Method
    public async Task<BaseResult_VM<bool>> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        try
        {
            User user = mapper.Map<User>(request.RegisterUser_VM);
            var registerUser = await userManager.CreateAsync(user, request.RegisterUser_VM.Password);

            if (registerUser.Succeeded)
            {
                return new BaseResult_VM<bool>
                {
                    Result = true,
                    Code = 0,
                    Message = "ثبت نام با موفقیت انجام شد"
                };
            }

            if (registerUser.Errors.Count() != 0)
            {
                var errors = registerUser.Errors.Select(e => e.Description).ToList();

                return new BaseResult_VM<bool>
                {
                    Result = false,
                    Code = -1,
                    Message = string.Join(" , ", errors)
                };
            }
        }

        catch (Exception ex)
        {
            var m = ex.Message;
        }

        return new BaseResult_VM<bool>
        {
            Result = true,
            Code = 0,
            Message = "ثبت نام با موفقیت انجام شد"

        };
    }
    #endregion
}
