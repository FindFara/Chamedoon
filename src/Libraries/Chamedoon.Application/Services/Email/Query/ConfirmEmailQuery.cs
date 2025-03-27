using Chamedoon.Application.Common.Models;
using Chamedoon.Application.Services.Account.Users.Query;
using Chamedoon.Domin.Entity.Users;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using System.Text;

namespace Chamedoon.Application.Services.Email.Query;
public class ConfirmEmailQuery : IRequest<OperationResult<bool>>
{
    public required string UserId { get; set; }
    public required string Token { get; set; }
}
public class ConfirmEmailCommandHandler : IRequestHandler<ConfirmEmailQuery, OperationResult<bool>>
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
    public async Task<OperationResult<bool>> Handle(ConfirmEmailQuery request, CancellationToken cancellationToken)
    {
        var user = userManager.FindByIdAsync(request.UserId);

        var decodedToken = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(request.Token));

        var confirmEmail = await userManager.ConfirmEmailAsync(user.Result, decodedToken);
        if (confirmEmail.Succeeded)
            return OperationResult<bool>.Success(true);

        return OperationResult<bool>.Fail("تایید ایمیل با مشکل مواجه شد", false);
    }
    #endregion
}

