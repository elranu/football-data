using Xunit;
using FootballData.Services;
using System;
using System.Collections.Generic;
using System.Text;
using FootballData.Data;
using FootballServicesTests.Tools;
using Moq;
using System.Net.Http;
using System.Net;
using Moq.Contrib.HttpClient;
using System.Threading.Tasks;
using FootballData.Core;
using System.IO;
using FootballData.Services.Interfaces;
using FootballData.Core.Models;
using System.Threading;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Microsoft.Extensions.Options;
using FootballData.Services.Options;

namespace FootballData.Services.Tests
{
    public class ImportServiceTests : IDisposable
    {
        private FootballDbContext context;

        public ImportServiceTests()
        {
            this.context = DatabaseTools.NewFootballDbContext("importDB");
        }


        [Fact()]
        public async Task ImportLeagueWithInvalidCodeAsyncTest()
        {
            var unitOfWork = new UnitOfWork(this.context);
            var mock = new Mock<ICompetitionService>();
            var handler = new Mock<HttpMessageHandler>();
            var httpClient = handler.CreateClient();
            SetupHandler(handler);
            var importService = new ImportFootballDataService(mock.Object, httpClient, unitOfWork, GetOptions());

            var result = await importService.ImportLeagueAsync("llele");

            Assert.Equal(OperationResultType.NotFound, result.GetSucceededValue());
        }

        [Fact()]
        public async Task ImportPLLeagueAsync()
        {
            var unitOfWork = new UnitOfWork(this.context);
            var mock = new Mock<ICompetitionService>();
            mock.Setup(x => x.CreateFullCompetitionAsync(It.IsAny<Competition>(), It.IsAny<ICollection<Team>>())).ReturnsAsync(new Competition());
            var handler = new Mock<HttpMessageHandler>();
            var httpClient = handler.CreateClient();
            SetupHandler(handler);
            
            var importService = new ImportFootballDataService(mock.Object, httpClient, unitOfWork, GetOptions());
            var result = await importService.ImportLeagueAsync("PL");

            Assert.Equal(OperationResultType.Successfull, result.GetSucceededValue());
            mock.Verify(x => x.CreateFullCompetitionAsync(It.IsAny<Competition>(), It.IsAny<ICollection<Team>>()), Times.Once);
        }

        [Fact]
        public async Task ImportPLLeague2TimesAsync()
        {
            var unitOfWork = new UnitOfWork(this.context);
            var competitionService = new CompetitionService(unitOfWork);
            var handler = new Mock<HttpMessageHandler>();
            var httpClient = handler.CreateClient();
            SetupHandler(handler);

            var importService = new ImportFootballDataService(competitionService, httpClient, unitOfWork, GetOptions());
            var result = await importService.ImportLeagueAsync("PL");

            Assert.Equal(OperationResultType.Successfull, result.GetSucceededValue());
            result = await importService.ImportLeagueAsync("PL");
            Assert.Equal(OperationResultType.AlreadyDone, result.GetSucceededValue());
        } 

        private static void SetupHandler(Mock<HttpMessageHandler> handler)
        {
            handler.SetupAnyRequest().ReturnsResponse(HttpStatusCode.BadRequest);
            handler.SetupRequest(HttpMethod.Get, "https://api.football-data.org/v2/competitions/PL/teams")
                .ReturnsResponse(HttpStatusCode.OK, File.ReadAllText("PLResponse.json"), "application/json");
            handler.SetupRequest(HttpMethod.Get, r => {
                return r.RequestUri.ToString().StartsWith("https://api.football-data.org/v2/teams");
            })
                .ReturnsAsync((HttpRequestMessage request, CancellationToken cancellationToken) =>
                {
                    var id = int.Parse(request.RequestUri.Segments.Last());
                    var team = new { AreaName = "England", Email = $"mail{id}@mail.com", Id = id, Name = $"Name{id}", ShortName = $"short{id}", Squad = new List<Object>() };

                    var idPlayerStart = id * 100000;
                    for (int j = idPlayerStart; j < idPlayerStart + 20; j++)
                    {
                        var num = $"{id}{j}";
                        var ply = new { Name = "Name" + num, Position = "position" + num, Id = j, DateOfBirth = DateTime.Now.AddYears(-20), CountryOfBirth = "Argentina", Team = team, Nationality = "Argentina" };
                        team.Squad.Add(ply);
                    }

                    var json = JsonConvert.SerializeObject(team, new JsonSerializerSettings()
                    {
                        ContractResolver = new CamelCasePropertyNamesContractResolver(),
                        ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                    });
                    return new HttpResponseMessage()
                    {
                        Content = new StringContent(json, Encoding.UTF8, "application/json")
                    };
                });
        }

        private static IOptions<FootballServiceOptions> GetOptions()
        {
            var options = new FootballServiceOptions()
            { BaseURL = "https://api.football-data.org/v2/", IntervalSecs = 0, MaxRequestPerInterval = 10 };
            return Microsoft.Extensions.Options.Options.Create<FootballServiceOptions>(options);
        }

        public void Dispose()
        {
            this.context.Database.EnsureDeleted();
            this.context.Dispose();
           
        }
    }
}