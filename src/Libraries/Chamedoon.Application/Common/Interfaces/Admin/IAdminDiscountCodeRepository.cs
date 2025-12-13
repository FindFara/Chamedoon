using Chamedoon.Application.Common.Models;
using Chamedoon.Domin.Entity.Payments;

namespace Chamedoon.Application.Common.Interfaces.Admin;

public interface IAdminDiscountCodeRepository
{
    Task<PaginatedList<DiscountCode>> GetDiscountCodesAsync(string? search, int pageNumber, int pageSize, CancellationToken cancellationToken);
    Task<DiscountCode?> GetDiscountCodeAsync(long id, CancellationToken cancellationToken);
    Task<DiscountCode?> GetByCodeAsync(string code, CancellationToken cancellationToken);
    Task<DiscountCode> CreateDiscountCodeAsync(DiscountCode code, CancellationToken cancellationToken);
    Task<DiscountCode?> UpdateDiscountCodeAsync(DiscountCode code, CancellationToken cancellationToken);
    Task<bool> DeleteDiscountCodeAsync(long id, CancellationToken cancellationToken);
}
