using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace FootballData.Data
{
    public static class DependeciesDataConfiguration
    {

        public static void SetupDataDependencies(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<FootballDbContext>(options =>
                options.UseSqlServer(configuration.GetValue<string>("DefaultConnectionString"),
                x => x.MigrationsAssembly("FootballData.Data")));
        }


    }
}
