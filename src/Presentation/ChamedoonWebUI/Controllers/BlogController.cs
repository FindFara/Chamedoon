using AutoMapper;
using Chamedoon.Application.Services.Blog.Query;
using Chamedoon.Application.Services.Blog.ViewModel;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ChamedoonWebUI.Controllers
{
    public class BlogController : Controller
    {
        private readonly IMediator mediator;
        private readonly IMapper mapper;

        public BlogController(IMediator mediator, IMapper mapper)
        {
            this.mediator = mediator;
            this.mapper = mapper;
        }


        public async Task<IActionResult> Index(string? search, string? writer, DateTime? dateFrom, DateTime? dateTo, int page = 1, int pageSize = 8)
        {
            var result = await mediator.Send(new GetBlogByPaginationQuery
            {
                PageNumber = page,
                PageSize = pageSize,
                Search = search,
                Writer = writer,
                DateFrom = dateFrom,
                DateTo = dateTo
            });

            if (result.IsSuccess == false || result.Result is null)
            {
                var emptyModel = new ChamedoonWebUI.Models.BlogIndexViewModel
                {
                    Articles = new List<BlogViewModel>(),
                    CurrentPage = page,
                    TotalPages = 0,
                    Search = search,
                    Writer = writer,
                    DateFrom = dateFrom,
                    DateTo = dateTo
                };
                return View(emptyModel);
            }

            var paginated = result.Result;
            var model = new Models.BlogIndexViewModel
            {
                Articles = paginated.Items,
                CurrentPage = paginated.PageNumber,
                TotalPages = paginated.TotalPages,
                Search = search,
                Writer = writer,
                DateFrom = dateFrom,
                DateTo = dateTo
            };
            return View(model);
        }
        public async Task<IActionResult> Detail(long id)
        {
            if (id == 0)
            {
                return NotFound();
            }
            var result = await mediator.Send(new GetBlogQuery { Id = id });
            if (result.IsSuccess == false || result.Result == null)
            {
                return NotFound();
            }
            return View(result.Result);
        }
    }
}
