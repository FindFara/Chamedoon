using Chamedoon.Application.Services.Blog.ViewModel;
using System;
using System.Collections.Generic;

namespace ChamedoonWebUI.Models;

public class HomeIndexViewModel
{
    public IReadOnlyList<BlogViewModel> PopularArticles { get; init; } = Array.Empty<BlogViewModel>();
}
