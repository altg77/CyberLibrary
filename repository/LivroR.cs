using CyberLibrary2.Contratos;
using CyberLibrary2.Data;
using CyberLibrary2.Models;

namespace CyberLibrary2.repository
{
    public class LivroR : ILivroR
    {
        private readonly BancoContext banco_context;

        public LivroR(BancoContext bancoContext)
        {
            banco_context = bancoContext;
        }

        public Livro adicionar(Livro livro)
        {
            banco_context.Livros.Add(livro);
            banco_context.SaveChanges();
            return livro;
        }

        public List<Livro> listarLivros()
        {
            return banco_context.Livros.ToList();
        }

        public Livro buscarId(int id)
        {
            return banco_context.Livros.FirstOrDefault(x => x.Id == id);
        }

        public Livro Atualizar(Livro livro)
        {
            Livro livroDB = buscarId(livro.Id);

            if (livroDB == null) throw new Exception("Contato não foi encontrado");

            //Alterations
            livroDB.Titulo = livro.Titulo;
            livroDB.Autor = livro.Autor;
            livroDB.Editora = livro.Editora;
            livroDB.Quantidade = livro.Quantidade;
            livroDB.Estante = livro.Estante;
            livroDB.Prateleira = livro.Prateleira;
            livroDB.Sinopse = livro.Sinopse;

            if (livro.CapaImagem != null)
            {
                livroDB.CapaImagem = livro.CapaImagem;
            }

            banco_context.Livros.Update(livroDB);
            banco_context.SaveChanges();
            return livroDB;
        }

        public bool excluir(int id)
        {
            Livro livroDB = buscarId(id);

            if (livroDB == null) throw new Exception("Contato não foi encontrado para exclusão");

            banco_context.Remove(livroDB);
            banco_context.SaveChanges();
            return true;

        }
    }
}


