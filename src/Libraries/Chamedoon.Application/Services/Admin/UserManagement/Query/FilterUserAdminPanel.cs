using Chamedoon.Application.Common.Interfaces;
using Chamedoon.Application.Services.Admin.UserManagement.ViewModel;
using Chamedoon.Domin.Entity.User;
using System.Linq;

namespace Chamedoon.Application.Services.Admin.UserManagement.Query
{

    public class FilterUserAdminPanel
    {
        private readonly IApplicationDbContext _context;
        public FilterUserAdminPanel(IApplicationDbContext context)
        {
            _context = context;
        }
        private IQueryable<User> CheckFiltersShouldApplied(AdminUserManagement_VM? model)
        {

            IQueryable<User> filter = _context.User;
            if (model == null)
            {
                return filter;
            }
            if (string.IsNullOrEmpty(model.UserName) is false)
            {
                filter = filter.Where(x =>(x.UserName?? string.Empty).Contains(model.UserName));
            }
            if (string.IsNullOrEmpty(model.Email) is false)
            {
                filter = filter.Where(x => (x.Email ?? string.Empty).Contains(model.Email));
            }

            return filter;
        }

        public IQueryable<User> ApplyAdminPanelUserFilters(AdminUserManagement_VM? model)
        {
            return CheckFiltersShouldApplied(model);
        }
    }


}
