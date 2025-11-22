using AutoMapper;
using Chamedoon.Application.Common.Utilities.AutoMapper;
using Chamedoon.Application.Services.Account.Users.ViewModel;
using Chamedoon.Domin.Entity.Customers;
using Chamedoon.Domin.Enums;
using Microsoft.AspNetCore.Http;

namespace Chamedoon.Application.Services.Customers.ViewModel;

public class UpsertCustomerViewModel : IMapFrom<EditUser_VM>, IMapFrom<Customer>
{
    public required long Id { get; set; }

    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Job { get; set; }
    public JobCategory JobCategory { get; set; }
    public int WorkExperienceYears { get; set; }
    public string? Description { get; set; }
    public string? PhoneNumber { get; set; }
    public string? ProfileImage { get; set; }
    public IFormFile? ProfileImageFile { get; set; }
    public Gender Gender { get; set; }
    public int Age { get; set; }
    public MaritalStatus MaritalStatus { get; set; }
    public string? MbtiType { get; set; }
    public decimal InvestmentAmount { get; set; }
    public string? FieldCategory { get; set; }
    public EducationLevel EducationLevel { get; set; }
    public string? LanguageCertificate { get; set; }
    public bool WantsFurtherEducation { get; set; }

}
