using AutoMapper;
using AutoMapper.QueryableExtensions;
using Chamedoon.Application.Common.Interfaces;
using Chamedoon.Application.Common.Models;
using Chamedoon.Application.Services.Admin.UserManagement.Query;
using Chamedoon.Application.Services.Admin.UserManagement.ViewModel;
using Chamedoon.Domin.Base;
using Chamedoon.Domin.Entity.User;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Chamedoon.Application.Services.Admin.UserManegment.Query
{
    public class GetUsersWithPaginationQuery : IRequest<BaseResult_VM<PaginatedList<AdminPanelUser_VM>>>
    {
        public int PageSize { get; set; } = 20;
        public int PageNumber { get; set; } = 1;
        public AdminPanelUser_VM AdminPanelUser { get; set; }
    }
    public class GetUsersWithPaginationQueryHandler : IRequestHandler<GetUsersWithPaginationQuery, BaseResult_VM<PaginatedList<AdminPanelUser_VM>>>
    {
        #region Property
        private readonly IApplicationDbContext context ;
        private readonly IMapper mapper;
        #endregion

        #region Ctor
        public GetUsersWithPaginationQueryHandler(IApplicationDbContext context , IMapper mapper)
        {
            this.context = context;
            this.mapper=mapper;
        }
        #endregion

        #region Method
        public async Task<BaseResult_VM<PaginatedList<AdminPanelUser_VM>>> Handle(GetUsersWithPaginationQuery request, CancellationToken cancellationToken)
        {
            //FilterUserAdminPanel filterUser = new FilterUserAdminPanel(context);
            //IQueryable<User> filters = filterUser.ApplyAdminPanelUserFilters(request.AdminPanelUser);

            //     PaginatedList<AdminPanelUser_VM> users = await filters
            //    .OrderByDescending(x => x.Id)
            //    .ProjectTo<AdminPanelUser_VM>(mapper.ConfigurationProvider)
            //    .PaginatedListAsync(request.PageNumber, request.PageSize);

            //_context.User.Where ()


            return new BaseResult_VM<PaginatedList<AdminPanelUser_VM>>
            {
                Code = 0,
                Message = "",

            };
        }

        #endregion
    }
}
