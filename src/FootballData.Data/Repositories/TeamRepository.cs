using FootballData.Core.Models;
using FootballData.Core.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FootballData.Data.Repositories
{
    public class TeamRepository: Repository<Team>, ITeamRepository
    {
        private readonly FootballDbContext context;

        public TeamRepository(FootballDbContext context) : base (context)
        {
            this.context = context;
        }

        public async Task<Team> GetByIdServiceAsync(int idService) => 
            await this.context.Teams.FirstOrDefaultAsync(t => t.IdService == idService);
    }
}
