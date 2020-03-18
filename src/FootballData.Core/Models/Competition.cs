using System;
using System.Collections.Generic;
using System.Text;

namespace FootballData.Core.Models
{
    public class Competition
    {
        public int Id { get; set; }
        /// <summary>
        /// Is the Id on Football-data
        /// </summary>
        public int IdService { get; set; }
        public string Name { get; set; }
        public string AreaName { get; set; }
        public string Code { get; set; }
        public ICollection<CompetitionTeam> Teams { get; set; }

    }
}
