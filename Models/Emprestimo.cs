using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CyberLibrary2.Models
{
    public class Emprestimo
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "O ID do livro é obrigatório.")]
        public int LivroId { get; set; }

        [ForeignKey("LivroId")]
        public Livro Livro { get; set; } // Propriedade de navegação para o Livro


        [Required(ErrorMessage = "O ID do usuário é obrigatório.")]
        public int UsuarioId { get; set; }


        [ForeignKey("UsuarioId")]
        public Usuario Usuario { get; set; } // Propriedade de navegação para o Usuário


        [Required(ErrorMessage = "A data do empréstimo é obrigatória.")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime DataEmprestimo { get; set; } = DateTime.Now;


        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? DataDevolucaoPrevista { get; set; }


        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? DataDevolucaoReal { get; set; }

        public bool Devolvido { get; set; } = false;
    }
}