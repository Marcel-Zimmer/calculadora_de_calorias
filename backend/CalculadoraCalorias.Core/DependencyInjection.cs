using CalculadoraCalorias.Core.Domain.Interfaces;
using CalculadoraCalorias.Core.Domain.Services;
using Microsoft.Extensions.DependencyInjection;

namespace CalculadoraCalorias.Core
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddCore(this IServiceCollection services)
        {
            services.AddScoped<IUsuarioService, UsuarioService>();
            services.AddScoped<IPerfilBiometricoService, PerfilBiometricoService>();
            services.AddScoped<IRegistroFisicoService, RegistroFisicoService>();
            return services;
        }
    }
}
