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

        public Emprestimo Adicionar(Emprestimo emprestimo)
        {
            var livro = _bancoContext.Livros.FirstOrDefault(l => l.Id == emprestimo.LivroId);

            if (livro != null && livro.QuantidadeDisponivel > 0)
            {
                livro.QuantidadeDisponivel--;
                if (livro.QuantidadeDisponivel == 0)
                {
                    livro.Disponivel = false;
                }
                _bancoContext.Livros.Update(livro);
            }

            else if (livro != null && livro.QuantidadeDisponivel == 0)
            {
                throw new Exception("Livro indisponível para empréstimo.");
            }
            else
            {
                throw new Exception("Livro não encontrado.");
            }

            _bancoContext.Emprestimos.Add(emprestimo);
            _bancoContext.SaveChanges();
            return emprestimo;
        }

        public List<Emprestimo> ListarEmprestimos()
        {
            // Inclui as propriedades de navegação para ter acesso aos dados do Livro e do Usuário
            return _bancoContext.Emprestimos
                                .Include(e => e.Livro)
                                .Include(e => e.Usuario)
                                .ToList();
        }

        public List<Emprestimo> ListarEmprestimosPorUsuario(int usuarioId)
        {
            return _bancoContext.Emprestimos
                                .Include(e => e.Livro)
                                .Include(e => e.Usuario)
                                .Where(e => e.UsuarioId == usuarioId)
                                .ToList();
        }

        public List<Emprestimo> ListarEmprestimosPorLivro(int livroId)
        {
            return _bancoContext.Emprestimos
                                .Include(e => e.Livro)
                                .Include(e => e.Usuario)
                                .Where(e => e.LivroId == livroId)
                                .ToList();
        }

        public Emprestimo BuscarId(int id)
        {
            return _bancoContext.Emprestimos
                                .Include(e => e.Livro)
                                .Include(e => e.Usuario)
                                .FirstOrDefault(x => x.Id == id);
        }

        public Emprestimo Atualizar(Emprestimo emprestimo)
        {
            Emprestimo emprestimoDB = BuscarId(emprestimo.Id);

            if (emprestimoDB == null)
            {
                throw new Exception("Empréstimo não encontrado para atualização.");
            }

            // Lógica para devolver o livro (incrementar quantidade disponível)
            if (emprestimo.Devolvido && !emprestimoDB.Devolvido) // Se foi marcado como devolvido agora
            {
                var livro = _bancoContext.Livros.FirstOrDefault(l => l.Id == emprestimo.LivroId);
                if (livro != null)
                {
                    livro.QuantidadeDisponivel++;
                    livro.Disponivel = true; // Marcar como disponível novamente
                    _bancoContext.Livros.Update(livro);
                }
            }
            // Se o livro foi devolvido e depois "desdevolvido" (não é comum, mas para consistência)
            else if (!emprestimo.Devolvido && emprestimoDB.Devolvido)
            {
                var livro = _bancoContext.Livros.FirstOrDefault(l => l.Id == emprestimo.LivroId);
                if (livro != null && livro.QuantidadeDisponivel > 0)
                {
                    livro.QuantidadeDisponivel--;
                    if (livro.QuantidadeDisponivel == 0)
                    {
                        livro.Disponivel = false;
                    }
                    _bancoContext.Livros.Update(livro);
                }
                else if (livro != null && livro.QuantidadeDisponivel == 0)
                {
                    throw new Exception("Não é possível emprestar novamente, livro indisponível.");
                }
                else
                {
                    throw new Exception("Livro não encontrado.");
                }
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

        public bool Excluir(int id)
        {
            Emprestimo emprestimoDB = BuscarId(id);

            if (emprestimoDB == null)
            {
                throw new Exception("Empréstimo não encontrado para exclusão.");
            }

            // Se o empréstimo não foi devolvido, incrementa a quantidade disponível antes de excluir
            if (!emprestimoDB.Devolvido)
            {
                var livro = _bancoContext.Livros.FirstOrDefault(l => l.Id == emprestimoDB.LivroId);
                if (livro != null)
                {
                    livro.QuantidadeDisponivel++;
                    livro.Disponivel = true; // Marcar como disponível novamente
                    _bancoContext.Livros.Update(livro);
                }
            }

            _bancoContext.Emprestimos.Remove(emprestimoDB);
            _bancoContext.SaveChanges();
            return true;
        }
    }
}