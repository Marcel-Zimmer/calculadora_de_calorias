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
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

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
                  .WithOne()                  
                  .HasForeignKey<PerfilBiometrico>(p => p.UsuarioId) 
                  .OnDelete(DeleteBehavior.Cascade); 
        });

        modelBuilder.Entity<RegistroFisico>(entity =>
        {
            entity.HasOne(r => r.Usuario)
                  .WithMany() 
                  .HasForeignKey(r => r.UsuarioId)
                  .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(r => r.PerfilBiometrico)
                  .WithMany()
                  .HasForeignKey(r => r.PerfilBiometricoId)
                  .OnDelete(DeleteBehavior.Cascade);
        });
    }
}
