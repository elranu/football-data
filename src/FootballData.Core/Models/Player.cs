﻿using System;
using System.Collections.Generic;
using System.Text;

namespace FootballData.Core.Models
{
    public class Player
    {
        public int Id { get; set; }
        /// <summary>
        /// Is the Id on Football-data
        /// </summary>
        public int IdService { get; set; }
        public string Name { get; set; }
        public string Position { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string CountryOfBirth { get; set; }
        public string Nationality { get; set; }
        public Team Team { get; set; }
        public int IdTeam { get; set; }
    }
}
