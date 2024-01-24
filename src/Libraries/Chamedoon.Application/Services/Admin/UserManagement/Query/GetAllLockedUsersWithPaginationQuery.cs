using AutoMapper;
using AutoMapper.QueryableExtensions;
using Chamedoon.Application.Common.Interfaces;
using Chamedoon.Application.Common.Models;
using Chamedoon.Application.Common.Utilities.AutoMapper;
using Chamedoon.Application.Services.Admin.UserManagement.ViewModel;
using Chamedoon.Domin.Base;
using Chamedoon.Domin.Entity.User;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chamedoon.Application.Services.Admin.UserManagement.Query
{
    public class GetAllLockedUsersWithPaginationQuery : IRequest<BaseResult_VM<PaginatedList<AdminPanelUser_VM>>>
    {
        public int PageSize { get; set; } = 20;
        public int PageNumber { get; set; } = 1;
        public required AdminPanelUser_VM AdminPanelUser { get; set; }
    }
    public class GetAllLockedUsersQueryHandler : IRequestHandler<GetAllLockedUsersWithPaginationQuery, BaseResult_VM<PaginatedList<AdminPanelUser_VM>>>
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
        public async Task<BaseResult_VM<PaginatedList<AdminPanelUser_VM>>> Handle(GetAllLockedUsersWithPaginationQuery request, CancellationToken cancellationToken)
        {
            FilterUserAdminPanel filterUser = new FilterUserAdminPanel(_context);
            IQueryable<User> filters = filterUser.ApplyAdminPanelUserFilters(request.AdminPanelUser);

            PaginatedList<AdminPanelUser_VM> users = await filters
           .Where(u=>u.LockoutEnabled == true)
           .OrderByDescending(x => x.Id)
           .ProjectTo<AdminPanelUser_VM>(mapper.ConfigurationProvider)
           .PaginatedListAsync(request.PageNumber, request.PageSize);

            return new BaseResult_VM<PaginatedList<AdminPanelUser_VM>>
            {
                Code = 0,
                Message = "Successful",
                Result =users
            };
        }

        #endregion
    }
}
