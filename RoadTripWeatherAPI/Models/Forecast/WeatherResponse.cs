using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RoadTripWeatherAPI.Models.Forecast
{
    public class WeatherResponse
    {
        public double Lat { get; set; }
        public double Lon { get; set; }
        public string Timezone { get; set; }
        public int TimezoneOffset { get; set; }
        public List<Hourly> Hourly { get; set; }
        public List<Alert> Alerts { get; set; }
    }
}
