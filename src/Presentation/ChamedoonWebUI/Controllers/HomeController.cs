using Chamedoon.Application.Services.Blog.Query;
using Chamedoon.Application.Services.Blog.ViewModel;
using ChamedoonWebUI.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace ChamedoonWebUI.Controllers
{
    public class HomeController : Controller
    {
        private readonly IMediator _mediator;

        public HomeController(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<IActionResult> Index()
        {
            var result = await _mediator.Send(new GetPopularBlogsQuery { Count = 4 });
            IReadOnlyList<BlogViewModel> popularArticles = Array.Empty<BlogViewModel>();

            if (result.IsSuccess && result.Result is not null)
            {
                popularArticles = result.Result;
            }

            var model = new HomeIndexViewModel
            {
                PopularArticles = popularArticles
            };

            return View(model);
        }

        public IActionResult About()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
