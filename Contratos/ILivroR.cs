using CyberLibrary2.Models;

namespace CyberLibrary2.Contratos
{
    public interface ILivroR
    {
        Livro buscarId(int id);
        Livro adicionar(Livro livro);
        List<Livro> listarLivros();
        Livro Atualizar(Livro livro);
        bool excluir(int id);
    }

}
