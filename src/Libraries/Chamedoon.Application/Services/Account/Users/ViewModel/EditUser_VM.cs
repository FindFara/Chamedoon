using Chamedoon.Application.Common.Utilities.AutoMapper;
using Chamedoon.Domin.Entity.Users;
using Chamedoon.Domin.Enums;
using Microsoft.AspNetCore.Http;

namespace Chamedoon.Application.Services.Account.Users.ViewModel;
public class EditUser_VM 
{
    public long? Id { get; set; }
    public string? UserName { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Email { get; set; }
    public string? Job { get; set; }
    public JobCategory JobCategory { get; set; }
    public int WorkExperienceYears { get; set; }
    public string? Description { get; set; }
    public string? PhoneNumber { get; set; }
    public Gender? Gender { get; set; }
    public int Age { get; set; }
    public MaritalStatus MaritalStatus { get; set; }
    public string? MbtiType { get; set; }
    public decimal InvestmentAmount { get; set; }
    public string? FieldCategory { get; set; }
    public EducationLevel EducationLevel { get; set; }
    public string? LanguageCertificate { get; set; }
    public bool WantsFurtherEducation { get; set; }
    public IFormFile? ProfileImageFile { get; set; }

}
