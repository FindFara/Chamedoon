using Chamedoon.Application.Common.Utilities.AutoMapper;
using Chamedoon.Domin.Entity.Customers;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chamedoon.Application.Services.Customers.ViewModel
{
    public class AddCustomerViewModel : IMapFrom<Customer>
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Job { get; set; }
        public string? Description { get; set; }
        public IFormFile? ProfileImage { get; set; }
    }
}
