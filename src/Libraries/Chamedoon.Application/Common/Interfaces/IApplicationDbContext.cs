using Chamedoon.Domin.Entity.Blogs;
using Chamedoon.Domin.Entity.Customers;
using Chamedoon.Domin.Entity.Permissions;
using Chamedoon.Domin.Entity.Payments;
using Chamedoon.Domin.Entity.Users;
using Microsoft.EntityFrameworkCore;

namespace Chamedoon.Application.Common.Interfaces
{
    public interface IApplicationDbContext
    {

        #region User
        public DbSet<User> User { get; set; }
        public DbSet<UserRole> UserRole { get; set; }

        #endregion

        #region Permission
        public DbSet<Role> Role { get; set; }
        public DbSet<RolePermission> RolePermission { get; set; }

        #endregion

        #region Blog
        public DbSet<Article> Article { get; set; }
        public DbSet<ArticleComment> ArticleComment { get; set; }
        #endregion


        #region Customer
        public DbSet<Customer> Customers { get; set; }
        public DbSet<CustomerReport> CustomerReports { get; set; }
        #endregion

        #region Payments
        public DbSet<PaymentRequest> PaymentRequests { get; set; }
        public DbSet<PaymentResponse> PaymentResponses { get; set; }
        #endregion

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
