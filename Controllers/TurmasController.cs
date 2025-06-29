using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using CyberLibrary2.Models;
using CyberLibrary2.Contratos;
using Microsoft.AspNetCore.Authorization;

namespace CyberLibrary2.Controllers;

public class TurmasController : Controller
{
    private readonly ITurmaR _turmaRep;

    public TurmasController(ITurmaR turmaRep)
    {
        _turmaRep = turmaRep;
    }

    [Authorize]
    public IActionResult Index()
    {
        List<Turma> turmas = _turmaRep.ListarTurmas();
        return View(turmas);
    }

    public IActionResult Adicionar()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Adicionar(Turma turma)
    {
        if (ModelState.IsValid)
        {
            _turmaRep.Adicionar(turma);
            TempData["MensagemSucesso"] = "Turma adicionada com sucesso!";
            return RedirectToAction("Index");
        }
        return View(turma);
    }

    public IActionResult Editar(int id)
    {
        var turma = _turmaRep.BuscarId(id);
        if (turma == null)
        {
            TempData["MensagemErro"] = "Turma não encontrada.";
            return RedirectToAction("Index");
        }
        return View(turma);
    }

    [HttpPost]
    public IActionResult Atualizar(Turma turma)
    {
        if (ModelState.IsValid)
        {
            try
            {
                _turmaRep.Atualizar(turma);
                TempData["MensagemSucesso"] = "Turma atualizada com sucesso!";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["MensagemErro"] = $"Erro ao atualizar turma: {ex.Message}";
                Debug.WriteLine($"Erro ao atualizar turma: {ex.Message}");
            }
        }
        return View(turma);
    }

    [HttpPost]
    public IActionResult Excluir(int id)
    {
        try
        {
            _turmaRep.Excluir(id);
            TempData["MensagemSucesso"] = "Turma excluída com sucesso!";
        }
        catch (Exception ex)
        {
            TempData["MensagemErro"] = $"Erro ao excluir turma: {ex.Message}";
            Debug.WriteLine($"Erro ao excluir turma: {ex.Message}");
        }
        return RedirectToAction("Index");
    }
}