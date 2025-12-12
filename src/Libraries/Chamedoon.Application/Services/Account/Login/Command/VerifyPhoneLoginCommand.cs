using AutoMapper;
using System;
using Chamedoon.Application.Common.Models;
using Chamedoon.Application.Common.Utilities;
using Chamedoon.Application.Services.Account.Register.Command;
using Chamedoon.Application.Services.Account.Users.Command;
using Chamedoon.Application.Services.Account.Users.Query;
using Chamedoon.Application.Services.Account.Users.ViewModel;
using Chamedoon.Application.Services.Customers.Command;
using MediatR;
using Microsoft.Extensions.Caching.Memory;

namespace Chamedoon.Application.Services.Account.Login.Command;

public class VerifyPhoneLoginCommand : IRequest<OperationResult<UserDetails_VM>>
{
    public required string PhoneNumber { get; set; }
    public required string Code { get; set; }
}

public class VerifyPhoneLoginCommandHandler : IRequestHandler<VerifyPhoneLoginCommand, OperationResult<UserDetails_VM>>
{
    private const string CachePrefix = "phone-login-";

    private readonly IMediator _mediator;
    private readonly IMemoryCache _cache;
    private readonly IMapper _mapper;

    public VerifyPhoneLoginCommandHandler(IMediator mediator, IMemoryCache cache, IMapper mapper)
    {
        _mediator = mediator;
        _cache = cache;
        _mapper = mapper;
    }

    public async Task<OperationResult<UserDetails_VM>> Handle(VerifyPhoneLoginCommand request, CancellationToken cancellationToken)
    {
        var normalizedPhone = PhoneNumberHelper.Normalize(request.PhoneNumber);
        if (normalizedPhone is null)
        {
            return OperationResult<UserDetails_VM>.Fail("شماره موبایل وارد شده معتبر نیست.");
        }

        if (!_cache.TryGetValue(CachePrefix + normalizedPhone, out string? cachedCode) || string.IsNullOrWhiteSpace(cachedCode))
        {
            return OperationResult<UserDetails_VM>.Fail("کد تایید منقضی شده است. دوباره تلاش کنید.");
        }

        if (!string.Equals(cachedCode, request.Code, StringComparison.Ordinal))
        {
            return OperationResult<UserDetails_VM>.Fail("کد تایید وارد شده صحیح نیست.");
        }

        _cache.Remove(CachePrefix + normalizedPhone);

        var existingUser = await _mediator.Send(new GetUserByPhoneNumberQuery { PhoneNumber = normalizedPhone }, cancellationToken);

        long userId;
        if (existingUser.IsSuccess && existingUser.Result is not null)
        {
            userId = existingUser.Result.Id;
        }
        else
        {
            var registerResult = await _mediator.Send(new RegisterUserByPhoneCommand { PhoneNumber = normalizedPhone }, cancellationToken);
            if (registerResult.IsSuccess is false)
            {
                return OperationResult<UserDetails_VM>.Fail(registerResult.Message);
            }

            var addCustomer = await _mediator.Send(new AddCustomerCommand { Id = registerResult.Result }, cancellationToken);
            if (addCustomer.IsSuccess is false)
            {
                return OperationResult<UserDetails_VM>.Fail(addCustomer.Message);
            }

            userId = registerResult.Result;
        }

        var ensureSecurity = await _mediator.Send(new EnsureUserPhoneSecurityCommand { UserId = userId, PhoneNumber = normalizedPhone }, cancellationToken);
        if (ensureSecurity.IsSuccess is false)
        {
            return OperationResult<UserDetails_VM>.Fail(ensureSecurity.Message);
        }

        var signIn = await _mediator.Send(new SignInUserCommand { UserId = userId.ToString(), IsPersistent = true }, cancellationToken);
        if (signIn.IsSuccess is false)
        {
            return OperationResult<UserDetails_VM>.Fail(signIn.Message);
        }

        var userDetails = await _mediator.Send(new GetUserQuery { Id = userId }, cancellationToken);
        if (userDetails.IsSuccess is false || userDetails.Result is null)
        {
            return OperationResult<UserDetails_VM>.Fail("دریافت اطلاعات کاربر امکان پذیر نیست.");
        }

        var mapped = _mapper.Map<UserDetails_VM>(userDetails.Result);
        return OperationResult<UserDetails_VM>.Success(mapped);
    }
}
