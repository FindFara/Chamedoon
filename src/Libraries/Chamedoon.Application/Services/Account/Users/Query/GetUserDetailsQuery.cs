using AutoMapper;
using Chamedoon.Application.Common.Interfaces;
using Chamedoon.Application.Common.Models;
using Chamedoon.Application.Services.Account.Users.ViewModel;
using Chamedoon.Domin.Entity.Users;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Chamedoon.Application.Services.Account.Users.Query;

public class GetUserDetailsQuery : IRequest<OperationResult<UserDetails_VM>>
{
    public long? Id { get; set; }
    public string? UserName { get; set; }
    public string? Email { get; set; }
}
public class GetUserDetailsQueryHandler : IRequestHandler<GetUserDetailsQuery, OperationResult<UserDetails_VM>>
{
    #region Property
    private readonly IApplicationDbContext _context;
    private readonly IMapper mapper;
    #endregion

    #region Ctor
    public GetUserDetailsQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        this.mapper = mapper;
    }
    #endregion

    #region Method
    public async Task<OperationResult<UserDetails_VM>> Handle(GetUserDetailsQuery request, CancellationToken cancellationToken)
    {
        User? user = await _context.User
             .AsNoTracking()
             .SingleOrDefaultAsync(u =>
                                   u.Id == request.Id ||
                                   u.NormalizedUserName == (request.UserName ?? "").ToUpper() ||
                                   u.NormalizedEmail == (request.Email ?? "").ToUpper());

        if (user is null)
            return OperationResult<UserDetails_VM>.Fail("کاربری با مشخصات واد شده یافت نشد");

        return OperationResult<UserDetails_VM>.Success(mapper.Map<UserDetails_VM>(user));
    }
    #endregion
}
