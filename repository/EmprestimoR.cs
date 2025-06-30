using CyberLibrary2.Contratos;
using CyberLibrary2.Data;
using CyberLibrary2.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CyberLibrary2.repository
{
    public class EmprestimoR : IEmprestimoR
    {
        private readonly BancoContext _bancoContext;

        public EmprestimoR(BancoContext bancoContext)
        {
            _bancoContext = bancoContext;
        }

        public Emprestimo AdicionarEmprestimo(Emprestimo emprestimo)
        {
            emprestimo.Livro = _bancoContext.Livros.FirstOrDefault(l => l.Id == emprestimo.LivroId);
            emprestimo.Usuario = _bancoContext.Usuarios.FirstOrDefault(u => u.Id == emprestimo.UsuarioId);

            if (emprestimo.Livro == null)
            {
                throw new Exception("Livro não encontrado.");
            }

            if (emprestimo.Usuario == null)
            {
                throw new Exception("Usuário não encontrado.");
            }

            if (emprestimo.Livro.QuantidadeDisponivel <= 0)
            {
                throw new Exception("Livro indisponível para empréstimo.");
            }

            emprestimo.Livro.QuantidadeDisponivel--;
            emprestimo.Livro.Disponivel = emprestimo.Livro.QuantidadeDisponivel > 0;

            _bancoContext.Emprestimos.Add(emprestimo);
            _bancoContext.SaveChanges();
            return emprestimo;
        }

        public List<Emprestimo> ListarEmprestimos()
        {
            // Include related entities (Livro and Usuario) for richer data
            return _bancoContext.Emprestimos
                                .Include(e => e.Livro)
                                .Include(e => e.Usuario)
                                .ToList();
        }

        public Emprestimo BuscarEmprestimoPorId(int id)
        {
            return _bancoContext.Emprestimos
                                .Include(e => e.Livro)
                                .Include(e => e.Usuario)
                                .FirstOrDefault(e => e.Id == id);
        }

        public Emprestimo AtualizarEmprestimo(Emprestimo emprestimo)
        {
            Emprestimo emprestimoDB = BuscarEmprestimoPorId(emprestimo.Id);

            if (emprestimoDB == null)
            {
                throw new Exception("Empréstimo não encontrado.");
            }

            emprestimoDB.LivroId = emprestimo.LivroId;
            emprestimoDB.UsuarioId = emprestimo.UsuarioId;
            emprestimoDB.DataEmprestimo = emprestimo.DataEmprestimo;
            emprestimoDB.DataDevolucaoPrevista = emprestimo.DataDevolucaoPrevista;
            emprestimoDB.DataDevolucaoReal = emprestimo.DataDevolucaoReal;
            emprestimoDB.Devolvido = emprestimo.Devolvido;

            _bancoContext.Emprestimos.Update(emprestimoDB);
            _bancoContext.SaveChanges();
            return emprestimoDB;
        }

        public bool ExcluirEmprestimo(int id)
        {
            Emprestimo emprestimoDB = BuscarEmprestimoPorId(id);

            if (emprestimoDB == null)
            {
                throw new Exception("Empréstimo não encontrado para exclusão.");
            }

            _bancoContext.Emprestimos.Remove(emprestimoDB);
            _bancoContext.SaveChanges();
            return true;
        }

        public bool RegistrarDevolucao(int emprestimoId)
        {
            Emprestimo emprestimoDB = BuscarEmprestimoPorId(emprestimoId);

            if (emprestimoDB == null)
            {
                throw new Exception("Empréstimo não encontrado.");
            }

            if (emprestimoDB.Devolvido)
            {
                throw new Exception("Este livro já foi devolvido.");
            }

            emprestimoDB.DataDevolucaoReal = DateTime.Now;
            emprestimoDB.Devolvido = true;

            // Increase available quantity
            Livro livro = _bancoContext.Livros.FirstOrDefault(l => l.Id == emprestimoDB.LivroId);
            if (livro != null)
            {
                livro.QuantidadeDisponivel++;
                livro.Disponivel = livro.QuantidadeDisponivel > 0;
            }
            else
            {
                throw new Exception("Livro associado ao empréstimo não encontrado.");
            }

            _bancoContext.Emprestimos.Update(emprestimoDB);
            _bancoContext.Livros.Update(livro); 
            _bancoContext.SaveChanges();
            return true;
        }

        public List<Emprestimo> ListarEmprestimosPorUsuario(int usuarioId)
        {
            return _bancoContext.Emprestimos
                                .Include(e => e.Livro)
                                .Include(e => e.Usuario)
                                .Where(e => e.UsuarioId == usuarioId)
                                .ToList();
        }

        public List<Emprestimo> ListarEmprestimosAtivos()
        {
            return _bancoContext.Emprestimos
                                .Include(e => e.Livro)
                                .Include(e => e.Usuario)
                                .Where(e => !e.Devolvido)
                                .ToList();
        }
    }
}