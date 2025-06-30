using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CyberLibrary2.Migrations
{
    /// <inheritdoc />
    public partial class atualizarLivro : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Formato",
                table: "Livros");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Formato",
                table: "Livros",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");
        }
    }
}
