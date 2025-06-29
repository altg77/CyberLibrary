using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using CyberLibrary2.Models;
using CyberLibrary2.Contratos;
using Microsoft.AspNetCore.Authorization;

namespace CyberLibrary2.Controllers;

public class CategoriasController : Controller
{
    private readonly ICategoriaR _categoriaRep;

    public CategoriasController(ICategoriaR categoriaRep)
    {
        _categoriaRep = categoriaRep;
    }

    [Authorize]
    public IActionResult Index()
    {
        List<Categoria> categorias = _categoriaRep.ListarCategorias();
        return View(categorias);
    }

    public IActionResult Adicionar()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Adicionar(Categoria categoria)
    {
        if (ModelState.IsValid)
        {
            _categoriaRep.Adicionar(categoria);
            TempData["MensagemSucesso"] = "Categoria adicionada com sucesso!";
            return RedirectToAction("Index");
        }
        return View(categoria);
    }

    public IActionResult Editar(int id)
    {
        var categoria = _categoriaRep.BuscarId(id);
        if (categoria == null)
        {
            TempData["MensagemErro"] = "Categoria não encontrada.";
            return RedirectToAction("Index");
        }
        return View(categoria);
    }

    [HttpPost]
    public IActionResult Atualizar(Categoria categoria)
    {
        if (ModelState.IsValid)
        {
            try
            {
                _categoriaRep.Atualizar(categoria);
                TempData["MensagemSucesso"] = "Categoria atualizada com sucesso!";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["MensagemErro"] = $"Erro ao atualizar categoria: {ex.Message}";
                Debug.WriteLine($"Erro ao atualizar categoria: {ex.Message}");
            }
        }
        return View(categoria);
    }

    [HttpPost]
    public IActionResult Excluir(int id)
    {
        try
        {
            _categoriaRep.Excluir(id);
            TempData["MensagemSucesso"] = "Categoria excluída com sucesso!";
        }
        catch (Exception ex)
        {
            TempData["MensagemErro"] = $"Erro ao excluir categoria: {ex.Message}";
            Debug.WriteLine($"Erro ao excluir categoria: {ex.Message}");
        }
        return RedirectToAction("Index");
    }
}