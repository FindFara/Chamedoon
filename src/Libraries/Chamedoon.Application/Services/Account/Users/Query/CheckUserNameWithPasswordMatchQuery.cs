using Chamedoon.Application.Common.Models;
using Chamedoon.Application.Services.Account.Login.ViewModel;
using MediatR;

namespace Chamedoon.Application.Services.Account.Users.Query;

public class CheckUserNameAndPasswordMatchQuery : IRequest<OperationResult<bool>>
{
    public required LoginUserViewModel LoginUser { get; set; }
    public required string UserName { get; set; }

}
public class CheckUserNameAndPasswordMatchHandler : IRequestHandler<CheckUserNameAndPasswordMatchQuery, OperationResult<bool>>
{
    #region Method
    public async Task<OperationResult<bool>> Handle(CheckUserNameAndPasswordMatchQuery request, CancellationToken cancellationToken)
    {
        return OperationResult<bool>.Fail("ورود با رمز عبور غیرفعال شده است.");
    }

    #endregion
}
