using System.Threading.Tasks;

namespace FootballData.Services.Interfaces
{
    public interface IPlayerService
    {
        Task<int?> CountTotalPlayersOnCompetition(string code);
    }
}