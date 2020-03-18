using FootballData.Core;
using System.Threading.Tasks;

namespace FootballData.Services.Interfaces
{
    public interface IImportFootballDataService
    {
        Task<IOperationResult<OperationResultType>> ImportLeagueAsync(string code);
    }
}