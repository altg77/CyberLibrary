using CyberLibrary2.Contratos;
using CyberLibrary2.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering; // For SelectList

namespace CyberLibrary2.Controllers
{
    public class EmprestimosController : Controller
    {
        private readonly IEmprestimoR _emprestimoRepository;
        private readonly ILivroR _livroRepository; 
        private readonly IUsuarioR _usuarioRepository; 

        public EmprestimosController(IEmprestimoR emprestimoRepository, ILivroR livroRepository, IUsuarioR usuarioRepository)
        {
            _emprestimoRepository = emprestimoRepository;
            _livroRepository = livroRepository;
            _usuarioRepository = usuarioRepository;
        }

        public IActionResult Index()
        {
            List<Emprestimo> emprestimos = _emprestimoRepository.ListarEmprestimos();
            return View(emprestimos);
        }

        public IActionResult Adicionar()
        {
            var usuariosDisponiveis = _usuarioRepository.ListarUsuarios()
                                                    .Where(u => u.Cargo != "bibliotecario")
                                                    .ToList();

            ViewBag.Livros = new SelectList(_livroRepository.listarLivros().Where(l => l.QuantidadeDisponivel > 0), "Id", "Titulo");
            ViewBag.Usuarios = new SelectList(usuariosDisponiveis, "Id", "Nome");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Adicionar(Emprestimo emprestimo)
        {
            ModelState.Remove("Livro");
            ModelState.Remove("Usuario");
            ModelState.Remove("Devolvido"); 

            if (ModelState.IsValid)
            {
                try
                {
                    _emprestimoRepository.AdicionarEmprestimo(emprestimo);
                    TempData["MensagemSucesso"] = "Empréstimo registrado com sucesso!";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    TempData["MensagemErro"] = $"Erro ao registrar empréstimo: {ex.Message}";
                }
            }

            var usuariosDisponiveis = _usuarioRepository.ListarUsuarios()
                                                    .Where(u => u.Cargo != "bibliotecario")
                                                    .ToList();

            ViewBag.Livros = new SelectList(_livroRepository.listarLivros().Where(l => l.QuantidadeDisponivel > 0), "Id", "Titulo", emprestimo.LivroId);
            ViewBag.Usuarios = new SelectList(usuariosDisponiveis, "Id", "Nome", emprestimo.UsuarioId);
            return View(emprestimo);
        }

        public IActionResult Editar(int id)
        {
            Emprestimo emprestimo = _emprestimoRepository.BuscarEmprestimoPorId(id);
            if (emprestimo == null)
            {
                return NotFound();
            }

            var availableBooks = _livroRepository.listarLivros()
                                                    .Where(l => l.QuantidadeDisponivel > 0 || l.Id == emprestimo.LivroId)
                                                    .ToList();
            
            // Filter out users with "bibliotecario" role
            var usuariosDisponiveis = _usuarioRepository.ListarUsuarios()
                                                    .Where(u => u.Cargo != "bibliotecario")
                                                    .ToList();

            ViewBag.Livros = new SelectList(availableBooks, "Id", "Titulo", emprestimo.LivroId);
            ViewBag.Usuarios = new SelectList(usuariosDisponiveis, "Id", "Nome", emprestimo.UsuarioId);
            return View(emprestimo);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Atualizar(Emprestimo emprestimo)
        {
            ModelState.Remove("Livro");
            ModelState.Remove("Usuario");

            if (ModelState.IsValid)
            {
                try
                {
                    _emprestimoRepository.AtualizarEmprestimo(emprestimo);
                    TempData["MensagemSucesso"] = "Empréstimo atualizado com sucesso!";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    TempData["MensagemErro"] = $"Erro ao atualizar empréstimo: {ex.Message}";
                }
            }

            var availableBooks = _livroRepository.listarLivros()
                                                    .Where(l => l.QuantidadeDisponivel > 0 || l.Id == emprestimo.LivroId)
                                                    .ToList();
            
            // Filter out users with "bibliotecario" role for redisplay as well
            var usuariosDisponiveis = _usuarioRepository.ListarUsuarios()
                                                    .Where(u => u.Cargo != "bibliotecario")
                                                    .ToList();

            ViewBag.Livros = new SelectList(availableBooks, "Id", "Titulo", emprestimo.LivroId);
            ViewBag.Usuarios = new SelectList(usuariosDisponiveis, "Id", "Nome", emprestimo.UsuarioId);
            return View("Editar", emprestimo);
        }

        public IActionResult ConfirmarRemocao(int id)
        {
            Emprestimo emprestimo = _emprestimoRepository.BuscarEmprestimoPorId(id);
            if (emprestimo == null)
            {
                return NotFound();
            }
            return View(emprestimo);
        }

        [HttpPost, ActionName("Remover")]
        [ValidateAntiForgeryToken]
        public IActionResult RemoverConfirmado(int id)
        {
            try
            {
                _emprestimoRepository.ExcluirEmprestimo(id);
                TempData["MensagemSucesso"] = "Empréstimo removido com sucesso!";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["MensagemErro"] = $"Erro ao remover empréstimo: {ex.Message}";
                return RedirectToAction(nameof(ConfirmarRemocao), new { id = id });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult RegistrarDevolucao(int id)
        {
            try
            {
                bool success = _emprestimoRepository.RegistrarDevolucao(id);
                if (success)
                {
                    TempData["MensagemSucesso"] = "Devolução registrada com sucesso!";
                }
                else
                {
                    TempData["MensagemErro"] = "Não foi possível registrar a devolução.";
                }
            }
            catch (Exception ex)
            {
                TempData["MensagemErro"] = $"Erro ao registrar devolução: {ex.Message}";
            }
            return RedirectToAction(nameof(Index));
        }
    }
}