using Chamedoon.Application.Common.Interfaces;
using Chamedoon.Application.Common.Models;
using Chamedoon.Domin.Base;
using MediatR;

namespace Chamedoon.Application.Services.Account.Roles.Command;
public class RemoveRoleCommand : IRequest<OperationResult<bool>>
{
    public int? RoleId { get; set; }

}
public class RemoveRoleCommandHandler : IRequestHandler<RemoveRoleCommand, OperationResult<bool>>
{
    #region Property
    private readonly IApplicationDbContext _context;
    #endregion

    #region Ctor
    public RemoveRoleCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }
    #endregion

    #region Method
    public async Task<OperationResult<bool>> Handle(RemoveRoleCommand request, CancellationToken cancellationToken)
    {
        var role = await _context.Role.FindAsync(request.RoleId);
        if (role == null)
            return OperationResult<bool>.Fail("نقش موردنظر یافت نشد" , false);

        _context.Role.Remove(role);
        await _context.SaveChangesAsync(cancellationToken);

        return OperationResult<bool>.Success(true);
    }

    #endregion
}