using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Cyberlibrary.Controllers;

public class BibliotecarioController : Controller
{
    private readonly ILogger<BibliotecarioController> _logger;

    public BibliotecarioController(ILogger<BibliotecarioController> logger)
    {
        _logger = logger;
    }

    [Authorize]
    public IActionResult Index()
    {
        return View();
    }
}
