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

        public Usuario Atualizar(Usuario usuario)
        {
            Usuario usuarioDB = BuscarId(usuario.Id);

            if (usuarioDB == null)
            {
                throw new Exception("Usuário não foi encontrado para atualização.");
            }

            usuarioDB.Nome = usuario.Nome;
            usuarioDB.Email = usuario.Email;
            usuarioDB.Cargo = usuario.Cargo;
            usuarioDB.Login = usuario.Login;

            if (!string.IsNullOrEmpty(usuario.Senha))
            {
                usuarioDB.Senha = usuario.Senha;
            }

            if (usuario.ImagemUrl != null && usuario.ImagemUrl.Length > 0)
            {
                usuarioDB.ImagemUrl = usuario.ImagemUrl;
            }

            usuarioDB.ImagemUrl = usuario.ImagemUrl;
            usuarioDB.Telefone = usuario.Telefone;
            usuarioDB.TurmaId = usuario.TurmaId;

            banco_context.Usuarios.Update(usuarioDB);
            banco_context.SaveChanges();
            return usuarioDB;
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
            // In a real application, you would query your database here.
            // And use password hashing (e.g., BCrypt.Net.BCrypt.Verify(senha, user.SenhaHash)).
            return banco_context.Usuarios.FirstOrDefault(u => u.Login == login && u.Senha == senha); // This is for demo only!
        }
    }
}