using AutoMapper;
using Chamedoon.Application.Common.Interfaces;
using Chamedoon.Application.Common.Models;
using Chamedoon.Application.Common.Utilities;
using Chamedoon.Domin.Entity.Users;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Chamedoon.Application.Services.Account.Users.Query;

public class GetUserByPhoneNumberQuery : IRequest<OperationResult<User>>
{
    public required string PhoneNumber { get; set; }
}

public class GetUserByPhoneNumberQueryHandler : IRequestHandler<GetUserByPhoneNumberQuery, OperationResult<User>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetUserByPhoneNumberQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<OperationResult<User>> Handle(GetUserByPhoneNumberQuery request, CancellationToken cancellationToken)
    {
        var normalizedPhone = PhoneNumberHelper.Normalize(request.PhoneNumber);
        if (normalizedPhone is null)
        {
            return OperationResult<User>.Fail("شماره موبایل وارد شده معتبر نیست.");
        }

        var user = await _context.User
            .AsNoTracking()
            .SingleOrDefaultAsync(u => u.PhoneNumber == normalizedPhone, cancellationToken);

        if (user is null)
        {
            return OperationResult<User>.Fail("کاربر یافت نشد.");
        }

        return OperationResult<User>.Success(_mapper.Map<User>(user));
    }

}
