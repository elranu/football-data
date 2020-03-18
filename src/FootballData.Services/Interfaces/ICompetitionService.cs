using FootballData.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FootballData.Services.Interfaces
{
    public interface ICompetitionService
    {
        Task<Competition> CreateFullCompetitionAsync(Competition competition, ICollection<Team> teams = null);
    }
}