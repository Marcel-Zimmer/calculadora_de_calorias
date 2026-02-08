namespace CalculadoraCalorias.Infrastructure

{
    using CalculadoraCalorias.Core.Domain.Interfaces;
    using CalculadoraCalorias.Infrastructure.Data;
    using CalculadoraCalorias.Infrastructure.Repository;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;

    public static class DependencyInjection
    {

        public static IServiceCollection AddInfrastructure(this IServiceCollection services, string connectionString)
        {
            services.AddDbContext<AppDbContext>(options =>
                options.UseNpgsql(connectionString));

            services.AddScoped<IUsuarioRepository, UsuarioRepository>();
            services.AddScoped<IPerfilBiometricoRepository, PerfilBiometricoRepository>();

            return services;
        }
    }
}
