using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace CalculadoraCalorias.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RegistroFisicoMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RegistroFisico",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UsuarioId = table.Column<long>(type: "bigint", nullable: false),
                    PerfilBiometricoId = table.Column<long>(type: "bigint", nullable: false),
                    DataRegistro = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    PesoKg = table.Column<decimal>(type: "numeric", nullable: false),
                    ImcCalculado = table.Column<decimal>(type: "numeric", nullable: false),
                    TaxaMetabolicaBasal = table.Column<decimal>(type: "numeric", nullable: false),
                    MetaCaloricaDiaria = table.Column<decimal>(type: "numeric", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RegistroFisico", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RegistroFisico_PerfilBiometrico_PerfilBiometricoId",
                        column: x => x.PerfilBiometricoId,
                        principalTable: "PerfilBiometrico",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RegistroFisico_Usuarios_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RegistroFisico_PerfilBiometricoId",
                table: "RegistroFisico",
                column: "PerfilBiometricoId");

            migrationBuilder.CreateIndex(
                name: "IX_RegistroFisico_UsuarioId",
                table: "RegistroFisico",
                column: "UsuarioId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RegistroFisico");
        }
    }
}
