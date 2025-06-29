using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using CyberLibrary2.Models;
using CyberLibrary2.Contratos;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;

namespace CyberLibrary2.Controllers;

public class UsuariosController : Controller
{
    private readonly IUsuarioR _usuarioRep;

    public UsuariosController(IUsuarioR usuarioRep)
    {
        _usuarioRep = usuarioRep;
    }


    //Authentição de Usuario
    [AllowAnonymous]
    public IActionResult Login()
    {
        if (User.Identity.IsAuthenticated)
        {
            if (User.IsInRole("Bibliotecario")) // Assuming "Bibliotecario" is the role name for librarians
            {
                return RedirectToAction("Index", "Bibliotecario");
            }
            else if (User.IsInRole("Aluno")) // Assuming "Aluno" is the role name for students
            {
                return RedirectToAction("Index", "Aluno"); // Redirect to Aluno's dashboard
            }
        }
        return View();
    }

    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> Login(string login, string senha)
    {
        var usuario = _usuarioRep.BuscarPorLoginESenha(login, senha);

        if (usuario != null)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
                new Claim(ClaimTypes.Name, usuario.Nome),
                new Claim(ClaimTypes.Email, usuario.Email),
                new Claim(ClaimTypes.Role, usuario.Cargo) 
            };

            var claimsIdentity = new ClaimsIdentity(
                claims, CookieAuthenticationDefaults.AuthenticationScheme);

            var authProperties = new AuthenticationProperties
            {
                IsPersistent = true, // Remember me functionality
                ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(30) // Cookie expires in 30 minutes
            };

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties);

            TempData["MensagemSucesso"] = $"Bem-vindo, {usuario.Nome}!";

            if (usuario.Cargo == "Bibliotecario") 
            {
                return RedirectToAction("Index", "Bibliotecario");
            }
            else if (usuario.Cargo == "Aluno") 
            {
                return RedirectToAction("Index", "Aluno");
            }
            else
            {
                return RedirectToAction("Login", "Usuarios"); 
            }
        }

        ModelState.AddModelError(string.Empty, "Login ou senha inválidos.");
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        TempData["MensagemSucesso"] = "Você foi desconectado com sucesso.";
        return RedirectToAction("Login", "Usuarios");
    }

    [Authorize]
    public IActionResult Perfil(int id)
    {
        var usuario = _usuarioRep.BuscarId(id);
        return View(usuario);
    }

    // Cadastros
    [Authorize]
    public IActionResult Index()
    {
        List<Usuario> usuarios = _usuarioRep.ListarUsuarios();
        return View(usuarios);
    }

    public IActionResult Adicionar()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Adicionar(Usuario usuario)
    {
        if (ModelState.IsValid)
        {
            _usuarioRep.Adicionar(usuario);
            TempData["MensagemSucesso"] = "Usuário adicionado com sucesso!";
            return RedirectToAction("Index");
        }
        return View(usuario);
    }

    [HttpPost]
    public async Task<ActionResult> Atualizar(Usuario usuario, IFormFile? ImagemArq)
    {
        if (ModelState.IsValid)
        {
            if (ImagemArq != null && ImagemArq.Length > 0)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await ImagemArq.CopyToAsync(memoryStream);
                    usuario.ImagemUrl = memoryStream.ToArray();
                }
            }
        }

        _usuarioRep.Atualizar(usuario);
        return RedirectToAction("Perfil");
    }

    [HttpPost]
    public IActionResult Excluir(int id)
    {
        try
        {
            _usuarioRep.Excluir(id);
            TempData["MensagemSucesso"] = "Usuário excluído com sucesso!";
        }
        catch (Exception ex)
        {
            TempData["MensagemErro"] = $"Erro ao excluir usuário: {ex.Message}";
            Debug.WriteLine($"Erro ao excluir usuário: {ex.Message}");
        }

        return RedirectToAction("Index");
    }

}
