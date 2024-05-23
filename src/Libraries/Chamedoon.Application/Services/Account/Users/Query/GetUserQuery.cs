using AutoMapper;
using Chamedoon.Application.Common.Interfaces;
using Chamedoon.Application.Common.Models;
using Chamedoon.Domin.Entity.Users;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Chamedoon.Application.Services.Account.Users.Query;

public class GetUserQuery : IRequest<OperationResult<User>>
{
    public long? Id { get; set; }
    public string? UserName { get; set; }
    public string? Email { get; set; }
}
public class GetUserByIdQueryHandler : IRequestHandler<GetUserQuery, OperationResult<User>>
{
    #region Property
    private readonly IApplicationDbContext _context;
    private readonly IMapper mapper;
    #endregion

    #region Ctor
    public GetUserByIdQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        this.mapper = mapper;
    }
    #endregion

    #region Method
    public async Task<OperationResult<User>> Handle(GetUserQuery request, CancellationToken cancellationToken)
    {
        User? user = await _context.User
             .SingleOrDefaultAsync(u =>
                                   u.Id == request.Id ||
                                   u.NormalizedUserName == (request.UserName ?? "").ToUpper() ||
                                   u.NormalizedEmail == (request.Email ?? "").ToUpper());
        if (user is null)
            OperationResult<User>.Fail("کاربری با مشخصات واد شده یافت نشد");

        return OperationResult<User>.Success(mapper.Map<User>(user));
    }
    #endregion
}
