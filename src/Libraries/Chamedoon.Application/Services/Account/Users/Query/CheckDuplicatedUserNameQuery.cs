using Chamedoon.Application.Common.Interfaces;
using Chamedoon.Application.Common.Models;
using Chamedoon.Domin.Base;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Chamedoon.Application.Services.Account.Users.Query;

public class CheckDuplicatedUserNameQuery : IRequest<OperationResult<bool>>
{
    public string UserName { get; set; }
}
public class CheckDuplicatedUserNameQueryHandler : IRequestHandler<CheckDuplicatedUserNameQuery, OperationResult<bool>>
{
    #region Property
    private readonly IApplicationDbContext _context;
    #endregion

    #region Ctor
    public CheckDuplicatedUserNameQueryHandler(IApplicationDbContext context)
    {
        _context = context;

    }

    #endregion

    #region Method
    public async Task<OperationResult<bool>> Handle(CheckDuplicatedUserNameQuery request, CancellationToken cancellationToken)
    {
        bool isDuplicated = await _context.User.AnyAsync(u => u.UserName == request.UserName);
        if (isDuplicated)
        {
            return OperationResult<bool>.Fail("این نام کاربری تکراری است", false);
        }

        return OperationResult<bool>.Success(true);

    }

    #endregion
}


