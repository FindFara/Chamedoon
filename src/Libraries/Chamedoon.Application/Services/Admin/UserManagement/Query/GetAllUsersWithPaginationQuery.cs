using AutoMapper;
using AutoMapper.QueryableExtensions;
using Chamedoon.Application.Common.Interfaces;
using Chamedoon.Application.Common.Models;
using Chamedoon.Application.Common.Utilities.AutoMapper;
using Chamedoon.Application.Services.Admin.UserManagement.ViewModel;
using Chamedoon.Domin.Base;
using Chamedoon.Domin.Entity.Users;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Runtime.ExceptionServices;
using System.Security.Cryptography;

namespace Chamedoon.Application.Services.Admin.UserManagement.Query;

public class GetAllUsersWithPaginationQuery : IRequest<OperationResult<PaginatedList<AdminUserManagement_VM>>>
{
    public int PageSize { get; set; }
    public int PageNumber { get; set; } = 1;
    public  AdminUserManagement_VM? AdminPanelUser { get; set; } 
}
public class GetUsersWithPaginationQueryHandler : IRequestHandler<GetAllUsersWithPaginationQuery, OperationResult<PaginatedList<AdminUserManagement_VM>>>
{
    #region Property
    private readonly IApplicationDbContext context;
    private readonly IMapper mapper;
    #endregion

    #region Ctor
    public GetUsersWithPaginationQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        this.context = context;
        this.mapper = mapper;
    }
    #endregion

    #region Method
    public async Task<OperationResult<PaginatedList<AdminUserManagement_VM>>> Handle(GetAllUsersWithPaginationQuery request, CancellationToken cancellationToken)
    {
        FilterUserAdminPanel filterUser = new FilterUserAdminPanel(context);
        IQueryable<User> filters = filterUser.ApplyAdminPanelUserFilters(request.AdminPanelUser);

        request.PageSize = request.PageSize is 0 ? 20 : request.PageSize;
        request.PageNumber = request.PageNumber is 0 ? 1 : request.PageNumber;

        PaginatedList<AdminUserManagement_VM> users = await filters
       .OrderByDescending(x => x.Id)
       .ProjectTo<AdminUserManagement_VM>(mapper.ConfigurationProvider)
       .PaginatedListAsync(request.PageNumber, request.PageSize);

        return OperationResult<PaginatedList<AdminUserManagement_VM>>.Success(users);
    }
    #endregion
}
