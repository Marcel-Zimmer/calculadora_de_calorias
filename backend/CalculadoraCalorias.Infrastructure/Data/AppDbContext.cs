using CalculadoraCalorias.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CalculadoraCalorias.Infrastructure.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }
    public DbSet<Usuario> Usuarios { get; set; }
    public DbSet<PerfilBiometrico> PerfilBiometrico { get; set; }
    public DbSet<RegistroFisico> RegistroFisico { get; set; }
    public DbSet<AtividadeFisica> AtividadeFisica { get; set; }
    public DbSet<Refeicao> Refeicao { get; set; }
    public DbSet<RefreshToken> RefreshTokens { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<RefreshToken>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasOne(d => d.Usuario)
                .WithMany()
                .HasForeignKey(d => d.UsuarioId);
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.Property(u => u.Nome)
                  .HasMaxLength(80)
                  .IsRequired();

            entity.HasIndex(u => u.Email)
                  .IsUnique();

        });

        modelBuilder.Entity<PerfilBiometrico>(entity =>
        {
            entity.HasOne(p => p.Usuario)   
                    .WithOne(u => u.PerfilBiometrico) 
                    .HasForeignKey<PerfilBiometrico>(p => p.UsuarioId)
                    .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<RegistroFisico>(entity =>
        {
            entity.HasOne(r => r.Usuario)
                  .WithMany(u => u.RegistroFisico) 
                  .HasForeignKey(r => r.UsuarioId)
                  .OnDelete(DeleteBehavior.Cascade);

        });
    }
}
