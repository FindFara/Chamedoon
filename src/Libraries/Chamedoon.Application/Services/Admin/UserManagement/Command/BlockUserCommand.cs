using Chamedoon.Application.Common.Interfaces;
using Chamedoon.Application.Common.Models;
using Chamedoon.Domin.Base;
using Chamedoon.Domin.Entity.User;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Chamedoon.Application.Services.Admin.UserManagement.Command;

public class BlockUserCommand : IRequest<OperationResult<bool>>
{
    public long UserId { get; set; }
    public DateTime? LockOutTime { get; set; }
}
public class BlockUserCommandHandler : IRequestHandler<BlockUserCommand, OperationResult<bool>>
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
    public async Task<OperationResult<bool>> Handle(BlockUserCommand request, CancellationToken cancellationToken)
    {
        User? user = await _context.User.SingleOrDefaultAsync(u => u.Id == request.UserId);
        if (user is null)
            return OperationResult<bool>.Fail();

        //TODO : set how get Persian date
        user.LockoutEnabled = true;
        user.LockoutEnd = request.LockOutTime;

        _context.User.Update(user);
        var save = await _context.SaveChangesAsync(cancellationToken);
        if (save is 0)
        {
            return OperationResult<bool>.Fail("مشکلی در فرایند مسدود کردن کاربر به وجود آمده");
        }

        return OperationResult<bool>.Success(true);

    }

    #endregion
}
