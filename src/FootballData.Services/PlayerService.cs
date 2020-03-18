using FootballData.Core;
using FootballData.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FootballData.Services
{
    public class PlayerService : IPlayerService
    {
        private readonly IUnitOfWork unitOfWork;

        public PlayerService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public async Task<int?> CountTotalPlayersOnCompetition(string code)
        {
            return await this.unitOfWork.Players.CountTotalPlayersInCompetitionAsync(code);
        }
    }
}
