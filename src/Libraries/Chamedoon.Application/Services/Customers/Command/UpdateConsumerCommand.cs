using AutoMapper;
using Chamedoon.Application.Common.Interfaces;
using Chamedoon.Application.Common.Models;
using Chamedoon.Application.Services.Customers.ViewModel;
using Chamedoon.Domin.Entity.Customers;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Chamedoon.Application.Services.Customers.Command
{
    public class UpdateCustomerCommand : IRequest<OperationResult>
    {
        public required UpsertCustomerViewModel UpsertCustomerViewModel { get; set; }
    }
    public class UpdateCustomerCommandHandler : IRequestHandler<UpdateCustomerCommand, OperationResult>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public UpdateCustomerCommandHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<OperationResult> Handle(UpdateCustomerCommand request, CancellationToken cancellationToken)
        {
            Customer? customer = await _context.Customers.SingleOrDefaultAsync(c => c.Id == request.UpsertCustomerViewModel.Id);

            if (customer == null)
                return OperationResult.Fail();

            if (request.UpsertCustomerViewModel.ProfileImageFile?.Length > 0)
            {
                request.UpsertCustomerViewModel.ProfileImage = SaveProfileImageAsBase64(request.UpsertCustomerViewModel.ProfileImageFile);
            }
            else if (!string.IsNullOrEmpty(customer.ProfileImage))
            {
                request.UpsertCustomerViewModel.ProfileImage = TryNormalizeStoredImage(customer.ProfileImage);
            }

            _mapper.Map(request.UpsertCustomerViewModel, customer);
            _context.CustomerReports.Add(new CustomerReport
            {
                CustomerId = customer.Id,
                Age = customer.Age,
                MaritalStatus = customer.MaritalStatus,
                MbtiType = customer.MbtiType,
                InvestmentAmount = customer.InvestmentAmount,
                JobCategory = customer.JobCategory,
                JobTitle = customer.Job,
                WorkExperienceYears = customer.WorkExperienceYears,
                FieldCategory = customer.FieldCategory,
                EducationLevel = customer.EducationLevel,
                LanguageCertificate = customer.LanguageCertificate,
                WantsFurtherEducation = customer.WantsFurtherEducation,
                Description = customer.Description,
                PhoneNumber = customer.PhoneNumber,
                CreatedAtUtc = DateTime.UtcNow
            });
            await _context.SaveChangesAsync(cancellationToken);
            return OperationResult.Success();
        }
        private static string SaveProfileImageAsBase64(IFormFile profileImage)
        {
            using var ms = new MemoryStream();
            profileImage.CopyTo(ms);
            var base64 = Convert.ToBase64String(ms.ToArray());
            var contentType = string.IsNullOrWhiteSpace(profileImage.ContentType) ? "image/png" : profileImage.ContentType;
            return $"data:{contentType};base64,{base64}";
        }

        private static string TryNormalizeStoredImage(string storedValue)
        {
            if (storedValue.StartsWith("data:"))
            {
                return storedValue;
            }

            var relativePath = storedValue.TrimStart('/').Replace("\\", "/");
            var physicalPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", relativePath);

            if (File.Exists(physicalPath))
            {
                var bytes = File.ReadAllBytes(physicalPath);
                var base64 = Convert.ToBase64String(bytes);
                var extension = Path.GetExtension(physicalPath).ToLowerInvariant();
                var contentType = extension switch
                {
                    ".jpg" or ".jpeg" => "image/jpeg",
                    ".gif" => "image/gif",
                    ".webp" => "image/webp",
                    _ => "image/png"
                };
                return $"data:{contentType};base64,{base64}";
            }

            return storedValue;
        }
    }
}
