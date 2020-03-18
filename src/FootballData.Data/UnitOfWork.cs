using FootballData.Core;
using FootballData.Core.Repositories;
using FootballData.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FootballData.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly FootballDbContext context;
        private ITeamRepository teamRepository;
        private IPlayerRepository playerRepository;
        private ICompetitionRepository competitionRepository;
        private ICompetitionTeamRepository competitionTeams;

        public UnitOfWork(FootballDbContext context)
        {
            this.context = context;
        }

        public ICompetitionRepository Competitions => competitionRepository = competitionRepository ?? new CompetitionRepository(context);

        public IPlayerRepository Players => playerRepository = playerRepository ?? new PlayerRepository(context);

        public ITeamRepository Teams => teamRepository = teamRepository ?? new TeamRepository(context);

        public ICompetitionTeamRepository CompetitionTeams => competitionTeams = competitionTeams ?? new CompetitionTeamRepository(context);

        public async Task<int> CommitAsync()
        {
            return await context.SaveChangesAsync();
        }

        public void Dispose()
        {
            context.Dispose();
        }
    }
}
