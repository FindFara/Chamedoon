using AutoMapper;
using Chamedoon.Application.Common.Interfaces;
using Chamedoon.Application.Common.Models;
using Chamedoon.Application.Services.Blog.ViewModel;
using MediatR;
using Chamedoon.Domin.Entity.Blogs;

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
            Article? article = _context.Article.FirstOrDefault(a => a.Id == request.Article.Id);
            if (article == null)
            {
                return OperationResult<bool>.Fail("مقاله ای با این مشخصات یافت نشد ");
            }

            article = _mapper.Map<Article>(request.Article);

            _context.Article.Update(article);
            await _context.SaveChangesAsync(cancellationToken);
            return OperationResult<bool>.Success(true);
        }
    }
}
