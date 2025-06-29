using CyberLibrary2.Models;

namespace CyberLibrary2.Contratos
{
    public interface ICategoriaR
    {
        Categoria BuscarId(int id);
        Categoria Adicionar(Categoria categoria);
        List<Categoria> ListarCategorias();
        Categoria Atualizar(Categoria categoria);
        bool Excluir(int id);
    }
}