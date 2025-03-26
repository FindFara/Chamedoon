using AutoMapper;
using Chamedoon.Application.Common.Utilities.AutoMapper;
using Chamedoon.Application.Services.Account.Users.ViewModel;
using Chamedoon.Domin.Enums;
using Edition.Common.Extensions;

namespace Chamedoon.Application.Services.Customers.ViewModel
{
    public class CustomerDetailsViewModel : IMapFrom<Domin.Entity.Customers.Customer>
    {
        public string? FirstName { get; set; } 
        public string? LastName { get; set; }
        public string? Job { get; set; }
        public string? Description { get; set; }
        public string? ProfileImage { get; set; }
        public Gender? Gender { get; set; }
        public string[]? GenderList { get; set; } = EnumExtensions.GetEnumDescriptions<Gender>();
        public UserDetails_VM? User { get; set; }
    }
}
