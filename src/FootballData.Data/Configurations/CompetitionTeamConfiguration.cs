using FootballData.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace FootballData.Data.Configurations
{
    public class CompetitionTeamConfiguration : IEntityTypeConfiguration<CompetitionTeam>
    {
        public void Configure(EntityTypeBuilder<CompetitionTeam> builder)
        {
            builder.HasKey(ct => new { ct.IdCompetition, ct.IdTeam });
            builder.HasOne(ct => ct.Team)
                .WithMany(t => t.Competitions)
                .HasForeignKey(ct => ct.IdTeam);

            builder.HasOne(ct => ct.Competition)
                .WithMany(c => c.Teams)
                .HasForeignKey(ct => ct.IdCompetition);

            builder.ToTable("CompetitionTeams");
        }
    }
}
