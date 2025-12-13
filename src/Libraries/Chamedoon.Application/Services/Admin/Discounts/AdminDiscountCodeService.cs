using System.Linq;
using Chamedoon.Application.Common.Interfaces.Admin;
using Chamedoon.Application.Common.Models;
using Chamedoon.Application.Services.Admin.Common;
using Chamedoon.Application.Services.Admin.Common.Models;
using Chamedoon.Domin.Entity.Payments;

namespace Chamedoon.Application.Services.Admin.Discounts;

public class AdminDiscountCodeService : IAdminDiscountCodeService
{
    private readonly IAdminDiscountCodeRepository _repository;

    public AdminDiscountCodeService(IAdminDiscountCodeRepository repository)
    {
        _repository = repository;
    }

    public async Task<OperationResult<PaginatedList<AdminDiscountCodeDto>>> GetDiscountCodesAsync(string? search, int page, int pageSize, CancellationToken cancellationToken)
    {
        var codes = await _repository.GetDiscountCodesAsync(search, page, pageSize, cancellationToken);
        var mapped = codes.Items.Select(code => code.ToAdminDiscountCodeDto()).ToList();
        return OperationResult<PaginatedList<AdminDiscountCodeDto>>.Success(new PaginatedList<AdminDiscountCodeDto>(mapped, codes.TotalCount, codes.PageNumber, codes.PageSize));
    }

    public async Task<OperationResult<AdminDiscountCodeDto>> GetDiscountCodeAsync(long id, CancellationToken cancellationToken)
    {
        var code = await _repository.GetDiscountCodeAsync(id, cancellationToken);
        if (code is null)
        {
            return OperationResult<AdminDiscountCodeDto>.Fail("کد تخفیف یافت نشد.");
        }

        return OperationResult<AdminDiscountCodeDto>.Success(code.ToAdminDiscountCodeDto());
    }

    public async Task<OperationResult<AdminDiscountCodeDto>> CreateDiscountCodeAsync(AdminDiscountCodeInput input, CancellationToken cancellationToken)
    {
        var existing = await _repository.GetByCodeAsync(input.Code, cancellationToken);
        if (existing is not null)
        {
            return OperationResult<AdminDiscountCodeDto>.Fail("کد تخفیف دیگری با این کد وجود دارد.");
        }

        var created = await _repository.CreateDiscountCodeAsync(BuildDiscountCode(input), cancellationToken);
        return OperationResult<AdminDiscountCodeDto>.Success(created.ToAdminDiscountCodeDto());
    }

    public async Task<OperationResult<AdminDiscountCodeDto>> UpdateDiscountCodeAsync(AdminDiscountCodeInput input, CancellationToken cancellationToken)
    {
        if (!input.Id.HasValue)
        {
            return OperationResult<AdminDiscountCodeDto>.Fail("شناسه ارسال نشده است.");
        }

        var existing = await _repository.GetByCodeAsync(input.Code, cancellationToken);
        if (existing is not null && existing.Id != input.Id.Value)
        {
            return OperationResult<AdminDiscountCodeDto>.Fail("کد انتخابی تکراری است.");
        }

        var updated = await _repository.UpdateDiscountCodeAsync(BuildDiscountCode(input), cancellationToken);
        if (updated is null)
        {
            return OperationResult<AdminDiscountCodeDto>.Fail("کد تخفیف یافت نشد.");
        }

        return OperationResult<AdminDiscountCodeDto>.Success(updated.ToAdminDiscountCodeDto());
    }

    public async Task<OperationResult<bool>> DeleteDiscountCodeAsync(long id, CancellationToken cancellationToken)
    {
        var deleted = await _repository.DeleteDiscountCodeAsync(id, cancellationToken);
        return deleted
            ? OperationResult<bool>.Success(true)
            : OperationResult<bool>.Fail("امکان حذف کد تخفیف وجود ندارد.");
    }

    private static DiscountCode BuildDiscountCode(AdminDiscountCodeInput input)
        => new()
        {
            Id = input.Id ?? 0,
            Code = input.Code.Trim(),
            Type = input.Type,
            Value = input.Value,
            IsActive = input.IsActive,
            ExpiresAtUtc = input.ExpiresAtUtc,
            Description = input.Description
        };
}
