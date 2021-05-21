using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RoadTripWeatherAPI.Models.Forecast
{
    public class Alert
    {
        public string SenderName { get; set; }
        public string Event { get; set; }
        public int Start { get; set; }
        public int End { get; set; }
        public string Description { get; set; }
        public List<string> Tags { get; set; }
    }
}
