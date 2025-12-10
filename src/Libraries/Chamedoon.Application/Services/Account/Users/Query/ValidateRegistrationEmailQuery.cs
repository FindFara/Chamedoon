using System;
using System.Net.Mail;
using Chamedoon.Application.Common.Interfaces;
using Chamedoon.Application.Common.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Chamedoon.Application.Services.Account.Users.Query;

public class ValidateRegistrationEmailQuery : IRequest<OperationResult<bool>>
{
    public required string Email { get; set; }
}

public class ValidateRegistrationEmailQueryHandler : IRequestHandler<ValidateRegistrationEmailQuery, OperationResult<bool>>
{
    #region Property
    private readonly IApplicationDbContext _context;
    #endregion

    #region Ctor
    public ValidateRegistrationEmailQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }
    #endregion

    #region Method
    public async Task<OperationResult<bool>> Handle(ValidateRegistrationEmailQuery request, CancellationToken cancellationToken)
    {
        var emailValidation = ValidateEmail(request.Email);
        if (emailValidation.IsSuccess is false)
        {
            return emailValidation;
        }

        bool isDuplicated = await _context.User.AnyAsync(u => u.Email == request.Email, cancellationToken);
        if (isDuplicated)
        {
            return OperationResult<bool>.Fail("این ایمیل قبلاً استفاده شده است. لطفاً لاگین کنید.");
        }

        return OperationResult<bool>.Success(true);
    }

    private static OperationResult<bool> ValidateEmail(string email)
    {
        if (string.IsNullOrWhiteSpace(email) || IsValidEmailFormat(email) is false)
            return OperationResult<bool>.Fail("ایمیل وارد شده معتبر نمی باشد.");

        if (IsGmailAddress(email) is false)
            return OperationResult<bool>.Fail("ثبت نام تنها با ایمیل های Gmail امکان پذیر است.");

        return OperationResult<bool>.Success(true);
    }

    private static bool IsValidEmailFormat(string email)
    {
        try
        {
            _ = new MailAddress(email);
            return true;
        }
        catch
        {
            return false;
        }
    }

    private static bool IsGmailAddress(string email)
    {
        return email.Trim().EndsWith("@gmail.com", StringComparison.OrdinalIgnoreCase);
    }

    #endregion
}
