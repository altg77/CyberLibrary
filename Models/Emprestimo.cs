using System;
using System.ComponentModel.DataAnnotations;

namespace CyberLibrary2.Models
{
    public class Emprestimo
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "O livro é obrigatório.")]
        public int LivroId { get; set; }
        public Livro? Livro { get; set; } 

        [Required(ErrorMessage = "O usuário é obrigatório.")]
        public int UsuarioId { get; set; }
        public Usuario? Usuario { get; set; } 
        [Required(ErrorMessage = "A data de empréstimo é obrigatória.")]
        public DateTime DataEmprestimo { get; set; } = DateTime.Now;

        [Required(ErrorMessage = "A data de devolução prevista é obrigatória.")]
        public DateTime DataDevolucaoPrevista { get; set; }

        public DateTime? DataDevolucaoReal { get; set; }

        public bool Devolvido { get; set; } = false;
    }
}