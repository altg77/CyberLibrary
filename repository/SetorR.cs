using CyberLibrary2.Contratos;
using CyberLibrary2.Data;
using CyberLibrary2.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CyberLibrary2.repository
{
    public class SetorR : ISetorR
    {
        private readonly BancoContext _bancoContext;

        public SetorR(BancoContext bancoContext)
        {
            _bancoContext = bancoContext;
        }

        public Setor Adicionar(Setor setor)
        {
            _bancoContext.Setores.Add(setor);
            _bancoContext.SaveChanges();
            return setor;
        }

        public List<Setor> ListarSetores()
        {
            return _bancoContext.Setores.ToList();
        }

        public Setor BuscarId(int id)
        {
            return _bancoContext.Setores.FirstOrDefault(x => x.Id == id);
        }

        public Setor Atualizar(Setor setor)
        {
            Setor setorDB = BuscarId(setor.Id);

            if (setorDB == null)
            {
                throw new Exception("Setor não encontrada para atualização.");
            }

            setorDB.Nome = setor.Nome;
            setorDB.Descricao = setor.Descricao;

            _bancoContext.Setores.Update(setorDB);
            _bancoContext.SaveChanges();
            return setorDB;
        }

        public bool Excluir(int id)
        {
            Setor turmaDB = BuscarId(id);

            if (turmaDB == null)
            {
                throw new Exception("Turma não encontrada para exclusão.");
            }

            _bancoContext.Setores.Remove(turmaDB);
            _bancoContext.SaveChanges();
            return true;
        }
    }
}