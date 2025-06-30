using System.Diagnostics;
using CyberLibrary2.Contratos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Cyberlibrary.Controllers;

public class BibliotecarioController : Controller
{
    private readonly IEmprestimoR _emprestimoRep;
    private readonly IUsuarioR _usuarioRep;
    private readonly ILivroR _livroRep;
    private readonly ICategoriaR _categoriaRep;
    private readonly ISetorR _setorRep;

    public BibliotecarioController(IEmprestimoR emprestimoRep, IUsuarioR usuarioRep, ILivroR livroRep, ICategoriaR categoriaRep, ISetorR setorRep)
    {
        _emprestimoRep = emprestimoRep;
        _usuarioRep = usuarioRep;
        _livroRep = livroRep;
        _categoriaRep = categoriaRep;
        _setorRep = setorRep;
    }


    [Authorize]
    public IActionResult Index()
    {
        return View();
    }

    [HttpGet]
    public IActionResult GetDashboardCounts()
    {
        var activeLoansCount = _emprestimoRep.ListarEmprestimos().Count(e => !e.Devolvido);
        var usersCount = _usuarioRep.ListarUsuarios().Count(u => u.Cargo != "Bibliotecario");
        var availableBooksCount = _livroRep.listarLivros().Sum(l => l.QuantidadeDisponivel);
        var categoriesCount = _categoriaRep.ListarCategorias().Count();

        return Json(new
        {
            activeLoans = activeLoansCount,
            users = usersCount,
            availableBooks = availableBooksCount,
            categories = categoriesCount
        });
    }

    [HttpGet]
    public IActionResult GetRecentLoansChartData()
    {
        // This is a simplified example. You'd need to group loans by month.
        // For actual implementation, consider fetching data for the last 6-12 months.
        var recentLoans = _emprestimoRep.ListarEmprestimos()
                                        .Where(e => e.DataEmprestimo >= DateTime.Today.AddMonths(-6))
                                        .GroupBy(e => new { e.DataEmprestimo.Year, e.DataEmprestimo.Month })
                                        .OrderBy(g => g.Key.Year)
                                        .ThenBy(g => g.Key.Month)
                                        .Select(g => new { Month = $"{g.Key.Month}/{g.Key.Year}", Count = g.Count() })
                                        .ToList();

        return Json(recentLoans);
    }

    [HttpGet]
    public IActionResult GetBooksBySectorChartData()
    {
        var booksBySector = _livroRep.listarLivros()
                                     .GroupBy(l => l.Setor ?? "Sem Setor") // Directly use l.Setor, coalesce to "Sem Setor" if null
                                     .Select(g => new { Sector = g.Key, Count = g.Count() })
                                     .ToList();
        return Json(booksBySector);
    }
    [HttpGet]
    public IActionResult GetLatestLoans()
    {
        var latestLoans = _emprestimoRep.ListarEmprestimos()
                                        .OrderByDescending(e => e.DataEmprestimo)
                                        .Take(10) // Get the 10 most recent loans
                                        .Select(e => new
                                        {
                                            BookTitle = e.Livro.Titulo,
                                            UserName = e.Usuario.Nome,
                                            LoanDate = e.DataEmprestimo.ToShortDateString(),
                                            ReturnDate = e.DataDevolucaoReal?.ToShortDateString() ?? "Não Devolvido",
                                            Status = e.Devolvido ? "Devolvido" : "Ativo"
                                        })
                                        .ToList();
        return Json(new { data = latestLoans }); // DataTables expects "data" property
    }

}
