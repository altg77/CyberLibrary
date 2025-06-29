using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CyberLibrary2.Migrations
{
    /// <inheritdoc />
    public partial class Arquivo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CapaUrl",
                table: "Livros");

            migrationBuilder.AddColumn<byte[]>(
                name: "CapaImagem",
                table: "Livros",
                type: "longblob",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CapaImagem",
                table: "Livros");

            migrationBuilder.AddColumn<string>(
                name: "CapaUrl",
                table: "Livros",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");
        }
    }
}
