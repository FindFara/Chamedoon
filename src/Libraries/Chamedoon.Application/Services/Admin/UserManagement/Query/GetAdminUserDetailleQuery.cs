using AutoMapper;
using Chamedoon.Application.Common.Interfaces;
using Chamedoon.Application.Common.Models;
using Chamedoon.Application.Services.Admin.UserManagement.ViewModel;
using Chamedoon.Domin.Base;
using Chamedoon.Domin.Entity.Users;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Chamedoon.Application.Services.Admin.UserManagement.Query;

public class GetAdminPanelUserDetailleQuery : IRequest<OperationResult<AdminUserDetaile_VM>>
{
    public long UserId { get; set; }
}
public class GetAdminPanelUserDetailleQueryHandler : IRequestHandler<GetAdminPanelUserDetailleQuery, OperationResult<AdminUserDetaile_VM>>
{
    #region Property
    private readonly IApplicationDbContext context;
    private readonly IMapper mapper;
    #endregion

    #region Ctor
    public GetAdminPanelUserDetailleQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        this.context = context;
        this.mapper=mapper;
    }
    #endregion

    #region Method
    public async Task<OperationResult<AdminUserDetaile_VM>> Handle(GetAdminPanelUserDetailleQuery request, CancellationToken cancellationToken)
    {
        User? user = await context.User.AsNoTracking().SingleOrDefaultAsync(u => u.Id == request.UserId);
        if (user is null)
            return OperationResult<AdminUserDetaile_VM>.Fail();

        return OperationResult<AdminUserDetaile_VM>.Success(mapper.Map<AdminUserDetaile_VM>(user));
    }

    #endregion
}
