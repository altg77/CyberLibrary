using System.ComponentModel.DataAnnotations;

namespace CyberLibrary2.Models
{
    public class Turma
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "O nome da turma é obrigatório.")]
        [StringLength(100, ErrorMessage = "O nome da turma não pode ter mais de 100 caracteres.")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "A descrição da turma é obrigatória.")]
        [StringLength(500, ErrorMessage = "A descrição não pode ter mais de 500 caracteres.")]
        public string Descricao { get; set; }

        [Required(ErrorMessage = "O ano/período da turma é obrigatório.")]
        [Range(2000, 2100, ErrorMessage = "O ano/período deve ser um valor válido (ex: 2024).")]
        public int AnoPeriodo { get; set; } 
    }
}