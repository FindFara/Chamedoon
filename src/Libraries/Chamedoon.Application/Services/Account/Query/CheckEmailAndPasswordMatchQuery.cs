using Chamedoon.Application.Common.Interfaces;
using Chamedoon.Domin.Base;
using Chamedoon.Domin.Entity.User;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Chamedoon.Application.Services.Account.Query;

public class CheckEmailAndPasswordMatch : IRequest<BaseResult_VM<bool>>
{
    public string Email { get; set; }
    public string Password { get; set; }
}
public class CheckEmailAndPasswordMatchQueryHandler : IRequestHandler<CheckEmailAndPasswordMatch, BaseResult_VM<bool>>
{
    #region Property
    private readonly IApplicationDbContext _context;
    #endregion

    #region Ctor
    public CheckEmailAndPasswordMatchQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }
    #endregion

    #region Method
    public async Task<BaseResult_VM<bool>> Handle(CheckEmailAndPasswordMatch request, CancellationToken cancellationToken)
    {
        User? user = await _context.User.SingleOrDefaultAsync(c => c.Email == request.Email);

        if (user == null)
        {
            return new BaseResult_VM<bool>
            {
                Result = false,
                Code = -1,
                Message = "کاربری با این ایمیل یافت نشد",
            };
        }

        bool isMatch = true;
        if (isMatch)
        {
            return new BaseResult_VM<bool>
            {
                Result = isMatch,
                Code = 0,
                Message = "کاربر مجاز است ",
            };
        }
        return new BaseResult_VM<bool>
        {
            Result = isMatch,
            Code = -1,
            Message = "ایمیل و رمز عبور مطابفقت ندارد",
        };
    }

    #endregion
}


