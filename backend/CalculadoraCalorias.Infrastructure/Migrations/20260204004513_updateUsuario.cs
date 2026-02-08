using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CalculadoraCalorias.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class updateUsuario : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PerfilBiometrico_Usuarios_UsuarioId1",
                table: "PerfilBiometrico");

            migrationBuilder.DropForeignKey(
                name: "FK_RegistroFisico_Usuarios_UsuarioId1",
                table: "RegistroFisico");

            migrationBuilder.DropIndex(
                name: "IX_RegistroFisico_UsuarioId1",
                table: "RegistroFisico");

            migrationBuilder.DropIndex(
                name: "IX_PerfilBiometrico_UsuarioId1",
                table: "PerfilBiometrico");

            migrationBuilder.DropColumn(
                name: "UsuarioId1",
                table: "RegistroFisico");

            migrationBuilder.DropColumn(
                name: "UsuarioId1",
                table: "PerfilBiometrico");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "UsuarioId1",
                table: "RegistroFisico",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "UsuarioId1",
                table: "PerfilBiometrico",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_RegistroFisico_UsuarioId1",
                table: "RegistroFisico",
                column: "UsuarioId1",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PerfilBiometrico_UsuarioId1",
                table: "PerfilBiometrico",
                column: "UsuarioId1",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_PerfilBiometrico_Usuarios_UsuarioId1",
                table: "PerfilBiometrico",
                column: "UsuarioId1",
                principalTable: "Usuarios",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RegistroFisico_Usuarios_UsuarioId1",
                table: "RegistroFisico",
                column: "UsuarioId1",
                principalTable: "Usuarios",
                principalColumn: "Id");
        }
    }
}
