using Chamedoon.Application.Common.Interfaces.Admin;
using Chamedoon.Domin.Entity.Countries;
using Chamedoon.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Chamedoon.Infrastructure.Repositories.Admin;

public class AdminCountryRepository : IAdminCountryRepository
{
    private readonly ApplicationDbContext _context;

    public AdminCountryRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<Country>> GetCountriesWithDetailsAsync(CancellationToken cancellationToken)
    {
        return await _context.Countries
            .AsNoTracking()
            .AsSplitQuery()
            .Include(c => c.LivingCosts)
            .Include(c => c.Restrictions)
            .Include(c => c.Jobs)
            .Include(c => c.Educations)
            .OrderBy(c => c.Name)
            .ToListAsync(cancellationToken);
    }

    public Task<Country?> GetCountryAsync(long id, CancellationToken cancellationToken)
        => _context.Countries
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);

    public Task<Country?> GetCountryWithDetailsAsync(long id, CancellationToken cancellationToken)
        => _context.Countries
            .AsNoTracking()
            .AsSplitQuery()
            .Include(c => c.LivingCosts)
            .Include(c => c.Restrictions)
            .Include(c => c.Jobs)
            .Include(c => c.Educations)
            .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);

    public async Task<Country> AddCountryAsync(Country country, CancellationToken cancellationToken)
    {
        await _context.Countries.AddAsync(country, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return country;
    }

    public async Task<Country?> UpdateCountryAsync(Country country, CancellationToken cancellationToken)
    {
        var existing = await _context.Countries.FirstOrDefaultAsync(c => c.Id == country.Id, cancellationToken);
        if (existing is null)
        {
            return null;
        }

        existing.Key = country.Key;
        existing.Name = country.Name;
        existing.InvestmentAmount = country.InvestmentAmount;
        existing.InvestmentCurrency = country.InvestmentCurrency;
        existing.InvestmentNotes = country.InvestmentNotes;
        existing.AdditionalInfo = country.AdditionalInfo;
        existing.MaritalStatusImpact = country.MaritalStatusImpact;

        await _context.SaveChangesAsync(cancellationToken);
        return existing;
    }

    public Task<CountryLivingCost?> GetLivingCostAsync(long id, CancellationToken cancellationToken)
        => _context.CountryLivingCosts.FirstOrDefaultAsync(c => c.Id == id, cancellationToken);

    public async Task<CountryLivingCost> SaveLivingCostAsync(CountryLivingCost livingCost, CancellationToken cancellationToken)
    {
        if (livingCost.Id == 0)
        {
            await _context.CountryLivingCosts.AddAsync(livingCost, cancellationToken);
        }
        else
        {
            _context.CountryLivingCosts.Update(livingCost);
        }

        await _context.SaveChangesAsync(cancellationToken);
        return livingCost;
    }

    public Task<CountryRestriction?> GetRestrictionAsync(long id, CancellationToken cancellationToken)
        => _context.CountryRestrictions.FirstOrDefaultAsync(r => r.Id == id, cancellationToken);

    public async Task<CountryRestriction> SaveRestrictionAsync(CountryRestriction restriction, CancellationToken cancellationToken)
    {
        if (restriction.Id == 0)
        {
            await _context.CountryRestrictions.AddAsync(restriction, cancellationToken);
        }
        else
        {
            _context.CountryRestrictions.Update(restriction);
        }

        await _context.SaveChangesAsync(cancellationToken);
        return restriction;
    }

    public Task<CountryJob?> GetJobAsync(long id, CancellationToken cancellationToken)
        => _context.CountryJobs.FirstOrDefaultAsync(j => j.Id == id, cancellationToken);

    public async Task<CountryJob> SaveJobAsync(CountryJob job, CancellationToken cancellationToken)
    {
        if (job.Id == 0)
        {
            await _context.CountryJobs.AddAsync(job, cancellationToken);
        }
        else
        {
            _context.CountryJobs.Update(job);
        }

        await _context.SaveChangesAsync(cancellationToken);
        return job;
    }

    public async Task<bool> DeleteJobAsync(long id, CancellationToken cancellationToken)
    {
        var existing = await _context.CountryJobs.FirstOrDefaultAsync(j => j.Id == id, cancellationToken);
        if (existing is null)
        {
            return false;
        }

        _context.CountryJobs.Remove(existing);
        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }

    public Task<CountryEducation?> GetEducationAsync(long id, CancellationToken cancellationToken)
        => _context.CountryEducations.FirstOrDefaultAsync(e => e.Id == id, cancellationToken);

    public async Task<CountryEducation> SaveEducationAsync(CountryEducation education, CancellationToken cancellationToken)
    {
        if (education.Id == 0)
        {
            await _context.CountryEducations.AddAsync(education, cancellationToken);
        }
        else
        {
            _context.CountryEducations.Update(education);
        }

        await _context.SaveChangesAsync(cancellationToken);
        return education;
    }

    public async Task<bool> DeleteEducationAsync(long id, CancellationToken cancellationToken)
    {
        var existing = await _context.CountryEducations.FirstOrDefaultAsync(e => e.Id == id, cancellationToken);
        if (existing is null)
        {
            return false;
        }

        _context.CountryEducations.Remove(existing);
        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }
}
