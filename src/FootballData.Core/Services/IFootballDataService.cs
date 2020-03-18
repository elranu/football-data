using FootballData.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FootballData.Core.Services
{
    interface IFootballDataService
    {
        Task ImportCompetitionAsync(string leagueCode);
        Task<int> TotalPlayers(string leagueCode);
        Task<Competition> GetCompetition(string leagueCode);
    }
}
