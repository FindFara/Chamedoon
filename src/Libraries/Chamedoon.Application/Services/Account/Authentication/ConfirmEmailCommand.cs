using Chamedoon.Application.Common.Interfaces;
using Chamedoon.Application.Services.Account.Query;
using Chamedoon.Domin.Base;
using Chamedoon.Domin.Entity.User;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chamedoon.Application.Services.Account.Authentication;
public class ConfirmEmailCommand : IRequest<BaseResult_VM<bool>>
{
    public long UserId { get; set; }
    public string Token { get; set; }
}
public class ConfirmEmailCommandHandler : IRequestHandler<ConfirmEmailCommand, BaseResult_VM<bool>>
{
    #region Property
    private readonly UserManager<User> userManager;
    private readonly IMediator mediator;
    #endregion

    #region Ctor
    public ConfirmEmailCommandHandler( UserManager<User> userManager , IMediator mediator)
    {
        this.userManager = userManager;
        this.mediator = mediator;
    }
    #endregion

    #region Method
    public async Task<BaseResult_VM<bool>> Handle(ConfirmEmailCommand request, CancellationToken cancellationToken)
    {
        var user = await mediator.Send(new GetUserByIdQuery { Id = request.UserId});

        var decodedTocken = WebEncoders.Base64UrlDecode(request.Token);
        var normalToken =Encoding.UTF8.GetString(decodedTocken);

       var confirmEmail =await userManager.ConfirmEmailAsync(user.Result, normalToken);
        if (confirmEmail.Succeeded)
        {
            return new BaseResult_VM<bool>
            {
                Result = true,
                Code = 0,
                Message = "تایید ایمیل با موفقیت انجام شد",

            };
        }

        return new BaseResult_VM<bool>
        {
            Result = false,
            Code = -1,
            Message = "تایید ایمیل با مشکل مواجه شد",

        };
    }
    #endregion
}

