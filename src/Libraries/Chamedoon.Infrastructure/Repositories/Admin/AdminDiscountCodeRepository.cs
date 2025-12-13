using System.Linq;
using Chamedoon.Application.Common.Interfaces.Admin;
using Chamedoon.Application.Common.Models;
using Chamedoon.Domin.Entity.Payments;
using Chamedoon.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Chamedoon.Infrastructure.Repositories.Admin;

public class AdminDiscountCodeRepository : IAdminDiscountCodeRepository
{
    private readonly ApplicationDbContext _context;

    public AdminDiscountCodeRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public Task<PaginatedList<DiscountCode>> GetDiscountCodesAsync(string? search, int pageNumber, int pageSize, CancellationToken cancellationToken)
    {
        var query = _context.DiscountCodes.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(search))
        {
            var lowered = search.Trim().ToLower();
            query = query.Where(code =>
                (!string.IsNullOrEmpty(code.Code) && code.Code.ToLower().Contains(lowered)) ||
                (!string.IsNullOrEmpty(code.Description) && code.Description.ToLower().Contains(lowered)));
        }

        query = query.OrderByDescending(code => code.CreatedAtUtc);
        return PaginatedList<DiscountCode>.CreateAsync(query, pageNumber, pageSize);
    }

    public Task<DiscountCode?> GetDiscountCodeAsync(long id, CancellationToken cancellationToken)
        => _context.DiscountCodes.AsNoTracking().FirstOrDefaultAsync(c => c.Id == id, cancellationToken);

    public Task<DiscountCode?> GetByCodeAsync(string code, CancellationToken cancellationToken)
        => _context.DiscountCodes.AsNoTracking().FirstOrDefaultAsync(c => c.Code == code, cancellationToken);

    public async Task<DiscountCode> CreateDiscountCodeAsync(DiscountCode code, CancellationToken cancellationToken)
    {
        _context.DiscountCodes.Add(code);
        await _context.SaveChangesAsync(cancellationToken);
        return code;
    }

    public async Task<DiscountCode?> UpdateDiscountCodeAsync(DiscountCode code, CancellationToken cancellationToken)
    {
        var existing = await _context.DiscountCodes.FirstOrDefaultAsync(c => c.Id == code.Id, cancellationToken);
        if (existing is null)
        {
            return null;
        }

        existing.Code = code.Code;
        existing.Type = code.Type;
        existing.Value = code.Value;
        existing.IsActive = code.IsActive;
        existing.ExpiresAtUtc = code.ExpiresAtUtc;
        existing.Description = code.Description;

        await _context.SaveChangesAsync(cancellationToken);
        return existing;
    }

    public async Task<bool> DeleteDiscountCodeAsync(long id, CancellationToken cancellationToken)
    {
        var existing = await _context.DiscountCodes.FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
        if (existing is null)
        {
            return false;
        }

        _context.DiscountCodes.Remove(existing);
        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }
}
