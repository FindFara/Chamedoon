using Chamedoon.Application.Common.Interfaces;
using Chamedoon.Domin.Base;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Chamedoon.Application.Services.Account.Query;

public class CheckDuplicatedUserNameQuery : IRequest<BaseResult_VM<bool>>
{
    public string UserName { get; set; }
}
public class CheckDuplicatedUserNameQueryHandler : IRequestHandler<CheckDuplicatedUserNameQuery, BaseResult_VM<bool>>
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
    public async Task<BaseResult_VM<bool>> Handle(CheckDuplicatedUserNameQuery request, CancellationToken cancellationToken)
    {
        bool isDuplicated = await _context.User.AnyAsync(u => u.UserName == request.UserName);
        if (isDuplicated)
        {
            return new BaseResult_VM<bool>
            {
                Result = false,
                Code = -1,
                Message = "این نام کاربری تکراری است",
            };
        }

        return new BaseResult_VM<bool>
        {
            Result = true,
            Code = 0,
        };
    }

    #endregion
}


