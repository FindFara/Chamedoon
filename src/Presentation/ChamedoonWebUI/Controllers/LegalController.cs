using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ChamedoonWebUI.Controllers;

[AllowAnonymous]
public class LegalController : Controller
{
    [HttpGet("privacy")]
    public IActionResult Privacy() => View();

    [HttpGet("terms")]
    public IActionResult Terms() => View();

    [HttpGet("support")]
    public IActionResult Support() => View();
}
