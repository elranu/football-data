using FootballData.Core.Models;
using FootballData.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace FootballData.Data.Repositories
{
    public class CompetitionTeamRepository : Repository<CompetitionTeam>, ICompetitionTeamRepository
    {

        public CompetitionTeamRepository(FootballDbContext context) : base (context)
        {

        }
    }
}
