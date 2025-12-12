using Chamedoon.Application.Common.Models;

namespace Chamedoon.Application.Common.Interfaces;

public interface ISmsService
{
    Task<OperationResult> SendVerificationCodeAsync(string phoneNumber, string code, CancellationToken cancellationToken);
}
