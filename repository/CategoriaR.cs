using CyberLibrary2.Contratos;
using CyberLibrary2.Data;
using CyberLibrary2.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CyberLibrary2.repository
{
    public class CategoriaR : ICategoriaR
    {
        private readonly BancoContext _bancoContext;

        public CategoriaR(BancoContext bancoContext)
        {
            _bancoContext = bancoContext;
        }

        public Categoria Adicionar(Categoria categoria)
        {
            _bancoContext.Categorias.Add(categoria);
            _bancoContext.SaveChanges();
            return categoria;
        }

        public List<Categoria> ListarCategorias()
        {
            return _bancoContext.Categorias.ToList();
        }

        public Categoria BuscarId(int id)
        {
            return _bancoContext.Categorias.FirstOrDefault(x => x.Id == id);
        }

        public Categoria Atualizar(Categoria categoria)
        {
            Categoria categoriaDB = BuscarId(categoria.Id);

            if (categoriaDB == null)
            {
                throw new Exception("Categoria não encontrada para atualização.");
            }

            categoriaDB.Nome = categoria.Nome;
            categoriaDB.Descricao = categoria.Descricao;

            _bancoContext.Categorias.Update(categoriaDB);
            _bancoContext.SaveChanges();
            return categoriaDB;
        }

        public bool Excluir(int id)
        {
            Categoria categoriaDB = BuscarId(id);

            if (categoriaDB == null)
            {
                throw new Exception("Categoria não encontrada para exclusão.");
            }

            _bancoContext.Categorias.Remove(categoriaDB);
            _bancoContext.SaveChanges();
            return true;
        }
    }
}