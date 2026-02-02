using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CalculadoraCalorias.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RegistroFisicoMigration2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "MetaCaloricaDiaria",
                table: "RegistroFisico",
                type: "numeric",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "numeric");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "MetaCaloricaDiaria",
                table: "RegistroFisico",
                type: "numeric",
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "numeric",
                oldNullable: true);
        }
    }
}
