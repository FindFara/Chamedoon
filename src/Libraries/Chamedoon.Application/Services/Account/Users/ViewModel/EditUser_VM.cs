using Chamedoon.Application.Common.Utilities.AutoMapper;
using Chamedoon.Domin.Entity.Users;
using Microsoft.AspNetCore.Http;

namespace Chamedoon.Application.Services.Account.Users.ViewModel;
public class EditUser_VM : IMapFrom<User>
{
    public long? Id { get; set; }
    public string? UserName { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string Job { get; set; }
    public string Description { get; set; }
    public string ProfileImage { get; set; } 
    public IFormFile ProfileImageFile { get; set; }

}
