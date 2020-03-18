using FootballData.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace FootballData.Data.Configurations
{
    public class PlayerConfiguration : IEntityTypeConfiguration<Player>
    {
        public void Configure(EntityTypeBuilder<Player> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).UseIdentityColumn();
            builder.HasIndex(x => x.IdService);
            builder.HasOne(p => p.Team)
                .WithMany(t => t.Squad).IsRequired();

            builder.Property(c => c.Name).HasColumnType("nvarchar(200)").IsRequired();
            builder.Property(c => c.Nationality).HasColumnType("varchar(50)").IsRequired();
            builder.Property(c => c.Position).HasColumnType("varchar(50)").IsRequired();
            builder.Property(c => c.DateOfBirth).HasColumnType("smalldatetime");
            builder.Property(c => c.CountryOfBirth).HasColumnType("varchar(50)").IsRequired();

            builder.ToTable("Players");
        }
    }
}
