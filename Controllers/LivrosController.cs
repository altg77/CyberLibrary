using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using CyberLibrary2.Models;
using CyberLibrary2.Contratos;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.AspNetCore.Authorization;
using System.Linq;

namespace CyberLibrary2.Controllers;

public class LivrosController : Controller
{
    private readonly ILivroR livro_rep;
    private readonly ICategoriaR _categoriaRep;
    private readonly ISetorR _setorRep;


    public LivrosController(ILivroR livroRep, ICategoriaR categoriaRep, ISetorR setorRep)
    {
        livro_rep = livroRep;
        _categoriaRep = categoriaRep;
        _setorRep = setorRep;
    }

    [Authorize]
    public IActionResult Index()
    {
        List<Livro> livros = livro_rep.listarLivros();
        return View(livros);
    }

    public IActionResult Adicionar()
    {
        ViewBag.CategoriasDisponiveis = _categoriaRep.ListarCategorias();
        ViewBag.SetoresDisponiveis = _setorRep.ListarSetores();
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Adicionar(Livro livro, IFormFile capaArq)
    {
        if (ModelState.IsValid)
        {
            if (capaArq != null && capaArq.Length > 0)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await capaArq.CopyToAsync(memoryStream);
                    livro.CapaImagem = memoryStream.ToArray();
                }
            }

            livro_rep.adicionar(livro);
            return RedirectToAction("Index");
        }

        ViewBag.SetoresDisponiveis = _setorRep.ListarSetores();
        ViewBag.CategoriasDisponiveis = _categoriaRep.ListarCategorias();
        return View(livro);
    }

    public IActionResult Editar(int id)
    {
        var livro = livro_rep.buscarId(id);
        return View(livro);
    }

    [HttpPost]
    public async Task<IActionResult> Atualizar(Livro livro, IFormFile? capaArq)
    {
        if (ModelState.IsValid)
        {
            if (capaArq != null && capaArq.Length > 0)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await capaArq.CopyToAsync(memoryStream);
                    livro.CapaImagem = memoryStream.ToArray();
                }
            }
        }

        livro_rep.Atualizar(livro);
        return RedirectToAction("Index");
    }

    [HttpPost]
    public IActionResult Excluir(int id)
    {
        try
        {
            livro_rep.excluir(id);
            TempData["MensagemSucesso"] = "Livro excluído com sucesso!";
        }
        catch (Exception ex)
        {
            TempData["MensagemErro"] = $"Erro ao excluir livro: {ex.Message}";
            Debug.WriteLine($"Erro ao excluir livro: {ex.Message}");
        }

        return RedirectToAction("Index");
    }


}
