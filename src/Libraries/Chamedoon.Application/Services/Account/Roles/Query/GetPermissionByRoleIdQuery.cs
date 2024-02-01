using AutoMapper;
using Chamedoon.Application.Common.Interfaces;
using Chamedoon.Application.Services.Account.Roles.ViewModel;
using Chamedoon.Domin.Base;
using Chamedoon.Domin.Entity.Permissions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Chamedoon.Application.Services.Account.Roles.Query;
public class GetPermissionByRoleIdQuery : IRequest<BaseResult_VM<List<RolePermission_VM>>>
{
    public long RoleId { get; set; }
}
public class GetPermissionByRoleIdQueryHandler : IRequestHandler<GetPermissionByRoleIdQuery, BaseResult_VM<List<RolePermission_VM>>>
{
    #region Property
    private readonly IApplicationDbContext context;
    private readonly IMapper mapper;
    #endregion

    #region Ctor
    public GetPermissionByRoleIdQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        this.context = context;
        this.mapper = mapper;
    }
    #endregion

    #region Method
    public async Task<BaseResult_VM<List<RolePermission_VM>>> Handle(GetPermissionByRoleIdQuery request, CancellationToken cancellationToken)
    {
        List<RolePermission> permissions = await context.RolePermission.Where(p => p.RoleId == request.RoleId).ToListAsync();
        return new BaseResult_VM<List<RolePermission_VM>>
        {
            Result = mapper.Map<List<RolePermission_VM>>(permissions),
            Code = 0,
            Message = "با موفقیت دریافت شد ",
        };
    }

    #endregion
}