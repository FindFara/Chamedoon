using Chamedoon.Application.Common.Models;
using Chamedoon.Application.Services.Account.Users.Query;
using Chamedoon.Domin.Entity.User;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Chamedoon.Application.Services.Account.Query;


public class GetUserRolesQuery : IRequest<OperationResult<IList<string>>>
{
    public long? Id { get; set; }
    public string? UserName { get; set; }
}
public class GetUserRolesQueryHandler : IRequestHandler<GetUserRolesQuery, OperationResult<IList<string>>>
{
    #region Property
    private readonly UserManager<User> userManager;
    private readonly IMediator mediator;
    #endregion

    #region Ctor
    public GetUserRolesQueryHandler(UserManager<User> userManager, IMediator mediator)
    {
        this.userManager = userManager;
        this.mediator = mediator;
    }
    #endregion

    #region Method
    public async Task<OperationResult<IList<string>>> Handle(GetUserRolesQuery request, CancellationToken cancellationToken)
    {
        var user = await mediator.Send(new GetUserQuery { UserName = request.UserName, Id = request.Id });
        if (user.IsSuccess is false)
            return OperationResult<IList<string>>.Fail();

        IList<string>? roles = await userManager.GetRolesAsync(user.Result);
        if (roles is null || roles.Count is 0)
            return OperationResult<IList<string>>.Fail("برای کاربر نقشی یافت نشد");

        return OperationResult<IList<string>>.Success(roles);
    }

    #endregion
}
