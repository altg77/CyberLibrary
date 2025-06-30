using System.ComponentModel.DataAnnotations;

namespace CyberLibrary2.Models
{
    public class Usuario
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "O nome de usuário é obrigatório.")]
        [StringLength(100, ErrorMessage = "O nome de usuário não pode ter mais de 100 caracteres.")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "O email é obrigatório.")]
        [EmailAddress(ErrorMessage = "Formato de email inválido.")]
        public string Email { get; set; }

        public string Cargo { get; set; } = "Aluno";


        [StringLength(500, ErrorMessage = "A URL da imagem não pode exceder 500 caracteres.")]
        public byte[]? ImagemUrl { get; set; }


        [Phone(ErrorMessage = "Formato de telefone inválido.")]
        [StringLength(20, ErrorMessage = "O telefone não pode ter mais de 20 caracteres.")]
        [Display(Name = "Telefone")]
        public string? Telefone { get; set; }


        [Display(Name = "Turma")]
        public int? TurmaId { get; set; }


        //Login e Senha
        [Required(ErrorMessage = "O login é obrigatório.")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "O login deve ter entre 3 e 50 caracteres.")]
        [Display(Name = "Login de Acesso")]
        public string Login { get; set; }

        [Required(ErrorMessage = "A senha é obrigatória.")]
        [DataType(DataType.Password)]
        public string Senha { get; set; }


    }
}