using Microsoft.AspNetCore.Http;

namespace Chamedoon.Application.Services.Customers.ViewModel;

public class UpsertCustomerViewModel
{
    public long Id { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Job { get; set; }
    public string? Description { get; set; }
    public IFormFile? ProfileImage { get; set; }
}
