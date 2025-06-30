using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using CyberLibrary2.Models;
using CyberLibrary2.Contratos;
using Microsoft.AspNetCore.Authorization;
using System.Linq;
using System.Security.Claims;
using CyberLibrary2.Models.ViewModel; 

namespace CyberLibrary2.Controllers;

[Authorize]
public class AlunoController : Controller
{
    private readonly ILivroR _livroRep;
    private readonly IEmprestimoR _emprestimoRep;
    private readonly IUsuarioR _usuarioRep;

    public AlunoController(ILivroR livroRep, IEmprestimoR emprestimoRep, IUsuarioR usuarioRep)
    {
        _livroRep = livroRep;
        _emprestimoRep = emprestimoRep;
        _usuarioRep = usuarioRep;
    }

    public IActionResult Index()
    {
        var viewModel = new Filtro();
        int? currentUserId = null;

        if (User.Identity.IsAuthenticated)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim != null && int.TryParse(userIdClaim.Value, out int id))
            {
                currentUserId = id;
            }
        }

        viewModel.RecommendedBooks = GetRecommendedBooks(currentUserId);

        return View(viewModel);
    }

     public IActionResult Emprestimos()
    {
        var viewModel = new Filtro();
        int? currentUserId = null;

        if (User.Identity.IsAuthenticated)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim != null && int.TryParse(userIdClaim.Value, out int id))
            {
                currentUserId = id;
            }
        }

        viewModel.RecentBooks = GetRecentBooks();
        viewModel.ExploreBooks = GetRandomBooks(); 
        viewModel.MostBorrowedBooks = GetMostBorrowedBooks();
        viewModel.TopRatedBooks = GetTopRatedBooks();
        viewModel.Mangas = GetBooksBySector("Manga");
        viewModel.MachadoDeAssisBooks = GetBooksByAuthor("Machado de Assis");

        return View(viewModel);
    }

    private List<Livro> GetRecommendedBooks(int? userId)
    {
        if (!userId.HasValue)
        {
            return GetRandomBooks();
        }

        var borrowedBooks = _emprestimoRep.ListarEmprestimos()
            .Where(e => e.UsuarioId == userId.Value)
            .Select(e => e.Livro)
            .ToList();

        if (!borrowedBooks.Any())
        {
            return GetRandomBooks();
        }

        var borrowedBookCategoryNames = borrowedBooks
            .Where(l => !string.IsNullOrEmpty(l.Categoria))
            .Select(l => l.Categoria)
            .Distinct()
            .ToList();

        var borrowedBookSectorNames = borrowedBooks
            .Where(l => !string.IsNullOrEmpty(l.Setor))
            .Select(l => l.Setor)
            .Distinct()
            .ToList();

        var borrowedBookIds = borrowedBooks.Select(l => l.Id).ToList();

        var recommendedBooks = _livroRep.listarLivros()
            .Where(l =>
                (borrowedBookCategoryNames.Contains(l.Categoria) || borrowedBookSectorNames.Contains(l.Setor)) &&
                !borrowedBookIds.Contains(l.Id))
            .Take(10)
            .ToList();

        if (!recommendedBooks.Any())
        {
            return GetRandomBooks();
        }

        return recommendedBooks;
    }

    private List<Livro> GetRecentBooks()
    {
        return _livroRep.listarLivros().OrderByDescending(l => l.Id).Take(10).ToList();
    }

    private List<Livro> GetRandomBooks()
    {
        return _livroRep.listarLivros().OrderBy(l => Guid.NewGuid()).Take(10).ToList();
    }

    private List<Livro> GetMostBorrowedBooks()
    {
        return _emprestimoRep.ListarEmprestimos()
            .GroupBy(e => e.LivroId)
            .OrderByDescending(g => g.Count())
            .Select(g => _livroRep.buscarId(g.Key))
            .Where(l => l != null)
            .Take(10)
            .ToList();
    }

    private List<Livro> GetTopRatedBooks()
    {
        return _livroRep.listarLivros().Take(5).ToList();
    }

    private List<Livro> GetBooksBySector(string sectorName)
    {
        return _livroRep.listarLivros()
            .Where(l => l.Setor != null && l.Setor.Equals(sectorName, StringComparison.OrdinalIgnoreCase))
            .ToList();
    }

    private List<Livro> GetBooksByAuthor(string authorName)
    {
        return _livroRep.listarLivros()
            .Where(l => l.Autor != null && l.Autor.Contains(authorName, StringComparison.OrdinalIgnoreCase))
            .ToList();
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
