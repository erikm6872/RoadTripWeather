using GoogleMapsApi.Entities.Common;
using RoadTripWeatherAPI.Models.Forecast;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RoadTripWeatherAPI.Models.WeatherRoute
{
    public class HourlyWeather
    {
        public Location Location { get; set; }
        public Hourly Forecast { get; set; }
        public List<Alert> Alerts { get; set; }
        public DateTime ForecastTimestamp { get; set; }
    }
}
