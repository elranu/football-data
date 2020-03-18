using FootballData.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace FootballData.Data.Configurations
{
    public class CompetitionConfiguration : IEntityTypeConfiguration<Competition>
    {
        public void Configure(EntityTypeBuilder<Competition> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).UseIdentityColumn();
            builder.HasIndex(x => x.IdService);
            builder.Property(c => c.AreaName).HasColumnType("varchar(50)").IsRequired();
            builder.Property(c => c.Name).HasColumnType("nvarchar(50)").IsRequired();
            builder.Property(c => c.Code).HasColumnType("varchar(5)");


            builder.ToTable("Competitions");
        }
    }
}
