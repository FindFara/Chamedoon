using AutoMapper;
using Chamedoon.Application.Common.Interfaces;
using Chamedoon.Application.Common.Models;
using Chamedoon.Application.Services.Account.Roles.ViewModel;
using Chamedoon.Domin.Base;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Chamedoon.Application.Services.Account.Roles.Query;

public class GetAllRolesQuery : IRequest<OperationResult<List<Role_VM>>>
{
}
public class GetAllRolesQueryHandler : IRequestHandler<GetAllRolesQuery, OperationResult<List<Role_VM>>>
{
    #region Property
    private readonly IApplicationDbContext context;
    private readonly IMapper mapper;
    #endregion

    #region Ctor
    public GetAllRolesQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        this.context = context;
        this.mapper = mapper;
    }
    #endregion

    #region Method
    public async Task<OperationResult<List<Role_VM>>> Handle(GetAllRolesQuery request, CancellationToken cancellationToken)
    {

        var roles = await context.Role.ToListAsync();
        return OperationResult<List<Role_VM>>.Success(mapper.Map<List<Role_VM>>(roles));
    }

    #endregion
}


