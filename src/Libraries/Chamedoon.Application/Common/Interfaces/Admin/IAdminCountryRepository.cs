using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Chamedoon.Domin.Entity.Countries;

namespace Chamedoon.Application.Common.Interfaces.Admin;

public interface IAdminCountryRepository
{
    Task<List<Country>> GetCountriesWithDetailsAsync(CancellationToken cancellationToken);
    Task<Country?> GetCountryAsync(long id, CancellationToken cancellationToken);
    Task<Country?> GetCountryWithDetailsAsync(long id, CancellationToken cancellationToken);
    Task<Country> AddCountryAsync(Country country, CancellationToken cancellationToken);
    Task<Country?> UpdateCountryAsync(Country country, CancellationToken cancellationToken);
    Task<CountryLivingCost?> GetLivingCostAsync(long id, CancellationToken cancellationToken);
    Task<CountryLivingCost> SaveLivingCostAsync(CountryLivingCost livingCost, CancellationToken cancellationToken);
    Task<CountryRestriction?> GetRestrictionAsync(long id, CancellationToken cancellationToken);
    Task<CountryRestriction> SaveRestrictionAsync(CountryRestriction restriction, CancellationToken cancellationToken);
    Task<CountryJob?> GetJobAsync(long id, CancellationToken cancellationToken);
    Task<CountryJob> SaveJobAsync(CountryJob job, CancellationToken cancellationToken);
    Task<CountryEducation?> GetEducationAsync(long id, CancellationToken cancellationToken);
    Task<CountryEducation> SaveEducationAsync(CountryEducation education, CancellationToken cancellationToken);
}
