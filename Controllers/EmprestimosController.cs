using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering; // Necessário para SelectList
using CyberLibrary2.Models;
using CyberLibrary2.Contratos;
using System.Linq;
using Microsoft.AspNetCore.Authorization;

namespace CyberLibrary2.Controllers;

public class EmprestimosController : Controller
{
    private readonly IEmprestimoR _emprestimoRep;
    private readonly ILivroR _livroRep;
    private readonly IUsuarioR _usuarioRep;

    public EmprestimosController(IEmprestimoR emprestimoRep, ILivroR livroRep, IUsuarioR usuarioRep)
    {
        _emprestimoRep = emprestimoRep;
        _livroRep = livroRep;
        _usuarioRep = usuarioRep;
    }

    [Authorize]
    public IActionResult Index()
    {
        List<Emprestimo> emprestimos = _emprestimoRep.ListarEmprestimos();
        return View(emprestimos);
    }

    public IActionResult Adicionar()
    {
        ViewBag.Livros = new SelectList(_livroRep.listarLivros().Where(l => l.Disponivel), "Id", "Titulo");
        ViewBag.Usuarios = new SelectList(_usuarioRep.ListarUsuarios(), "Id", "Nome");
        return View();
    }

     [HttpPost]
    public IActionResult Adicionar(Emprestimo emprestimo)
    {
        if (ModelState.IsValid)
        {
            try
            {
                _emprestimoRep.Adicionar(emprestimo);
                TempData["MensagemSucesso"] = "Empréstimo registrado com sucesso!";
            }
            catch (Exception ex)
            {
                TempData["MensagemErro"] = $"Erro ao registrar empréstimo: {ex.Message}";
                Debug.WriteLine($"Erro ao registrar empréstimo: {ex.Message}");
            }
        }
        else
        {
            TempData["MensagemErro"] = "Por favor, corrija os erros de validação no formulário.";
            foreach (var state in ModelState)
            {
                foreach (var error in state.Value.Errors)
                {
                    Debug.WriteLine($"Erro de Validação em {state.Key}: {error.ErrorMessage}");
                }
            }
        }

        return RedirectToAction("Index");
    }

    public IActionResult Editar(int id)
    {
        var emprestimo = _emprestimoRep.BuscarId(id);
        if (emprestimo == null)
        {
            TempData["MensagemErro"] = "Empréstimo não encontrado.";
            return RedirectToAction("Index");
        }

        // Preenche ViewBags com listas para dropdowns
        ViewBag.Livros = new SelectList(_livroRep.listarLivros(), "Id", "Titulo", emprestimo.LivroId);
        ViewBag.Usuarios = new SelectList(_usuarioRep.ListarUsuarios(), "Id", "Nome", emprestimo.UsuarioId);
        return View(emprestimo);
    }

    [HttpPost]
    public IActionResult Atualizar(Emprestimo emprestimo)
    {
        if (ModelState.IsValid)
        {
            try
            {
                _emprestimoRep.Atualizar(emprestimo);
                TempData["MensagemSucesso"] = "Empréstimo atualizado com sucesso!";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                TempData["MensagemErro"] = $"Erro ao atualizar empréstimo: {ex.Message}";
                Debug.WriteLine($"Erro ao atualizar empréstimo: {ex.Message}");
            }
        }
        // Recarrega as ViewBags em caso de erro de validação ou exceção
        ViewBag.Livros = new SelectList(_livroRep.listarLivros(), "Id", "Titulo", emprestimo.LivroId);
        ViewBag.Usuarios = new SelectList(_usuarioRep.ListarUsuarios(), "Id", "Nome", emprestimo.UsuarioId);
        return View(emprestimo);
    }

    [HttpPost]
    public IActionResult Excluir(int id)
    {
        try
        {
            _emprestimoRep.Excluir(id);
            TempData["MensagemSucesso"] = "Empréstimo excluído com sucesso!";
        }
        catch (Exception ex)
        {
            TempData["MensagemErro"] = $"Erro ao excluir empréstimo: {ex.Message}";
            Debug.WriteLine($"Erro ao excluir empréstimo: {ex.Message}");
        }
        return RedirectToAction("Index");
    }
}