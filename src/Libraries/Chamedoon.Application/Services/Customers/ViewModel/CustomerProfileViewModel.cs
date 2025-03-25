
using Chamedoon.Application.Common.Utilities.AutoMapper;
using Chamedoon.Domin.Entity.Customers;

namespace Chamedoon.Application.Services.Customers.ViewModel
{
    public class CustomerProfileViewModel : IMapFrom<Customer>
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? ProfileImage { get; set; }
    }
}
