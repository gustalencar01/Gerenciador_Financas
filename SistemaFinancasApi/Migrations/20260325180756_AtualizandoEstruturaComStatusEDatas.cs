using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SistemaFinancasApi.Migrations
{
    /// <inheritdoc />
    public partial class AtualizandoEstruturaComStatusEDatas : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "Data",
                table: "Receitas",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "Data",
                table: "Despesas",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "Pago",
                table: "Despesas",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "Limites",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Categoria = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ValorLimite = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Limites", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Limites");

            migrationBuilder.DropColumn(
                name: "Data",
                table: "Receitas");

            migrationBuilder.DropColumn(
                name: "Data",
                table: "Despesas");

            migrationBuilder.DropColumn(
                name: "Pago",
                table: "Despesas");
        }
    }
}
