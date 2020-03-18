using FootballData.Core;
using FootballData.Core.Models;
using FootballData.Data;
using FootballData.Services;
using FootballServicesTests.Tools;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace FootballServicesTests
{
    public class CompetitionServiceTests : IDisposable
    {
        private readonly FootballDbContext context;
        private readonly CompetitionService competitionService;
        private const string DB_NAME = "CompetitionDb";

        public CompetitionServiceTests()
        {
            this.context = DatabaseTools.NewFootballDbContext(DB_NAME);
            this.competitionService = new CompetitionService(new UnitOfWork(this.context));
        }

        [Fact]
        public async Task TestFullCompetitionCreation()
        {
            var competition = new Competition() { AreaName = "England", Code = "PL", Name = "Primer League", IdService = 22 };
            var teams = GetGenericTeams();

            var result = await this.competitionService.CreateFullCompetitionAsync(competition, teams);
            Assert.True(result.Id != 0);

            var dbContext = DatabaseTools.NewFootballDbContext(DB_NAME);
            var dbCompetition = await dbContext.Competitions.Include(c=> c.Teams)
                .ThenInclude(ct=> ct.Team).ThenInclude(t => t.Squad)
                .FirstAsync(c => c.IdService == competition.IdService);
            Assert.True(dbCompetition.Name == competition.Name);
            var dbTeams = dbCompetition.Teams.Select(ct => ct.Team);
            Assert.Equal(15, dbTeams.Count());
            Assert.Equal(23 * 15, dbTeams.SelectMany(x=> x.Squad).Count());
        }

        [Fact]
        public async Task TestNotTeamDuplication()
        {
            var competition1 = new Competition() { AreaName = "Argentina", Code = "SL", Name = "SuperLiga", IdService = 24 };
            var competition2 = new Competition() { AreaName = "South America", Code = "LIB", Name = "Libertadores", IdService = 22 };
            var teams1 = GetGenericTeams();
            var teams2 = GetGenericTeams(14, 15, 24, 23);

            await this.competitionService.CreateFullCompetitionAsync(competition1, teams1);
            await this.competitionService.CreateFullCompetitionAsync(competition2, teams2);

            var dbContext = DatabaseTools.NewFootballDbContext(DB_NAME);
            var list = dbContext.Teams.Include(t => t.Competitions).ThenInclude(ct => ct.Competition)
                .Where(t => t.IdService == 14).ToList();
            Assert.Single(list);
            Assert.True(list.First().Competitions.Count == 2);
            Assert.Contains(list.First().Competitions.Select(ct => ct.Competition),
                t => t.IdService == competition1.IdService);
        }

        public List<Team> GetGenericTeams(int teamsIdStart = 1, int totalTeams = 15, 
            int playersIdStart = 1, int totalPlayers = 23)
        {
            var listTeams = new List<Team>();
            for (int i = teamsIdStart; i < teamsIdStart + totalTeams; i++)
            {
                var team = new Team()
                {
                    IdService = i,
                    AreaName = "England",
                    Name = "TeamLong" + i,
                    Email = $"some{i}@mail.com",
                    ShortName = "Team" + i
                };
                
                var players = new List<Player>();
                for (int j = playersIdStart; j < playersIdStart + totalPlayers; j++)
                {
                    var num = $"{i}{j}";
                    var ply = new Player() { Name = "Name" + num, Position = "position" + num, IdService = j, DateOfBirth = DateTime.Now.AddYears(-j), CountryOfBirth = "Argentina", Team = team };
                    players.Add(ply);
                }
                team.Squad = players;
                listTeams.Add(team);
            };
            return listTeams;
        }

        public void Dispose()
        {
            this.context.Database.EnsureDeleted();
            this.context.Dispose();
        }
    }
}
