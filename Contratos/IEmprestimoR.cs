using CyberLibrary2.Models;

namespace CyberLibrary2.Contratos
{
    public interface IEmprestimoR
    {
        Emprestimo BuscarId(int id);
        Emprestimo Adicionar(Emprestimo emprestimo);
        List<Emprestimo> ListarEmprestimos();
        List<Emprestimo> ListarEmprestimosPorUsuario(int usuarioId);
        List<Emprestimo> ListarEmprestimosPorLivro(int livroId);
        Emprestimo Atualizar(Emprestimo emprestimo);
        bool Excluir(int id);
    }
}