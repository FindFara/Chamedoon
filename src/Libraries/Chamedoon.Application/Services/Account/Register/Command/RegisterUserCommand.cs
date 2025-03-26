using AutoMapper;
using Chamedoon.Application.Common.Extensions;
using Chamedoon.Application.Common.Models;
using Chamedoon.Application.Services.Account.Register.ViewModel;
using Chamedoon.Application.Services.Email.Query;
using Chamedoon.Domin.Entity.Users;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Chamedoon.Application.Services.Account.Register.Command;

public class RegisterUserCommand : IRequest<OperationResult<long>>
{
    public required RegisterUser_VM RegisterUser { get; set; }
}
public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, OperationResult<long>>
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
    public async Task<OperationResult<long>> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        User user = mapper.Map<User>(request.RegisterUser);
        user.UserName = string.Concat("U-",StringExtensions.GenerateRandomString(8));
        var registerUser = await userManager.CreateAsync(user, request.RegisterUser.Password);
        if (registerUser.Succeeded)
        {
            //Send Token
            await userManager.AddToRoleAsync(user, "Member");
            var tokenUser = await mediator.Send(new SendTokenToUserEmailQuery { User = user });

            return OperationResult<long>.Success(user.Id);
        }
        return OperationResult<long>.Fail(registerUser.Errors.Select(e => e.Description).First());
    }
    #endregion
}
