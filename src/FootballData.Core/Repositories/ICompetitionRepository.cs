using FootballData.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FootballData.Core.Repositories
{
    public interface ICompetitionRepository : IRepository<Competition>
    {
        Task<Competition> GetCompetitionByCodeAsync(string code);
        Task<Competition> GetCompetitionByIdServiceAsync(int idService);
    }
}
