using Chamedoon.Application.Common.Interfaces;
using Chamedoon.Domin.Base;
using Chamedoon.Domin.Entity.User;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Chamedoon.Application.Services.Admin.UserManagement.Command;

public class BlockUserCommand : IRequest<BaseResult_VM<bool>>
{
    public long UserId { get; set; }
    public DateTime? LockOutTime { get; set; }
}
public class BlockUserCommandHandler : IRequestHandler<BlockUserCommand, BaseResult_VM<bool>>
{
    #region Property
    private readonly IApplicationDbContext _context;
    #endregion

    #region Ctor
    public BlockUserCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }
    #endregion

    #region Method
    public async Task<BaseResult_VM<bool>> Handle(BlockUserCommand request, CancellationToken cancellationToken)
    {
        User? user = await _context.User.SingleOrDefaultAsync(u => u.Id == request.UserId);
        if (user is null)
            return new BaseResult_VM<bool> { Code = -1 };

        //TODO : set how get Persian date
        user.LockoutEnabled = true;
        user.LockoutEnd = request.LockOutTime;

        _context.User.Update(user);
        var save = await _context.SaveChangesAsync(cancellationToken);
        if (save is 0)
        {
            return new BaseResult_VM<bool>
            {
                Result = true,
                Code = -1,
                Message = "مشکلی در فرایند مسدود کردن کاربر به وجود آمده",
            };
        }

        return new BaseResult_VM<bool>
        {
            Result = true,
            Code = 0,
            Message = "تا تاریخ مشخص شده کاربر مسدود شد ",

        };
    }

    #endregion
}
