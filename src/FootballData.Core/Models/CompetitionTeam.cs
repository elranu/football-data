using System;
using System.Collections.Generic;
using System.Text;

namespace FootballData.Core.Models
{
    public class CompetitionTeam
    {
        public int IdCompetition { get; set; }
        public Competition Competition { get; set; }
        public int IdTeam { get; set; }
        public Team Team { get; set; }
    }
}
