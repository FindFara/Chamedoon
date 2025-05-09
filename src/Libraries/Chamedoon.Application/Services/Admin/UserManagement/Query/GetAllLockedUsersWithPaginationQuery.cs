﻿using AutoMapper;
using AutoMapper.QueryableExtensions;
using Chamedoon.Application.Common.Interfaces;
using Chamedoon.Application.Common.Models;
using Chamedoon.Application.Common.Utilities.AutoMapper;
using Chamedoon.Application.Services.Admin.UserManagement.ViewModel;
using Chamedoon.Domin.Base;
using Chamedoon.Domin.Entity.Users;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chamedoon.Application.Services.Admin.UserManagement.Query
{
    public class GetAllLockedUsersWithPaginationQuery : IRequest<OperationResult<PaginatedList<AdminUserManagement_VM>>>
    {
        public int PageSize { get; set; } = 20;
        public int PageNumber { get; set; } = 1;
        public required AdminUserManagement_VM AdminPanelUser { get; set; }
    }
    public class GetAllLockedUsersQueryHandler : IRequestHandler<GetAllLockedUsersWithPaginationQuery, OperationResult<PaginatedList<AdminUserManagement_VM>>>
    {
        #region Property
        private readonly IApplicationDbContext _context;
        private readonly IMapper mapper;
        #endregion

        #region Ctor
        public GetAllLockedUsersQueryHandler(IApplicationDbContext context , IMapper mapper)
        {
            _context = context;
            this.mapper=mapper;
        }
        #endregion

        #region Method
        public async Task<OperationResult<PaginatedList<AdminUserManagement_VM>>> Handle(GetAllLockedUsersWithPaginationQuery request, CancellationToken cancellationToken)
        {
            FilterUserAdminPanel filterUser = new FilterUserAdminPanel(_context);
            IQueryable<User> filters = filterUser.ApplyAdminPanelUserFilters(request.AdminPanelUser);

            PaginatedList<AdminUserManagement_VM> users = await filters
           .Where(u=>u.LockoutEnabled == true)
           .OrderByDescending(x => x.Id)
           .ProjectTo<AdminUserManagement_VM>(mapper.ConfigurationProvider)
           .PaginatedListAsync(request.PageNumber, request.PageSize);

            return OperationResult<PaginatedList<AdminUserManagement_VM>>.Success(users);
        }

        #endregion
    }
}
