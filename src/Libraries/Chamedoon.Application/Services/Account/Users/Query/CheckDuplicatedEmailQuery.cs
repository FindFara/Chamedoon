﻿using Chamedoon.Application.Common.Interfaces;
using Chamedoon.Application.Common.Models;
using Chamedoon.Application.Services.Email.Query;
using MediatR;
using Microsoft.EntityFrameworkCore;


namespace Chamedoon.Application.Services.Account.Users.Query;

public class CheckDuplicatedEmailQuery : IRequest<OperationResult<bool>>
{
    public string Email { get; set; }
}
public class CheckDuplicatedEmailQueryHandler : IRequestHandler<CheckDuplicatedEmailQuery, OperationResult<bool>>
{
    #region Property
    private readonly IApplicationDbContext _context;
    private readonly IMediator mediator;
    #endregion

    #region Ctor
    public CheckDuplicatedEmailQueryHandler(IApplicationDbContext context, IMediator mediator)
    {
        _context = context;
        this.mediator = mediator;
    }
    #endregion

    #region Method
    public async Task<OperationResult<bool>> Handle(CheckDuplicatedEmailQuery request, CancellationToken cancellationToken)
    {

        bool isDuplicated = await _context.User.AnyAsync(u => u.Email == request.Email);
        if (isDuplicated)
        {
            var user = await mediator.Send(new GetUserQuery { Email = request.Email });

            if (!user.Result.EmailConfirmed)
            {
                await mediator.Send(new SendTokenToUserEmailQuery { UserName = user.Result.UserName });
                return OperationResult<bool>.Fail("ایمیل شما قبلاً ثبت شده اما هنوز تأیید نشده. لطفاً ایمیل خود را تأیید کنید.");
            }
            return OperationResult<bool>.Fail("این ایمیل قبلاً استفاده شده است. لطفاً لاگین کنید.");
        }
        return OperationResult<bool>.Success(true);
    }

    #endregion
}


