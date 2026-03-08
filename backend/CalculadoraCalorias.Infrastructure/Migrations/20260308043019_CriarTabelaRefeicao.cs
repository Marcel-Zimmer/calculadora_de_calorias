using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace CalculadoraCalorias.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class CriarTabelaRefeicao : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Refeicao",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UsuarioId = table.Column<long>(type: "bigint", nullable: false),
                    Apelido = table.Column<string>(type: "text", nullable: true),
                    Peso = table.Column<int>(type: "integer", nullable: false),
                    Tipo = table.Column<int>(type: "integer", nullable: false),
                    Data = table.Column<DateOnly>(type: "date", nullable: false),
                    GuidArquivo = table.Column<Guid>(type: "uuid", nullable: false),
                    StatusProcessamento = table.Column<int>(type: "integer", nullable: false),
                    Alimento = table.Column<string>(type: "text", nullable: true),
                    Calorias = table.Column<double>(type: "double precision", nullable: true),
                    Proteinas = table.Column<double>(type: "double precision", nullable: true),
                    Carboidratos = table.Column<double>(type: "double precision", nullable: true),
                    Gorduras = table.Column<double>(type: "double precision", nullable: true),
                    Acucares = table.Column<double>(type: "double precision", nullable: true),
                    Fibras = table.Column<double>(type: "double precision", nullable: true),
                    UtilizadoRefeicaoModelo = table.Column<bool>(type: "boolean", nullable: true),
                    CodigoRefeicaoModelo = table.Column<long>(type: "bigint", nullable: true),
                    RefeicaoModeloId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Refeicao", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Refeicao_Refeicao_RefeicaoModeloId",
                        column: x => x.RefeicaoModeloId,
                        principalTable: "Refeicao",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Refeicao_Usuarios_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Refeicao_RefeicaoModeloId",
                table: "Refeicao",
                column: "RefeicaoModeloId");

            migrationBuilder.CreateIndex(
                name: "IX_Refeicao_UsuarioId",
                table: "Refeicao",
                column: "UsuarioId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Refeicao");
        }
    }
}
