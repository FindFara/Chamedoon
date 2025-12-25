using Chamedoon.Application.Common.Models;
using Chamedoon.Application.Services.Admin.Common.Models;

namespace Chamedoon.Application.Services.Admin.Users;

public interface IAdminUserService
{
    Task<OperationResult<PaginatedList<AdminUserDto>>> GetUsersAsync(string? search, long? roleId, string? subscriptionPlanId, int pageNumber, int pageSize, CancellationToken cancellationToken);
    Task<OperationResult<AdminUserDto>> GetUserAsync(long id, CancellationToken cancellationToken);
    Task<OperationResult<AdminUserDto>> CreateUserAsync(AdminUserInput input, CancellationToken cancellationToken);
    Task<OperationResult<AdminUserDto>> UpdateUserAsync(AdminUserInput input, CancellationToken cancellationToken);
    Task<OperationResult<bool>> DeleteUserAsync(long id, CancellationToken cancellationToken);
    Task<OperationResult<IReadOnlyList<AdminRoleDto>>> GetRolesAsync(CancellationToken cancellationToken);
}
