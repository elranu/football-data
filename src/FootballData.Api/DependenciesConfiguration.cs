using FootballData.Core;
using FootballData.Data;
using FootballData.Services;
using FootballData.Services.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace FootballData.Api
{
    public static class DependenciesConfiguration
    {
        public static void SetupDependencies(this IServiceCollection services, IConfiguration configuration )
        {
            services.Configure<FootballServiceOptions>(configuration.GetSection("FootballService"));
            services.SetupDataDependencies(configuration);
            services.SetupServicesDependencies(configuration);

            services.AddTransient<IUnitOfWork, UnitOfWork>();
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo { Title = "Football-Data", Version = "v1" });
            });
        }
    }
}
