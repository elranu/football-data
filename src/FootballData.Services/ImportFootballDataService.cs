using FootballData.Core;
using FootballData.Core.Models;
using FootballData.Services.Interfaces;
using FootballData.Services.Options;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FootballData.Services
{
    public class ImportFootballDataService : IImportFootballDataService
    {
        private readonly ICompetitionService competitionService;
        private readonly HttpClient client;
        private readonly IUnitOfWork unitOfWork;
        private readonly Uri BASE_URL;
        private readonly int maxRequestsPerInterval;
        private readonly int intervalMs;

        public ImportFootballDataService(ICompetitionService competitionService, HttpClient client, 
            IUnitOfWork unitOfWork, IOptions<FootballServiceOptions> options)
        {
            this.competitionService = competitionService;
            this.client = client;
            this.unitOfWork = unitOfWork;
            this.BASE_URL = string.IsNullOrEmpty(options.Value.BaseURL) ? new Uri("https://api.football-data.org/v2/")
                : new Uri(options.Value.BaseURL);
            this.intervalMs = 1000 * (options.Value.IntervalSecs ?? 60);
            this.maxRequestsPerInterval = options.Value.MaxRequestPerInterval ?? 10;
            this.client.DefaultRequestHeaders.Add("X-Auth-Token", options.Value.AuthToken);//"692b28cc133343c7a6c8f0564050ba17"); //Todo: hardcoded
        }

        public async Task<IOperationResult<OperationResultType>> ImportLeagueAsync(string code)
        {
            var competition = await this.unitOfWork.Competitions.GetCompetitionByCodeAsync(code);
            if (competition != null) return OperationResult.Succeeded<OperationResultType>(OperationResultType.AlreadyDone);

            competition = new Competition();
            var teams = new List<Team>();
            using (var response = await this.client.GetAsync(new Uri(BASE_URL, $"competitions/{code}/teams")))
            {
                if(response.StatusCode == HttpStatusCode.BadRequest) return OperationResult.Succeeded<OperationResultType>(OperationResultType.NotFound);
                response.EnsureSuccessStatusCode();
                var result = await response.Content.ReadAsStringAsync();
                dynamic teamsResult = JsonConvert.DeserializeObject<ExpandoObject>(result, new ExpandoObjectConverter());

                competition.IdService = int.Parse(teamsResult.competition.id.ToString());
                competition.Name = teamsResult.competition.name;
                competition.Code = teamsResult.competition.code;
                competition.AreaName = teamsResult.competition.area.name;
                
                FillTeams(teams, teamsResult);
            }
            
            await FillSquadAsync(teams);
            await this.competitionService.CreateFullCompetitionAsync(competition, teams);
            return OperationResult.Succeeded<OperationResultType>(OperationResultType.Successfull);
        }

        private void FillTeams(List<Team> teams, dynamic teamsResult)
        {
            if (teamsResult.teams != null)
            {
                foreach (var team in teamsResult.teams)
                {
                    var newTeam = new Team()
                    {
                        AreaName = team.area.name,
                        IdService = int.Parse(team.id.ToString()),
                        Name = team.name,
                        ShortName = team.shortName,
                        Email = team.email
                    };
                    teams.Add(newTeam);
                }
            }
        }

        private async Task<List<Team>> FillSquadAsync(List<Team> teams)
        {
            var listTasks = new List<Task<Team>>();

            Thread.Sleep(this.intervalMs);
            for (int i = 1; i <= teams.Count; i++)
            {
                if(i % this.maxRequestsPerInterval == 0)
                {
                    await Task.WhenAll(listTasks);
                    listTasks.Clear();
                    Thread.Sleep(this.intervalMs);
                }
                listTasks.Add(FillSquad(teams[i - 1]));
            }

            return teams;
        }

        private async Task<Team> FillSquad(Team team)
        {
            if (team == null) return null;
            if (team.Squad == null) team.Squad = new List<Player>();

            using (var response = await this.client.GetAsync(new Uri(BASE_URL, $"teams/{team.IdService}")))
            {
                try
                {
                    response.EnsureSuccessStatusCode();
                    var result = await response.Content.ReadAsStringAsync();
                    dynamic teamResult = JsonConvert.DeserializeObject<ExpandoObject>(result, new ExpandoObjectConverter());
                    foreach (var player in teamResult.squad)
                    {
                        var newPlayer = new Player();
                        try
                        {
                            newPlayer.IdService = int.Parse(player.id.ToString());
                            newPlayer.Name = player.name;
                            newPlayer.CountryOfBirth = player.countryOfBirth;
                            newPlayer.Nationality = player.nationality;
                            newPlayer.Position = player.position ?? string.Empty;
                            newPlayer.DateOfBirth = player.dateOfBirth == null ? null :
                                DateTime.Parse(player.dateOfBirth.ToString());
                            newPlayer.Team = team;

                            team.Squad.Add(newPlayer);
                        }
                        catch(Exception e)
                        {
                            throw new Exception($"Error on playerId: {newPlayer.IdService}", e);
                        }
                    }
                    return team;
                }
                catch (Exception e)
                {
                    throw new Exception($"Error getting team Id: {team.IdService}", e);
                }
            }
        }
    }
}
