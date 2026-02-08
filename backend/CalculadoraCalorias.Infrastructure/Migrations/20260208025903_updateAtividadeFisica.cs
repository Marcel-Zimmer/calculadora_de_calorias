using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CalculadoraCalorias.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class updateAtividadeFisica : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AtividadeFisica_Usuarios_UsuarioId1",
                table: "AtividadeFisica");

            migrationBuilder.DropIndex(
                name: "IX_AtividadeFisica_UsuarioId1",
                table: "AtividadeFisica");

            migrationBuilder.DropColumn(
                name: "DuracaoMinutos",
                table: "AtividadeFisica");

            migrationBuilder.DropColumn(
                name: "UsuarioId1",
                table: "AtividadeFisica");

            migrationBuilder.AlterColumn<long>(
                name: "UsuarioId",
                table: "AtividadeFisica",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddColumn<TimeSpan>(
                name: "TempoExercicio",
                table: "AtividadeFisica",
                type: "interval",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0));

            migrationBuilder.AddColumn<double>(
                name: "VelocidadeMedia",
                table: "AtividadeFisica",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.CreateIndex(
                name: "IX_AtividadeFisica_UsuarioId",
                table: "AtividadeFisica",
                column: "UsuarioId");

            migrationBuilder.AddForeignKey(
                name: "FK_AtividadeFisica_Usuarios_UsuarioId",
                table: "AtividadeFisica",
                column: "UsuarioId",
                principalTable: "Usuarios",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AtividadeFisica_Usuarios_UsuarioId",
                table: "AtividadeFisica");

            migrationBuilder.DropIndex(
                name: "IX_AtividadeFisica_UsuarioId",
                table: "AtividadeFisica");

            migrationBuilder.DropColumn(
                name: "TempoExercicio",
                table: "AtividadeFisica");

            migrationBuilder.DropColumn(
                name: "VelocidadeMedia",
                table: "AtividadeFisica");

            migrationBuilder.AlterColumn<int>(
                name: "UsuarioId",
                table: "AtividadeFisica",
                type: "integer",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AddColumn<int>(
                name: "DuracaoMinutos",
                table: "AtividadeFisica",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<long>(
                name: "UsuarioId1",
                table: "AtividadeFisica",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AtividadeFisica_UsuarioId1",
                table: "AtividadeFisica",
                column: "UsuarioId1");

            migrationBuilder.AddForeignKey(
                name: "FK_AtividadeFisica_Usuarios_UsuarioId1",
                table: "AtividadeFisica",
                column: "UsuarioId1",
                principalTable: "Usuarios",
                principalColumn: "Id");
        }
    }
}
