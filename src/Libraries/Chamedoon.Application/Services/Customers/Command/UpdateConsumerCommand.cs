using AutoMapper;
using Chamedoon.Application.Common.Interfaces;
using Chamedoon.Application.Common.Models;
using Chamedoon.Application.Services.Customers.ViewModel;
using Chamedoon.Domin.Entity.Customers;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Chamedoon.Application.Services.Customers.Command
{
    public class UpdateCustomerCommand : IRequest<OperationResult>
    {
        public required UpsertCustomerViewModel UpsertCustomerViewModel { get; set; }
        public required long Id { get; set; }

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
            Customer? customer = await _context.Customers.FindAsync(request.Id);

            if (customer == null)
                return OperationResult.Fail();

            if (request.UpsertCustomerViewModel.ProfileImage?.Length > 0)
            {
                var filePath = SaveProfileImage(request.UpsertCustomerViewModel.ProfileImage);
                customer.ProfileImage = filePath;
            }
            _mapper.Map(request.UpsertCustomerViewModel, customer);
            _context.Customers.Update(customer);

            await _context.SaveChangesAsync(cancellationToken);
            return OperationResult.Success();
        }
        private string SaveProfileImage(IFormFile profileImage)
        {
            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/users");

            if (!Directory.Exists(uploadsFolder)) Directory.CreateDirectory(uploadsFolder);

            var uniqueFileName = Guid.NewGuid() + Path.GetExtension(profileImage.FileName);
            var filePath = Path.Combine(uploadsFolder, uniqueFileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                profileImage.CopyTo(stream);
            }

            return $"/images/users/{uniqueFileName}";
        }
    }
}
