using Chamedoon.Application.Common.Models;
using Chamedoon.Application.Services.Admin.Common.Models;

namespace Chamedoon.Application.Services.Admin.Discounts;

public interface IAdminDiscountCodeService
{
    Task<OperationResult<PaginatedList<AdminDiscountCodeDto>>> GetDiscountCodesAsync(string? search, int page, int pageSize, CancellationToken cancellationToken);
    Task<OperationResult<AdminDiscountCodeDto>> GetDiscountCodeAsync(long id, CancellationToken cancellationToken);
    Task<OperationResult<AdminDiscountCodeDto>> CreateDiscountCodeAsync(AdminDiscountCodeInput input, CancellationToken cancellationToken);
    Task<OperationResult<AdminDiscountCodeDto>> UpdateDiscountCodeAsync(AdminDiscountCodeInput input, CancellationToken cancellationToken);
    Task<OperationResult<bool>> DeleteDiscountCodeAsync(long id, CancellationToken cancellationToken);
}
