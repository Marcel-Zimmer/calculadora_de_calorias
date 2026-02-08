using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CalculadoraCalorias.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class updateAtividadeFisica2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RegistroFisico_PerfilBiometrico_PerfilBiometricoId",
                table: "RegistroFisico");

            migrationBuilder.DropIndex(
                name: "IX_RegistroFisico_PerfilBiometricoId",
                table: "RegistroFisico");

            migrationBuilder.DropColumn(
                name: "PerfilBiometricoId",
                table: "RegistroFisico");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "PerfilBiometricoId",
                table: "RegistroFisico",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateIndex(
                name: "IX_RegistroFisico_PerfilBiometricoId",
                table: "RegistroFisico",
                column: "PerfilBiometricoId");

            migrationBuilder.AddForeignKey(
                name: "FK_RegistroFisico_PerfilBiometrico_PerfilBiometricoId",
                table: "RegistroFisico",
                column: "PerfilBiometricoId",
                principalTable: "PerfilBiometrico",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
