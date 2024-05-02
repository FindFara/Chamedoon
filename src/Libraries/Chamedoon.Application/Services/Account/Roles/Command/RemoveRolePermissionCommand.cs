using Chamedoon.Application.Common.Interfaces;
using Chamedoon.Application.Common.Models;
using Chamedoon.Domin.Base;
using MediatR;

namespace Chamedoon.Application.Services.Account.Roles.Command;
public class RemoveRolePermissionCommand : IRequest<OperationResult<bool>>
{
    public int? RolePermissionId { get; set; }

}
public class RemoveRolePermissionCommandHandler : IRequestHandler<RemoveRolePermissionCommand, OperationResult<bool>>
{
    #region Property
    private readonly IApplicationDbContext _context;
    #endregion

    #region Ctor
    public RemoveRolePermissionCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }
    #endregion

    #region Method
    public async Task<OperationResult<bool>> Handle(RemoveRolePermissionCommand request, CancellationToken cancellationToken)
    {
        var rolePermission = await _context.RolePermission.FindAsync(request.RolePermissionId);
        if (rolePermission == null)
            return OperationResult<bool>.Fail("دسترسی موردنظر یافت نشد",false);

        _context.RolePermission.Remove(rolePermission);
        await _context.SaveChangesAsync(cancellationToken);

        return OperationResult<bool>.Success(true);
    }

    #endregion
}