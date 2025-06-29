using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Cyberlibrary.Controllers;

public class AlunoController : Controller
{
    private readonly ILogger<AlunoController> _logger;

    public AlunoController(ILogger<AlunoController> logger)
    {
        _logger = logger;
    }

    [Authorize]
    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Emprestimos()
    {
        return View();
    }

    public IActionResult Devolvidos()
    {
        return View();
    }

    public IActionResult MeusLivros()
    {
        return View();
    }

    public IActionResult Notificacoes()
    {
        return View();
    }
}
