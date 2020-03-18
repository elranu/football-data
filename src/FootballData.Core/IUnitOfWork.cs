using FootballData.Core.Repositories;
using System;
using System.Threading.Tasks;


namespace FootballData.Core
{
    public interface IUnitOfWork : IDisposable
    {
        ICompetitionRepository Competitions { get; }
        IPlayerRepository Players { get;  }
        ITeamRepository Teams { get;  }
        ICompetitionTeamRepository CompetitionTeams { get; }
        
        Task<int> CommitAsync();

    }
}
