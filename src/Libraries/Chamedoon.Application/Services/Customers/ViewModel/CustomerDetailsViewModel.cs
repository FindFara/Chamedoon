using AutoMapper;
using Chamedoon.Application.Common.Utilities.AutoMapper;
using Chamedoon.Application.Services.Account.Users.ViewModel;

namespace Chamedoon.Application.Services.Customers.ViewModel
{
    public class CustomerDetailsViewModel : IMapFrom<Domin.Entity.Customers.Customer>
    {
        public string? FirstName { get; set; } 
        public string? LastName { get; set; }
        public string? Job { get; set; }
        public string? Description { get; set; } 


        public UserDetails_VM? User { get; set; }
    }
}
