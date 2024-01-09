using Chamedoon.Application.Common.Interfaces;
using Chamedoon.Domin.Base;
using MediatR;
using Microsoft.EntityFrameworkCore;


namespace Chamedoon.Application.Services.Account.Query;

public class CheckDuplicatedEmailQuery : IRequest<BaseResult_VM<bool>>
{
    public string Email { get; set; }
}
public class CheckDuplicatedEmailQueryHandler : IRequestHandler<CheckDuplicatedEmailQuery, BaseResult_VM<bool>>
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
    public async Task<BaseResult_VM<bool>> Handle(CheckDuplicatedEmailQuery request, CancellationToken cancellationToken)
    {
        bool isDuplicated = await _context.User.AnyAsync(u => u.Email == request.Email);
        if (isDuplicated)
        {
            return new BaseResult_VM<bool>
            {
                Result = false,
                Code = 0,
                Message = "کاربر با این ایمیل وجود دارد ",
            };
        }

        return new BaseResult_VM<bool>
        {
            Result = true,
            Code = -1,
            Message = "کاربر با این ایمیل وجود ندارد"
        };
    }

    #endregion
}


