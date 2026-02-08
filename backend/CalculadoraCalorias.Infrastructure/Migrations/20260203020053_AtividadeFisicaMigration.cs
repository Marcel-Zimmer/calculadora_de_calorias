using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace CalculadoraCalorias.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AtividadeFisicaMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.CreateTable(
                name: "AtividadeFisica",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UsuarioId = table.Column<int>(type: "integer", nullable: false),
                    TipoAtividadeId = table.Column<int>(type: "integer", nullable: false),
                    PesoSnapshot = table.Column<decimal>(type: "numeric", nullable: false),
                    CaloriasCalculadas = table.Column<decimal>(type: "numeric", nullable: false),
                    DataExercicio = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DuracaoMinutos = table.Column<int>(type: "integer", nullable: false),
                    UsuarioId1 = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AtividadeFisica", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AtividadeFisica_Usuarios_UsuarioId1",
                        column: x => x.UsuarioId1,
                        principalTable: "Usuarios",
                        principalColumn: "Id");
                });

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

            migrationBuilder.CreateIndex(
                name: "IX_AtividadeFisica_UsuarioId1",
                table: "AtividadeFisica",
                column: "UsuarioId1");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PerfilBiometrico_Usuarios_UsuarioId1",
                table: "PerfilBiometrico");

            migrationBuilder.DropForeignKey(
                name: "FK_RegistroFisico_Usuarios_UsuarioId1",
                table: "RegistroFisico");

            migrationBuilder.DropTable(
                name: "AtividadeFisica");

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
    }
}
