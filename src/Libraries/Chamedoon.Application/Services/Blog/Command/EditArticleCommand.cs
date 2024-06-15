using AutoMapper;
using Chamedoon.Application.Common.Interfaces;
using Chamedoon.Application.Common.Models;
using Chamedoon.Application.Services.Blog.ViewModel;
using MediatR;
using Chamedoon.Domin.Entity.Blogs;
using Microsoft.EntityFrameworkCore;

namespace Chamedoon.Application.Services.Blog.Command
{
    public class EditArticleCommand : IRequest<OperationResult<bool>>
    {
        public Article_VM Article { get; set; } = new();
    }
    public class EditArticleCommandHandler : IRequestHandler<EditArticleCommand, OperationResult<bool>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public EditArticleCommandHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<OperationResult<bool>> Handle(EditArticleCommand request, CancellationToken cancellationToken)
        {
            Article? oldArticle = _context.Article.AsNoTracking().FirstOrDefault(a => a.Id == request.Article.Id);
            if (oldArticle == null)
            {
                return OperationResult<bool>.Fail("مقاله ای با این مشخصات یافت نشد ");
            }
            _context.Article.Update(_mapper.Map<Article>(request.Article));
            await _context.SaveChangesAsync(cancellationToken);
            return OperationResult<bool>.Success(true);
        }
    }
}
