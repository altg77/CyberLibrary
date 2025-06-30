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

            // Adicionar ImagemUrl (agora um caminho string) como uma claim se ela existir
            if (!string.IsNullOrEmpty(usuario.ImagemUrl))
            {
                claims.Add(new Claim("ProfilePictureUrl", usuario.ImagemUrl));
            }

            var claimsIdentity = new ClaimsIdentity(
                claims, CookieAuthenticationDefaults.AuthenticationScheme);

            var authProperties = new AuthenticationProperties
            {
                IsPersistent = true,
                ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(30)
            };

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties);

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
        return RedirectToAction("Login", "Usuarios");
    }

    [Authorize]
    public IActionResult Perfil()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (string.IsNullOrEmpty(userId))
        {
            return RedirectToAction("Login"); // Or handle unauthorized access
        }

        var usuario = _usuarioRep.BuscarId(int.Parse(userId));
        if (usuario == null)
        {
            return NotFound(); // Or handle user not found
        }
        return View(usuario);
    }

    // Cadastros
    [Authorize]
    public IActionResult Index()
    {
        List<Usuario> usuarios = _usuarioRep.ListarUsuarios();

        List<Usuario> usuariosFiltrados = usuarios
                                           .Where(u => u.Cargo != "Bibliotecario")
                                           .ToList();

        return View(usuariosFiltrados);
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
            return RedirectToAction("Index");
        }
        return View(usuario);
    }

    // Em UsuariosController.cs
    [HttpPost]
    public async Task<ActionResult> Atualizar(Usuario usuario, IFormFile? ImagemArq, string? NovaSenha, string? ConfirmarSenha)
    {
        var existingUsuario = _usuarioRep.BuscarId(usuario.Id);
        if (existingUsuario == null)
        {
            return NotFound();
        }

        // Lidar com o upload da imagem e salvar o caminho
        if (ImagemArq != null && ImagemArq.Length > 0)
        {
            // Defina o caminho onde as imagens serão salvas
            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "profile_pictures");
            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            // Gerar um nome de arquivo único
            var uniqueFileName = Guid.NewGuid().ToString() + "_" + ImagemArq.FileName;
            var filePath = Path.Combine(uploadsFolder, uniqueFileName);

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await ImagemArq.CopyToAsync(fileStream);
            }

            // Armazenar o caminho relativo no banco de dados
            existingUsuario.ImagemUrl = $"/images/profile_pictures/{uniqueFileName}";
        }
        // Se nenhuma nova imagem for carregada, mantenha a ImagemUrl existente
        else if (!string.IsNullOrEmpty(existingUsuario.ImagemUrl))
        {
            existingUsuario.ImagemUrl = existingUsuario.ImagemUrl;
        }
        else
        {
            existingUsuario.ImagemUrl = null; // Ou uma URL de imagem padrão
        }


        // Atualizar outras propriedades do usuário
        existingUsuario.Nome = usuario.Nome;
        existingUsuario.Email = usuario.Email;
        existingUsuario.Telefone = usuario.Telefone;

        // Lidar com a atualização da senha
        if (!string.IsNullOrEmpty(NovaSenha))
        {
            if (NovaSenha != ConfirmarSenha)
            {
                ModelState.AddModelError("NovaSenha", "A nova senha e a confirmação de senha não coincidem.");
                return View("Perfil", existingUsuario);
            }
            // Em uma aplicação real, você deve fazer hash da nova senha aqui
            existingUsuario.Senha = NovaSenha;
        }

        if (ModelState.IsValid)
        {
            try
            {
                _usuarioRep.Atualizar(existingUsuario);

                // Reautenticar o usuário com claims atualizadas (excluindo os dados grandes da imagem)
                var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, existingUsuario.Id.ToString()),
                new Claim(ClaimTypes.Name, existingUsuario.Nome),
                new Claim(ClaimTypes.Email, existingUsuario.Email),
                new Claim(ClaimTypes.Role, existingUsuario.Cargo)
            };

                // Adicionar a URL da imagem como uma claim se ela existir
                if (!string.IsNullOrEmpty(existingUsuario.ImagemUrl))
                {
                    claims.Add(new Claim("ProfilePictureUrl", existingUsuario.ImagemUrl));
                }

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var authProperties = new AuthenticationProperties
                {
                    IsPersistent = true,
                    ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(30)
                };

                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity),
                    authProperties);

                return RedirectToAction("Perfil");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"Erro ao atualizar perfil: {ex.Message}");
                Debug.WriteLine($"Erro ao atualizar perfil: {ex.Message}");
            }
        }
        return View("Perfil", existingUsuario);
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