using AutoMapper;
using Chamedoon.Application.Common.Interfaces;
using Chamedoon.Application.Common.Models;
using Chamedoon.Application.Services.Blog.ViewModel;
using Chamedoon.Domin.Entity.Blogs;
using Chamedoon.Domin.Entity.Users;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Chamedoon.Application.Services.Blog.Query
{
    public class GetBlogQuery : IRequest<OperationResult<ArticleViewModel>>
    {
        public long Id { get; set; }
        public string? Title { get; set; }
    }
    public class GetBlogQueryHandler : IRequestHandler<GetBlogQuery, OperationResult<ArticleViewModel>>
    {
        #region Property
        private readonly IApplicationDbContext _context;
        private readonly IMapper mapper;
        #endregion

        #region Ctor
        public GetBlogQueryHandler(IApplicationDbContext context , IMapper mapper)
        {
            _context = context;
            this.mapper = mapper;
        }
        #endregion

        #region Method
        public async Task<OperationResult<ArticleViewModel>> Handle(GetBlogQuery request, CancellationToken cancellationToken)
        {

            Article? article = await _context.Article
                      .SingleOrDefaultAsync(u =>
                           u.Id == request.Id ||
                           u.ArticleTitle == (request.Title ?? ""));
            if (article is null)
            {
                return OperationResult<ArticleViewModel>.Fail("مقاله ای با مشخصات واد شده یافت نشد");
            }

            article.VisitCount += 1;
            await _context.SaveChangesAsync(cancellationToken);

            return OperationResult<ArticleViewModel>.Success(mapper.Map<ArticleViewModel>(article));
        }

        #endregion
    }

}
