﻿// <auto-generated />
using System;
using FootballData.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace FootballData.Data.Migrations
{
    [DbContext(typeof(FootballDbContext))]
    partial class FootballDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("FootballData.Core.Models.Competition", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("AreaName")
                        .IsRequired()
                        .HasColumnType("varchar(50)");

                    b.Property<string>("Code")
                        .HasColumnType("varchar(5)");

                    b.Property<int>("IdService")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("Id");

                    b.HasIndex("IdService");

                    b.ToTable("Competitions");
                });

            modelBuilder.Entity("FootballData.Core.Models.CompetitionTeam", b =>
                {
                    b.Property<int>("IdCompetition")
                        .HasColumnType("int");

                    b.Property<int>("IdTeam")
                        .HasColumnType("int");

                    b.HasKey("IdCompetition", "IdTeam");

                    b.HasIndex("IdTeam");

                    b.ToTable("CompetitionTeams");
                });

            modelBuilder.Entity("FootballData.Core.Models.Player", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("CountryOfBirth")
                        .IsRequired()
                        .HasColumnType("varchar(50)");

                    b.Property<DateTime?>("DateOfBirth")
                        .HasColumnType("smalldatetime");

                    b.Property<int>("IdService")
                        .HasColumnType("int");

                    b.Property<int>("IdTeam")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(200)");

                    b.Property<string>("Nationality")
                        .IsRequired()
                        .HasColumnType("varchar(50)");

                    b.Property<string>("Position")
                        .IsRequired()
                        .HasColumnType("varchar(50)");

                    b.Property<int>("TeamId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("IdService");

                    b.HasIndex("TeamId");

                    b.ToTable("Players");
                });

            modelBuilder.Entity("FootballData.Core.Models.Team", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("AreaName")
                        .IsRequired()
                        .HasColumnType("varchar(50)");

                    b.Property<string>("Email")
                        .HasColumnType("varchar(200)");

                    b.Property<int>("IdService")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("varchar(100)");

                    b.Property<string>("ShortName")
                        .HasColumnType("varchar(100)");

                    b.HasKey("Id");

                    b.HasIndex("IdService");

                    b.ToTable("Teams");
                });

            modelBuilder.Entity("FootballData.Core.Models.CompetitionTeam", b =>
                {
                    b.HasOne("FootballData.Core.Models.Competition", "Competition")
                        .WithMany("Teams")
                        .HasForeignKey("IdCompetition")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("FootballData.Core.Models.Team", "Team")
                        .WithMany("Competitions")
                        .HasForeignKey("IdTeam")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("FootballData.Core.Models.Player", b =>
                {
                    b.HasOne("FootballData.Core.Models.Team", "Team")
                        .WithMany("Squad")
                        .HasForeignKey("TeamId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
