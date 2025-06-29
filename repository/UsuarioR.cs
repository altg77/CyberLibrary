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


            if (banco_context.Usuarios.Any(u => u.Login == usuario.Login && u.Id != usuario.Id))
            {
                throw new Exception("O login informado já está em uso por outro usuário.");
            }

            // Verifique se o novo email já existe para outro usuário
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

            // Apenas atualize a senha se ela for fornecida (não vazia/nula)
            if (!string.IsNullOrEmpty(usuario.Senha))
            {
                usuarioDB.Senha = usuario.Senha;
            }

            // A imagem agora será tratada pela lógica do controlador
            usuarioDB.ImagemUrl = usuario.ImagemUrl;

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
            return banco_context.Usuarios.FirstOrDefault(u => u.Login == login && u.Senha == senha);
        }
    }
}