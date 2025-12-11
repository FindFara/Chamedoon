using AutoMapper;
using Chamedoon.Application.Common.Interfaces;
using Chamedoon.Application.Common.Models;
using Chamedoon.Application.Services.Blog.ViewModel;
using Chamedoon.Domin.Entity.Blogs;
using MediatR;

namespace Chamedoon.Application.Services.Blog.Command
{
    public class DeleteArticleCommand : IRequest<OperationResult<bool>>
    {
        public ArticleViewModel Article { get; set; } = new();
    }
    public class DeleteArticleCommandHandler : IRequestHandler<DeleteArticleCommand, OperationResult<bool>>
    {
        #region Property
        private readonly IApplicationDbContext _context;
        #endregion

        #region Ctor
        public DeleteArticleCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }
        #endregion

        #region Method
        public async Task<OperationResult<bool>> Handle(DeleteArticleCommand request, CancellationToken cancellationToken)
        {

            Article? article = _context.Article.FirstOrDefault(a => a.Id == request.Article.Id);
            if (article == null)
            {
                return OperationResult<bool>.Fail("مقاله ای با این مشخصات یافت نشد ");
            }

            article.IsDeleted = true;
            _context.Article.Update(article);
            await _context.SaveChangesAsync(cancellationToken);
            return OperationResult<bool>.Success(true);
        }

        #endregion
    }

}
