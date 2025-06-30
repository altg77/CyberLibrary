using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using CyberLibrary2.Models;
using CyberLibrary2.Contratos;
using Microsoft.AspNetCore.Authorization;

namespace CyberLibrary2.Controllers;

public class SetoresController : Controller
{
    private readonly ISetorR _setorRep;

    public SetoresController(ISetorR setorRep)
    {
        _setorRep = setorRep;
    }

    [Authorize]
    public IActionResult Index()
    {
        List<Setor> setores = _setorRep.ListarSetores();
        return View(setores);
    }

    public IActionResult Adicionar()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Adicionar(Setor setor)
    {
        if (ModelState.IsValid)
        {
            _setorRep.Adicionar(setor);
            TempData["MensagemSucesso"] = "Setor adicionada com sucesso!";
            return RedirectToAction("Index");
        }
        return View(setor);
    }

    public IActionResult Editar(int id)
    {
        var turma = _setorRep.BuscarId(id);
        if (turma == null)
        {
            TempData["MensagemErro"] = "Setor não encontrada.";
            return RedirectToAction("Index");
        }
        return View(turma);
    }

    [HttpPost]
    public IActionResult Atualizar(Setor setor)
    {
        if (ModelState.IsValid)
        {
            try
            {
                _setorRep.Atualizar(setor);
                TempData["MensagemSucesso"] = "Setor atualizada com sucesso!";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["MensagemErro"] = $"Erro ao atualizar setor: {ex.Message}";
                Debug.WriteLine($"Erro ao atualizar setor: {ex.Message}");
            }
        }
        return View(setor);
    }

    [HttpPost]
    public IActionResult Excluir(int id)
    {
        try
        {
            _setorRep.Excluir(id);
            TempData["MensagemSucesso"] = "Setor excluída com sucesso!";
        }
        catch (Exception ex)
        {
            TempData["MensagemErro"] = $"Erro ao excluir setor: {ex.Message}";
            Debug.WriteLine($"Erro ao excluir setor: {ex.Message}");
        }
        return RedirectToAction("Index");
    }
}