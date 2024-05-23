using Chamedoon.Application.Common.Interfaces;
using Chamedoon.Application.Common.Models;
using Chamedoon.Application.Services.Account.Query;
using Chamedoon.Application.Services.Account.Users.Query;
using Chamedoon.Domin.Base;
using Chamedoon.Domin.Entity.Users;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chamedoon.Application.Services.Email.Query;
public class ConfirmEmailCommand : IRequest<OperationResult<bool>>
{
    public long UserId { get; set; }
    public string Token { get; set; }
}
public class ConfirmEmailCommandHandler : IRequestHandler<ConfirmEmailCommand, OperationResult<bool>>
{
    #region Property
    private readonly UserManager<User> userManager;
    private readonly IMediator mediator;
    #endregion

    #region Ctor
    public ConfirmEmailCommandHandler(UserManager<User> userManager, IMediator mediator)
    {
        this.userManager = userManager;
        this.mediator = mediator;
    }
    #endregion

    #region Method
    public async Task<OperationResult<bool>> Handle(ConfirmEmailCommand request, CancellationToken cancellationToken)
    {
        var user = await mediator.Send(new GetUserQuery { Id = request.UserId });

        var decodedTocken = WebEncoders.Base64UrlDecode(request.Token);
        var normalToken = Encoding.UTF8.GetString(decodedTocken);

        var confirmEmail = await userManager.ConfirmEmailAsync(user.Result, normalToken);
        if (confirmEmail.Succeeded)
        {
            return OperationResult<bool>.Success(true);
        }

        return OperationResult<bool>.Fail("تایید ایمیل با مشکل مواجه شد" , false);
    }
    #endregion
}

