using CyberLibrary2.Models;

namespace CyberLibrary2.Contratos
{
    public interface IUsuarioR
    {
        Usuario BuscarId(int id);
        Usuario Adicionar(Usuario usuario);
        List<Usuario> ListarUsuarios();
        Usuario Atualizar(Usuario usuario);
        bool Excluir(int id);

        Usuario BuscarPorLoginESenha(string login, string senha);
    }
}