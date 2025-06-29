using CyberLibrary2.Models;

namespace CyberLibrary2.Contratos
{
    public interface ITurmaR
    {
        Turma BuscarId(int id);
        Turma Adicionar(Turma turma);
        List<Turma> ListarTurmas();
        Turma Atualizar(Turma turma);
        bool Excluir(int id);  
    }
}