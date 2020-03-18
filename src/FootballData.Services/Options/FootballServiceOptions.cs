using System;
using System.Collections.Generic;
using System.Text;

namespace FootballData.Services.Options
{
    public class FootballServiceOptions
    {

        public string AuthToken { get; set; }
        public string BaseURL { get; set; }
        public int? MaxRequestPerInterval { get; set; }
        public int? IntervalSecs { get; set; }
    }
}
