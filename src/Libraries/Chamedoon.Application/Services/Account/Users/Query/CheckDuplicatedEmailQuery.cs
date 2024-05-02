using Chamedoon.Application.Common.Interfaces;
using Chamedoon.Application.Common.Models;
using Chamedoon.Domin.Base;
using MediatR;
using Microsoft.EntityFrameworkCore;


namespace Chamedoon.Application.Services.Account.Users.Query;

public class CheckDuplicatedEmailQuery : IRequest<OperationResult<bool>>
{
    public string Email { get; set; }
}
public class CheckDuplicatedEmailQueryHandler : IRequestHandler<CheckDuplicatedEmailQuery, OperationResult<bool>>
{
    #region Property
    private readonly IApplicationDbContext _context;
    #endregion

    #region Ctor
    public CheckDuplicatedEmailQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }
    #endregion

    #region Method
    public async Task<OperationResult<bool>> Handle(CheckDuplicatedEmailQuery request, CancellationToken cancellationToken)
    {
        bool isDuplicated = await _context.User.AnyAsync(u => u.Email == request.Email);
        if (isDuplicated)
        {
            return OperationResult<bool>.Fail("در حال حاظر کاربری با این ایمیل وجود دارد ", false);

        }

        return OperationResult<bool>.Success(true);
    }

    #endregion
}


