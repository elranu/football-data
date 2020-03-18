using FootballData.Core.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace FootballData.Data.Configurations
{
    public class TeamConfiguration : IEntityTypeConfiguration<Team>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Team> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).UseIdentityColumn();
           
            builder.HasIndex(x => x.IdService);
            builder.HasMany(t => t.Squad)
                .WithOne(p => p.Team).IsRequired();

            builder.Property(c => c.AreaName).HasColumnType("varchar(50)").IsRequired();
            builder.Property(c => c.Name).HasColumnType("varchar(100)").IsRequired();
            builder.Property(c => c.Email).HasColumnType("varchar(200)");
            builder.Property(c => c.ShortName).HasColumnType("varchar(100)");
            builder.ToTable("Teams");
        }
    }
}
