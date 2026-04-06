using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CalculadoraCalorias.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AtualizacaoTabelaAtividadeFisica : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CaloriasCalculadas",
                table: "AtividadeFisica",
                newName: "CaloriasEstimadas");

            migrationBuilder.AlterColumn<double>(
                name: "VelocidadeMedia",
                table: "AtividadeFisica",
                type: "double precision",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "double precision");

            migrationBuilder.AlterColumn<decimal>(
                name: "PesoSnapshot",
                table: "AtividadeFisica",
                type: "numeric",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "numeric");

            migrationBuilder.AlterColumn<DateOnly>(
                name: "DataExercicio",
                table: "AtividadeFisica",
                type: "date",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CaloriasEstimadas",
                table: "AtividadeFisica",
                newName: "CaloriasCalculadas");

            migrationBuilder.AlterColumn<double>(
                name: "VelocidadeMedia",
                table: "AtividadeFisica",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0,
                oldClrType: typeof(double),
                oldType: "double precision",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "PesoSnapshot",
                table: "AtividadeFisica",
                type: "numeric",
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "numeric",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "DataExercicio",
                table: "AtividadeFisica",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateOnly),
                oldType: "date");
        }
    }
}
