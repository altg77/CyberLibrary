using Microsoft.EntityFrameworkCore;
using CyberLibrary2.Models;

namespace CyberLibrary2.Data
{
    public class BancoContext : DbContext
    {
        public BancoContext(DbContextOptions<BancoContext> options) : base(options)
        {
        }
        public DbSet<Livro> Livros { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Categoria> Categorias { get; set; }
        public DbSet<Turma> Turmas { get; set; }
        public DbSet<Emprestimo> Emprestimos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Emprestimo>()
                .HasOne(e => e.Livro)
                .WithMany() // ou WithMany(l => l.Emprestimos) se Livro tiver uma coleção de Emprestimos
                .HasForeignKey(e => e.LivroId)
                .OnDelete(DeleteBehavior.Restrict); // Evita exclusão em cascata de livros

            modelBuilder.Entity<Emprestimo>()
                .HasOne(e => e.Usuario)
                .WithMany() // ou WithMany(u => u.Emprestimos) se Usuario tiver uma coleção de Emprestimos
                .HasForeignKey(e => e.UsuarioId)
                .OnDelete(DeleteBehavior.Restrict); // Evita exclusão em cascata de usuários
        }

    }
}

