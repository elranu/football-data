using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FootballData.Services.Interfaces;
using FootballData.Services.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FootballData.Services
{
    public static class DependenciesServicesConfiguration
    {

        public static void SetupServicesDependencies(this IServiceCollection services, IConfiguration configuration)
        {
            //services.Configure<FootballServiceOptions>(configuration.GetSection("FootballService"));
            services.AddHttpClient<IImportFootballDataService, ImportFootballDataService>();
            services.AddTransient<IPlayerService, PlayerService>();
            services.AddTransient<ICompetitionService, CompetitionService>();
        }
    }
}
