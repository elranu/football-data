using System;
using System.Collections.Generic;
using System.Text;

namespace FootballData.Core.Models
{
    public class Team
    {
        public int Id { get; set; }
        /// <summary>
        /// Is the Id on Football-data
        /// </summary>
        public int IdService { get; set; }
        public string Name { get; set; }
        public string ShortName { get; set; }
        public string AreaName { get; set; }
        public string Email { get; set; }
        public ICollection<CompetitionTeam> Competitions { get; set; }
        public ICollection<Player> Squad { get; set; }
    }
}
