using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Chamedoon.Domin.Entity.Users;
using Chamedoon.Domin.Entity.Permissions;

namespace Chamedoon.Infrastructure.FluentConfigs.Users;
public class UserConfigs : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {

        builder.ToTable("User");
        //List<User> users = new List<User>()
        //{
        //    new User()
        //    {
        //        Id = 1,
        //        UserName ="Fara",
        //        Email ="farakhakpour@gmail.com",
        //        NormalizedEmail ="FARAKHAKPOUR@GMAIL.COM",
        //        PhoneNumber ="09032383326",
        //        EmailConfirmed = true,
        //        Created = DateTime.Now,
        //        PasswordHash  ="AQAAAAIAAYagAAAAENOL+hqKevXrCIxtNuT/pOk0vKIrqTX3JwxqjQOnZ1O4RUpUj70uXwG6mcdFB1tj3w==",
        //        SecurityStamp ="",
        //        ConcurrencyStamp="",
        //        LastModified = DateTime.Now,
        //        NormalizedUserName ="FARA",
        //        TwoFactorEnabled = true,
        //        UserRoles = new List<UserRole>()
        //        {
        //            new UserRole() 
        //            {
        //                RoleId = 1,
        //                UserId =1
        //            }
        //        }
        //    },
        //};

        //builder.HasData(users);


    }
}
