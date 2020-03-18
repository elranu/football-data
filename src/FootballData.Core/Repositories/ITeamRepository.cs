using FootballData.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FootballData.Core.Repositories
{
    public interface ITeamRepository : IRepository<Team>
    {
        Task<Team> GetByIdServiceAsync(int idService);
    }
}
