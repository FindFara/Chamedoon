using AutoMapper;
using Chamedoon.Application.Services.Account.Register.ViewModel;
using Chamedoon.Application.Services.Email.Query;
using Chamedoon.Domin.Entity.User;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Chamedoon.Application.Services.Account.Register.Command;

public class RegisterUserCommand : IRequest<ResponseRegisterUser_VM>
{
    public required RegisterUser_VM RegisterUser { get; set; }
}
public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, ResponseRegisterUser_VM>
{
    #region Property
    private readonly IMapper mapper;
    private readonly UserManager<User> userManager;
    private readonly IMediator mediator;
    #endregion

    #region Ctor
    public RegisterUserCommandHandler(IMapper mapper, UserManager<User> userManager, IMediator mediator)
    {
        this.mapper = mapper;
        this.userManager = userManager;
        this.mediator = mediator;
    }
    #endregion

    #region Method
    public async Task<ResponseRegisterUser_VM> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        User user = mapper.Map<User>(request.RegisterUser);
        var registerUser = await userManager.CreateAsync(user, request.RegisterUser.Password);


        if (registerUser.Succeeded)
        {
            //Send Token
            await userManager.AddToRoleAsync(user, "Member");
            var tokenUser = await mediator.Send(new SendTokenToUserEmailQuery { User = user });

            return new ResponseRegisterUser_VM
            {
                Code = 0,
                Message = "ثبت نام با موفقیت انجام شد"
            };
        }

        return new ResponseRegisterUser_VM
        {
            Code = -1,
            Message = "ثبت نام با خطا مواجه شد",
            Errors = registerUser.Errors.Select(e => e.Description)
        };
    }
    #endregion
}
