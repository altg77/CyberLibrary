using CyberLibrary2.Models;
using CyberLibrary2.repository;

namespace CyberLibrary2.Contratos
{
    public interface ISetorR
    {
        Setor BuscarId(int id);
        Setor Adicionar(Setor setor);
        List<Setor> ListarSetores();
        Setor Atualizar(Setor setor);
        bool Excluir(int id);  
    }
}