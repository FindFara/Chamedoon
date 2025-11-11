using Chamedoon.Application.Common.Models;
using Chamedoon.Application.Services.Admin.Common.Models;

namespace Chamedoon.Application.Services.Admin.Roles;

public interface IAdminRoleService
{
    Task<OperationResult<IReadOnlyList<AdminRoleDto>>> GetRolesAsync(CancellationToken cancellationToken);
    Task<OperationResult<AdminRoleDto>> GetRoleAsync(long id, CancellationToken cancellationToken);
    Task<OperationResult<AdminRoleDto>> CreateRoleAsync(AdminRoleInput input, CancellationToken cancellationToken);
    Task<OperationResult<AdminRoleDto>> UpdateRoleAsync(AdminRoleInput input, CancellationToken cancellationToken);
    Task<OperationResult<bool>> DeleteRoleAsync(long id, CancellationToken cancellationToken);
    Task<OperationResult<IReadOnlyList<string>>> GetPermissionNamesAsync(CancellationToken cancellationToken);
}
