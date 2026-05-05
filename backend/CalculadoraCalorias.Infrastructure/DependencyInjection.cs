namespace CalculadoraCalorias.Infrastructure

{
    using CalculadoraCalorias.Core.Domain.Entities;
    using CalculadoraCalorias.Core.Domain.Interfaces;
    using CalculadoraCalorias.Core.Domain.Services;
    using CalculadoraCalorias.Infrastructure.Data;
    using CalculadoraCalorias.Infrastructure.Repository;
    using CalculadoraCalorias.Infrastructure.Services;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;

    public static class DependencyInjection
    {

        public static IServiceCollection AddInfrastructure(this IServiceCollection services, string connectionString)
        {
            services.AddDbContext<AppDbContext>(options =>
                options.UseNpgsql(connectionString, sqlOptions =>
                {
                    sqlOptions.EnableRetryOnFailure(
                        maxRetryCount: 5,
                        maxRetryDelay: TimeSpan.FromSeconds(10),
                        errorCodesToAdd: null);
                }));

            services.AddIdentity<ApplicationUser, IdentityRole<long>>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequiredLength = 6;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireLowercase = false;
            })
            .AddEntityFrameworkStores<AppDbContext>()
            .AddDefaultTokenProviders();

            services.AddScoped<IPasswordHasher<ApplicationUser>, CustomPasswordHasher>();

            services.AddScoped<IUsuarioRepository, UsuarioRepository>();
            services.AddScoped<IPerfilBiometricoRepository, PerfilBiometricoRepository>();
            services.AddScoped<IRegistroFisicoRepository, RegistroFisicoRepository>();
            services.AddScoped<IAtividadeFisicaRepository, AtividadeFisicaRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped(typeof(IRepositoryBase<>), typeof(RepositoryBase<>));
            services.AddScoped<IRefeicaoRepository, RefeicaoRepository>();
            services.AddScoped<IRegistroAguaRepository, RegistroAguaRepository>();
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();

            return services;
        }
    }
}
