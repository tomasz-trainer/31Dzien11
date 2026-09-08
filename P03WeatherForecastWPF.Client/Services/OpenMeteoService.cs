using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using P03WeatherForecastWPF.Client.Models;

namespace P04WeatherForecastConsole.Client
{
 

    internal class OpenMeteoService
    {
        private const string geocoding_base_url = "https://geocoding-api.open-meteo.com/v1/search";
        private const string forecast_base_url = "https://api.open-meteo.com/v1/forecast";

        public async Task<City[]> GetLocationsAsync(string locationName)
        {
            string url = $"{geocoding_base_url}?name={locationName}&count=10&language=pl&format=json";

            using (HttpClient client = new HttpClient()) 
            {
               var response = await client.GetAsync(url);
               string json = await response.Content.ReadAsStringAsync();

               var result = JsonConvert.DeserializeObject<GeocodingResponse>(json);

               return result?.Results?.ToArray() ?? Array.Empty<City>();
            }

        }

        public async Task<Weather> GetCurrentConditionsAsync(double latitude, double longitude)
        {
            string lat = latitude.ToString(System.Globalization.CultureInfo.InvariantCulture);
             string lon = longitude.ToString(System.Globalization.CultureInfo.InvariantCulture);

            string url = $"{forecast_base_url}?latitude={lat}&longitude={lon}&current=temperature_2m";


            using (HttpClient client = new HttpClient())
            {
                var response = await client.GetAsync(url);
                var json = await response.Content.ReadAsStringAsync();

                var result = JsonConvert.DeserializeObject<OpenMeteoForecastResponse>(json);
            
                return new Weather
                {
                    Time = result?.Current?.Time,
                    Temperature2m = result?.Current?.Temperature2m ?? 0
                };

            }
     
        }

    }
}
