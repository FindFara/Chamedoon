using Chamedoon.Application.Common.Models;

namespace Chamedoon.Application.Common.Interfaces;

public interface ISmsService
{
    Task<OperationResult<string>> SendVerificationCodeAsync(string phoneNumber, string code, CancellationToken cancellationToken);
}
