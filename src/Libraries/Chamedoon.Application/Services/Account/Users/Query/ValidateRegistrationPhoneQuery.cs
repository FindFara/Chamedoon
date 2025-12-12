using System.Text.RegularExpressions;
using Chamedoon.Application.Common.Interfaces;
using Chamedoon.Application.Common.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Chamedoon.Application.Services.Account.Users.Query;

public class ValidateRegistrationPhoneQuery : IRequest<OperationResult<bool>>
{
    public required string PhoneNumber { get; set; }
}

public class ValidateRegistrationPhoneQueryHandler : IRequestHandler<ValidateRegistrationPhoneQuery, OperationResult<bool>>
{
    private readonly IApplicationDbContext _context;

    public ValidateRegistrationPhoneQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<OperationResult<bool>> Handle(ValidateRegistrationPhoneQuery request, CancellationToken cancellationToken)
    {
        if (IsValidMobile(request.PhoneNumber) is false)
        {
            return OperationResult<bool>.Fail("شماره موبایل وارد شده معتبر نیست");
        }

        bool isDuplicated = await _context.User.AnyAsync(u => u.PhoneNumber == request.PhoneNumber, cancellationToken);
        if (isDuplicated)
        {
            return OperationResult<bool>.Fail("این شماره موبایل قبلاً استفاده شده است. لطفاً وارد شوید.");
        }

        return OperationResult<bool>.Success(true);
    }

    private static bool IsValidMobile(string? phone)
    {
        if (string.IsNullOrWhiteSpace(phone))
            return false;

        return Regex.IsMatch(phone, @"^(\+98|0)?9\d{9}$");
    }
}
