using CyberLibrary2.Contratos;
using CyberLibrary2.Data;
using CyberLibrary2.Models;

namespace CyberLibrary2.repository
{
    public class UsuarioR : IUsuarioR
    {
        private readonly BancoContext banco_context;

        public UsuarioR(BancoContext bancoContext)
        {
            banco_context = bancoContext;
        }

        public Usuario Adicionar(Usuario usuario)
        {
            banco_context.Usuarios.Add(usuario);
            banco_context.SaveChanges();
            return usuario;
        }

        public List<Usuario> ListarUsuarios()
        {
            return banco_context.Usuarios.ToList();
        }

        public Usuario BuscarId(int id)
        {
            return banco_context.Usuarios.FirstOrDefault(x => x.Id == id);
        }

        // Dentro de UsuarioR.cs

        public Usuario Atualizar(Usuario usuario)
        {
            Usuario usuarioDB = banco_context.Usuarios.FirstOrDefault(x => x.Id == usuario.Id);

            if (usuarioDB == null)
            {
                throw new Exception("Usuário não encontrado para atualização.");
            }

            // Seus checks de unicidade (login e email) já estão aqui, o que é bom!
            // Eles lançam exceções próprias, então o problema deve ser após eles.
            if (banco_context.Usuarios.Any(u => u.Login == usuario.Login && u.Id != usuario.Id))
            {
                throw new Exception("O login informado já está em uso por outro usuário.");
            }

            if (banco_context.Usuarios.Any(u => u.Email == usuario.Email && u.Id != usuario.Id))
            {
                throw new Exception("O email informado já está em uso por outro usuário.");
            }

            usuarioDB.Nome = usuario.Nome;
            usuarioDB.Email = usuario.Email;
            usuarioDB.Cargo = usuario.Cargo;
            usuarioDB.Login = usuario.Login;
            usuarioDB.Telefone = usuario.Telefone;
            usuarioDB.TurmaId = usuario.TurmaId;
            usuarioDB.ImagemUrl = usuario.ImagemUrl; // Mantenha esta linha

            if (!string.IsNullOrEmpty(usuario.Senha))
            {
                usuarioDB.Senha = usuario.Senha;
            }

            try
            {
                banco_context.Usuarios.Update(usuarioDB);
                banco_context.SaveChanges(); // A exceção provavelmente acontece aqui
                return usuarioDB;
            }
            catch (Microsoft.EntityFrameworkCore.DbUpdateException ex)
            {
                // Esta exceção é do Entity Framework Core
                // A InnerException conterá a causa raiz do problema no banco de dados
                System.Diagnostics.Debug.WriteLine($"Erro ao salvar no banco de dados: {ex.Message}");
                if (ex.InnerException != null)
                {
                    System.Diagnostics.Debug.WriteLine($"Inner Exception: {ex.InnerException.Message}");
                    // Você pode até inspecionar ex.InnerException.InnerException se houver mais níveis
                }
                throw; // Relança a exceção para que o controller ainda a capture
            }
            catch (Exception ex)
            {
                // Outros tipos de exceção que podem ocorrer
                System.Diagnostics.Debug.WriteLine($"Erro geral no repositório: {ex.Message}");
                throw;
            }
        }

        public bool Excluir(int id)
        {
            Usuario usuarioDB = BuscarId(id);

            if (usuarioDB == null)
            {
                throw new Exception("Usuário não foi encontrado para exclusão.");
            }

            banco_context.Usuarios.Remove(usuarioDB);
            banco_context.SaveChanges();
            return true;
        }

        public Usuario BuscarPorLoginESenha(string login, string senha)
        {
            return banco_context.Usuarios.FirstOrDefault(u => u.Login == login && u.Senha == senha);
        }
    }
}