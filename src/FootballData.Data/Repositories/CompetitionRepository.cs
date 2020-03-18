using FootballData.Core.Models;
using FootballData.Core.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FootballData.Data.Repositories
{
    public class CompetitionRepository : Repository<Competition>, ICompetitionRepository
    {
        private readonly FootballDbContext context;

        public CompetitionRepository(FootballDbContext context) : base(context)
        {
            this.context = context;
        }

        public async Task<Competition> GetCompetitionByIdServiceAsync(int idService) => 
            await this.context.Competitions.FirstOrDefaultAsync(c => c.IdService == idService);

        public async Task<Competition> GetCompetitionByCodeAsync(string code) =>
            await this.context.Competitions.FirstOrDefaultAsync(c => c.Code == code || c.Code == code.ToUpperInvariant());
    }
}
