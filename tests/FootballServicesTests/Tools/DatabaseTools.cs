using FootballData.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace FootballServicesTests.Tools
{
    public class DatabaseTools
    {

        public static FootballDbContext NewFootballDbContext(string databaseName = "testDB")
        {
            var options = new DbContextOptionsBuilder<FootballDbContext>()
                .UseInMemoryDatabase(databaseName: databaseName)
                .EnableSensitiveDataLogging()
                .Options;


            var context = new FootballDbContext(options);
            return context;
        }
    }
}
