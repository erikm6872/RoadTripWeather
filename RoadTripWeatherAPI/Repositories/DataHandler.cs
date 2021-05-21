using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using GoogleMapsApi.Entities.Directions.Response;
using Newtonsoft.Json;
using GoogleMapsApi.Entities.Directions.Request;
using GoogleMapsApi;
using RoadTripWeatherAPI.Models.Forecast;
using RoadTripWeatherAPI.Models.WeatherRoute;
using RoadTripWeatherAPI.Helpers;
using GoogleMapsApi.Entities.Common;
using RoadTripWeatherAPI.Exceptions;

namespace RoadTripWeatherAPI.Repositories
{
    public class DataHandler
    {
        private IConfiguration _config;

        public DataHandler(IConfiguration config)
        {
            _config = config;
        }

        public async Task<IEnumerable<HourlyWeather>> GetHourlyWeatherForRoute(string origin, string destination)
        {
            var dirs = await GetDirections(origin, destination);

            // If no routes are returned, there's been an error with Google Maps
            if (dirs.Routes == null || !dirs.Routes.Any())
                throw new NoRoutesException();

            // Calculate total number of hours in route
            int numHours = 0;
            foreach(var r in dirs.Routes)
            {
                foreach(var l in r.Legs)
                {
                    numHours += (l.Duration.Value.Days * 24) + l.Duration.Value.Hours;
                }
            }

            if (numHours > 48)
                throw new SourceAPILimitationException("OpenWeatherMap API only supports hourly forecasts 48 hours in advance.");

            // Build full list of polyline points
            List<Location> polyline = new List<Location>();
            foreach(var route in dirs.Routes)
            {
                foreach(var leg in route.Legs)
                {
                    foreach(var step in leg.Steps)
                    {
                        foreach(var point in step.PolyLine.Points)
                        {
                            polyline.Add(point);
                        }
                    }
                }
            }

            // Each entry in LocationIndices will be roughly 1 hour apart
            var locationIndices = polyline.Count / numHours;

            var hourlyWeather = new List<HourlyWeather>();
            for (int i = 0; i < numHours; i++)
            {
                int locationIndex = i * locationIndices;
                var forecasts = await GetWeatherByCoords(polyline[locationIndex].Latitude, polyline[locationIndex].Longitude);
                hourlyWeather.Add(new HourlyWeather()
                {
                    Location = polyline[locationIndex],
                    Forecast = forecasts.Hourly[i],
                    Alerts = forecasts.Alerts,
                    ForecastTimestamp = UnixTimeStampToDateTime(forecasts.Hourly[i].Dt)
                });
            }
            var polylineStr = PolylineConverter.Encode(hourlyWeather.Select(x => x.Location));  
            return hourlyWeather;
        }

        public async Task<DirectionsResponse> GetDirections(string origin, string destination)
        {
            var key = ValidateAPIKey(_config.GetSection("ApiKeys").GetSection("Google"));

            var directions_input = new DirectionsRequest()
            {
                Origin = origin,
                Destination = destination,
                ApiKey = key
            };

            var directions = await GoogleMaps.Directions.QueryAsync(directions_input);

            return directions;
        }

        public async Task<WeatherResponse> GetWeatherByCoords(double lat, double lon)
        {
            string baseUrl = _config.GetSection("ApiEndpoints").GetSection("OpenWeatherMaps").Value;
            string owmKey = ValidateAPIKey(_config.GetSection("ApiKeys").GetSection("OpenWeatherMaps"));

            string url = baseUrl + $"?lat={lat}&lon={lon}&exclude=current,daily,minutely&units=imperial&appid={owmKey}";

            var client = new HttpClient();
            var response = await client.GetStringAsync(url);

            var ret = JsonConvert.DeserializeObject<WeatherResponse>(response);
            return ret;
        }

        private static DateTime UnixTimeStampToDateTime(double unixTimeStamp)
        {
            // Unix timestamp is seconds past epoch
            DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddSeconds(unixTimeStamp).ToLocalTime();
            return dtDateTime;
        }

        private static string ValidateAPIKey(IConfigurationSection section)
        {
            if (section.Value == "{API Key}")
                throw new InvalidAPIKeyException($"{section.Path} is not defined in appsettings.json");
            else
                return section.Value;
        }
    }
}
