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
    public class PlayerRepository: Repository<Player>, IPlayerRepository
    {
        private readonly FootballDbContext context;

        public PlayerRepository(FootballDbContext context) : base(context)
        {
            this.context = context;
        }

        public async Task<int?> CountTotalPlayersInCompetitionAsync(string code)
        {
            var competition = await this.context.Competitions.Include(c => c.Teams).ThenInclude( ct => ct.Team)
                .SingleOrDefaultAsync(c => c.Code == code.ToUpperInvariant() || c.Code == code);
            if (competition == null) return null;

            var count =  await this.context.Players.Where(p =>
                competition.Teams.Select(ct => ct.Team).Contains(p.Team)).CountAsync();
            
            return count;
        }
    }
}
