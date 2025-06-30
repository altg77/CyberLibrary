using CyberLibrary2.Models;

namespace CyberLibrary2.Contratos
{
    public interface IEmprestimoR
    {
         Emprestimo AdicionarEmprestimo(Emprestimo emprestimo);
        List<Emprestimo> ListarEmprestimos();
        Emprestimo BuscarEmprestimoPorId(int id);
        Emprestimo AtualizarEmprestimo(Emprestimo emprestimo);
        bool ExcluirEmprestimo(int id);
        bool RegistrarDevolucao(int emprestimoId);
        List<Emprestimo> ListarEmprestimosPorUsuario(int usuarioId);
        List<Emprestimo> ListarEmprestimosAtivos();
    }
}