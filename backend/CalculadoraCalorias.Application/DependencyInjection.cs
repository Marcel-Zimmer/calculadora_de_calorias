using CalculadoraCalorias.Application.Features;
using CalculadoraCalorias.Application.Interfaces;
using CalculadoraCalorias.Application.Mapping;
using Microsoft.Extensions.DependencyInjection;

namespace CalculadoraCalorias.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddScoped<IUsuarioAppService, UsuarioAppService>();
            services.AddScoped<UsuarioMapper>();
            return services;
        }
    }
}
