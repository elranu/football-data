using FootballData.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FootballData.Core.Repositories
{
    public interface IPlayerRepository : IRepository<Player>
    {
        Task<int?> CountTotalPlayersInCompetitionAsync(string code);
    }
}
