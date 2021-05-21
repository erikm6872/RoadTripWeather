using GoogleMapsApi.Entities.Directions.Response;
using RoadTripWeatherAPI.Models.Forecast;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RoadTripWeatherAPI.Models.WeatherRoute
{
    public class WeatherStep
    {
        public Step Step { get; set; }
        public WeatherResponse WeatherForStep { get; set; }
    }
}
