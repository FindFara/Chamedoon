using AutoMapper;
using Chamedoon.Application.Common.Interfaces;
using Chamedoon.Application.Common.Models;
using Chamedoon.Application.Services.Customers.ViewModel;
using Chamedoon.Domin.Entity.Customers;
using MediatR;

namespace Chamedoon.Application.Services.Customers.Command
{
    public class UpsertCustomerCommand : IRequest<OperationResult>
    {
        public required UpsertCustomerViewModel UpsertCustomerViewModel { get; set; }
    }
    public class UpsertCustomerCommandHandler : IRequestHandler<UpsertCustomerCommand, OperationResult>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public UpsertCustomerCommandHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<OperationResult> Handle(UpsertCustomerCommand request, CancellationToken cancellationToken)
        {
            var customer = await _context.Customers.FindAsync(request.UpsertCustomerViewModel.Id, cancellationToken);

            if (request.UpsertCustomerViewModel.ProfileImage != null &&
                request.UpsertCustomerViewModel.ProfileImage.Length > 0)
            {
                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/users");
                var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(request.UpsertCustomerViewModel.ProfileImage.FileName);
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await request.UpsertCustomerViewModel.ProfileImage.CopyToAsync(stream);
                }
                customer.ProfileImage = $"/images/users/{uniqueFileName}";
            }

            if (customer == null)
            {
                customer = _mapper.Map<Customer>(request.UpsertCustomerViewModel);
                await _context.Customers.AddAsync(customer, cancellationToken);
            }
            else
            {

                _mapper.Map(request.UpsertCustomerViewModel, customer);
                _context.Customers.Update(customer);
            }
            await _context.SaveChangesAsync(cancellationToken);

            return OperationResult.Success();
        }
    }
}
