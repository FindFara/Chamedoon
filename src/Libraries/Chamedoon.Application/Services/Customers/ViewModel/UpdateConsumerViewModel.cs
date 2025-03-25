using AutoMapper;
using Chamedoon.Application.Common.Utilities.AutoMapper;
using Chamedoon.Application.Services.Account.Users.ViewModel;
using Chamedoon.Domin.Entity.Customers;
using Microsoft.AspNetCore.Http;

namespace Chamedoon.Application.Services.Customers.ViewModel;

public class UpsertCustomerViewModel : IMapFrom<EditUser_VM>, IMapFrom<Customer>
{
    public required long Id { get; set; }

    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Job { get; set; }
    public string? Description { get; set; }
    public string? ProfileImage { get; set; }
    public IFormFile? ProfileImageFile { get; set; }
}
