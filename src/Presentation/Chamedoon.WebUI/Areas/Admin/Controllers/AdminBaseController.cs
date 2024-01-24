using Microsoft.AspNetCore.Mvc;

namespace Chamedoon.WebUI.Areas.Admin.Controllers
{
    public class AdminBaseController : Controller
    {
        [Area("Admins")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
