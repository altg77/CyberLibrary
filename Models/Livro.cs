using System.ComponentModel.DataAnnotations;

namespace CyberLibrary2.Models
{
    public class Livro
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "O título é obrigatório.")]
        public string Titulo { get; set; }

        [Required(ErrorMessage = "O autor é obrigatório.")]
        public string Autor { get; set; }

        public string? Editora { get; set; }
        public string? Setor { get; set; }
        public bool Disponivel { get; set; } = true;

        public string? Estante { get; set; }
        public string? Prateleira { get; set; }
        public string? Sinopse { get; set; }
        public string? Categoria { get; set; }
        public string? Formato { get; set; }
        public byte[]? CapaImagem { get; set; }

        //Quantidade
        [Required(ErrorMessage = "A quantidade total é obrigatória.")]
        [Range(0, int.MaxValue, ErrorMessage = "A quantidade deve ser um número não negativo.")]
        public int Quantidade { get; set; } = 1;

        [Required(ErrorMessage = "A quantidade disponível é obrigatória.")]
        [Range(0, int.MaxValue, ErrorMessage = "A quantidade disponível deve ser um número não negativo.")]
        public int QuantidadeDisponivel { get; set; } = 1;

    }
}