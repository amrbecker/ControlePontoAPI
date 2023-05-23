using ControlePonto.Business.Interfaces;
using ControlePonto.Business.Services;
using ControlePonto.Data.Context;
using ControlePonto.Data.Repository;
using Microsoft.AspNetCore.Mvc.DataAnnotations;
using Microsoft.Extensions.DependencyInjection;

namespace ControlePonto.API.Configurations
{
    public static class DependencyInjectionConfig
    {
        public static IServiceCollection ResolveDependencies(this IServiceCollection services)
        {
            services.AddScoped<ControlePontoDbContext>();
            services.AddScoped<IPontoRepository, PontoRepository>();
            services.AddScoped<IPontoService, PontoService>();

            return services;
        }
    }
}