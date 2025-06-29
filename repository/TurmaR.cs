using CyberLibrary2.Contratos;
using CyberLibrary2.Data;
using CyberLibrary2.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CyberLibrary2.repository
{
    public class TurmaR : ITurmaR
    {
        private readonly BancoContext _bancoContext;

        public TurmaR(BancoContext bancoContext)
        {
            _bancoContext = bancoContext;
        }

        public Turma Adicionar(Turma turma)
        {
            _bancoContext.Turmas.Add(turma);
            _bancoContext.SaveChanges();
            return turma;
        }

        public List<Turma> ListarTurmas()
        {
            return _bancoContext.Turmas.ToList();
        }

        public Turma BuscarId(int id)
        {
            return _bancoContext.Turmas.FirstOrDefault(x => x.Id == id);
        }

        public Turma Atualizar(Turma turma)
        {
            Turma turmaDB = BuscarId(turma.Id);

            if (turmaDB == null)
            {
                throw new Exception("Turma não encontrada para atualização.");
            }

            turmaDB.Nome = turma.Nome;
            turmaDB.Descricao = turma.Descricao;
            turmaDB.AnoPeriodo = turma.AnoPeriodo;

            _bancoContext.Turmas.Update(turmaDB);
            _bancoContext.SaveChanges();
            return turmaDB;
        }

        public bool Excluir(int id)
        {
            Turma turmaDB = BuscarId(id);

            if (turmaDB == null)
            {
                throw new Exception("Turma não encontrada para exclusão.");
            }

            _bancoContext.Turmas.Remove(turmaDB);
            _bancoContext.SaveChanges();
            return true;
        }
    }
}