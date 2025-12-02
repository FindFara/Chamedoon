using AutoMapper;
using Chamedoon.Application.Common.Interfaces;
using Chamedoon.Application.Common.Models;
using Chamedoon.Application.Services.Customers.ViewModel;
using Chamedoon.Application.Services.Customers;
using Chamedoon.Domin.Entity.Customers;
using MediatR;
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
                request.UpsertCustomerViewModel.ProfileImage = ProfileImageHelper.ConvertToBase64(request.UpsertCustomerViewModel.ProfileImageFile);
            }
            else
            {
                request.UpsertCustomerViewModel.ProfileImage = ProfileImageHelper.NormalizeProfileImage(customer.ProfileImage);
            }
            _mapper.Map(request.UpsertCustomerViewModel, customer);
            await _context.SaveChangesAsync(cancellationToken);
            return OperationResult.Success();
        }
    }
}
