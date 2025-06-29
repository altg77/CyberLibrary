using System.ComponentModel.DataAnnotations;

namespace CyberLibrary2.Models
{
    public class Categoria
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "O nome da categoria é obrigatório.")]
        [MaxLength(100, ErrorMessage = "O nome da categoria não pode exceder 100 caracteres.")]
        public string Nome { get; set; }


        [StringLength(500, ErrorMessage = "A descrição não pode ter mais de 500 caracteres.")]
        public string Descricao { get; set; }
    }
}