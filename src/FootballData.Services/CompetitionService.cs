using FootballData.Core;
using FootballData.Core.Models;
using FootballData.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FootballData.Services
{
    public class CompetitionService : ICompetitionService
    {
        private readonly IUnitOfWork unitOfWork;

        public CompetitionService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public async Task<Competition> CreateFullCompetitionAsync(Competition competition, ICollection<Team> teams = null)
        {
            await this.unitOfWork.Competitions.AddAsync(competition);
            foreach (var team in teams ?? new List<Team>())
            {
                var dbTeam = await this.unitOfWork.Teams.GetByIdServiceAsync(team.IdService);
                if(dbTeam != null)
                {
                    var newCompetitionTeam = new CompetitionTeam
                    {
                        Team = dbTeam,
                        Competition = competition
                    };
                    await this.unitOfWork.CompetitionTeams.AddAsync(newCompetitionTeam);
                    continue;
                }
                
                var competitionTeam = new CompetitionTeam
                {
                    Team = team,
                    Competition = competition
                };
                await this.unitOfWork.CompetitionTeams.AddAsync(competitionTeam);
                await this.unitOfWork.Teams.AddAsync(team);
                await this.unitOfWork.Players.AddRangeAsync(team.Squad);
            }

            await this.unitOfWork.CommitAsync();
            return competition;
        }

    }
}
