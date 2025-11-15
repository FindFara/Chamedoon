using AutoMapper;
using AutoMapper.QueryableExtensions;
using Chamedoon.Application.Common.Interfaces;
using Chamedoon.Application.Common.Models;
using Chamedoon.Application.Common.Utilities.AutoMapper;
using Chamedoon.Application.Services.Blog.ViewModel;
using Chamedoon.Domin.Entity.Blogs;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Chamedoon.Application.Services.Blog.Query
{
    public class GetBlogByPaginationQuery : IRequest<OperationResult<PaginatedList<BlogViewModel>>>
    {
        public int PageSize { get; set; }
        public int PageNumber { get; set; } = 1;
        public string? Search { get; set; }
        public string? Writer { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
    }
    public class GetBlogByPaginationQueryHandler : IRequestHandler<GetBlogByPaginationQuery, OperationResult<PaginatedList<BlogViewModel>>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetBlogByPaginationQueryHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<OperationResult<PaginatedList<BlogViewModel>>> Handle(GetBlogByPaginationQuery request, CancellationToken cancellationToken)
        {
            IQueryable<Article> query = _context.Article.AsQueryable();

            if (!string.IsNullOrWhiteSpace(request.Search))
            {
                var term = request.Search.Trim();
                query = query.Where(a =>
                    a.ArticleTitle.Contains(term));
            }

            if (!string.IsNullOrWhiteSpace(request.Writer))
            {
                query = query.Where(a => a.Writer == request.Writer);
            }

            if (request.DateFrom.HasValue)
            {
                query = query.Where(a => a.Created >= request.DateFrom.Value);
            }
            if (request.DateTo.HasValue)
            {
                query = query.Where(a => a.Created <= request.DateTo.Value);
            }

            var pageSize = request.PageSize == 0 ? 8 : request.PageSize;
            var pageNumber = request.PageNumber == 0 ? 1 : request.PageNumber;

            var paginatedList = await query
                .OrderByDescending(a => a.Created)
                .ProjectTo<BlogViewModel>(_mapper.ConfigurationProvider)
                .PaginatedListAsync(pageNumber, pageSize);

            return OperationResult<PaginatedList<BlogViewModel>>.Success(paginatedList);
        }
    }
}