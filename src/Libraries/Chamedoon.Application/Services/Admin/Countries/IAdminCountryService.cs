using Chamedoon.Application.Common.Models;
using Chamedoon.Application.Services.Admin.Common.Models;

namespace Chamedoon.Application.Services.Admin.Countries;

public interface IAdminCountryService
{
    Task<OperationResult<IReadOnlyList<AdminCountryDto>>> GetCountriesAsync(CancellationToken cancellationToken);
    Task<OperationResult<AdminCountryDto>> SaveCountryAsync(AdminCountryInput input, CancellationToken cancellationToken);
    Task<OperationResult<AdminCountryLivingCostDto>> SaveLivingCostAsync(AdminCountryLivingCostInput input, CancellationToken cancellationToken);
    Task<OperationResult<AdminCountryRestrictionDto>> SaveRestrictionAsync(AdminCountryRestrictionInput input, CancellationToken cancellationToken);
    Task<OperationResult<AdminCountryJobDto>> SaveJobAsync(AdminCountryJobInput input, CancellationToken cancellationToken);
    Task<OperationResult<AdminCountryEducationDto>> SaveEducationAsync(AdminCountryEducationInput input, CancellationToken cancellationToken);
    Task<OperationResult> DeleteJobAsync(long id, long countryId, CancellationToken cancellationToken);
    Task<OperationResult> DeleteEducationAsync(long id, long countryId, CancellationToken cancellationToken);
}
