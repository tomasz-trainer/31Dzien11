using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace P04WeatherForecastConsole.Client
{
 

    public class GeocodingResponse
    {
        [JsonProperty("results")]
        public List<City> Results { get; set; }

        [JsonProperty("generationtime_ms")]
        public double GenerationtimeMs { get; set; }
    }

    public class City
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("latitude")]
        public double Latitude { get; set; }

        [JsonProperty("longitude")]
        public double Longitude { get; set; }

        [JsonProperty("elevation")]
        public double Elevation { get; set; }

        [JsonProperty("feature_code")]
        public string FeatureCode { get; set; }

        [JsonProperty("country_code")]
        public string CountryCode { get; set; }

        [JsonProperty("admin1_id")]
        public int Admin1Id { get; set; }

        [JsonProperty("admin2_id")]
        public int Admin2Id { get; set; }

        [JsonProperty("admin3_id")]
        public int Admin3Id { get; set; }

        [JsonProperty("timezone")]
        public string Timezone { get; set; }

        [JsonProperty("population")]
        public int Population { get; set; }

        [JsonProperty("country_id")]
        public int CountryId { get; set; }

        [JsonProperty("country")]
        public string Country { get; set; }

        [JsonProperty("admin1")]
        public string Admin1 { get; set; }

        [JsonProperty("admin2")]
        public string Admin2 { get; set; }

        [JsonProperty("admin3")]
        public string Admin3 { get; set; }
    }


    public class OpenMeteoForecastResponse
    {
        [JsonProperty("latitude")]
        public double Latitude { get; set; }

        [JsonProperty("longitude")]
        public double Longitude { get; set; }

        [JsonProperty("generationtime_ms")]
        public double GenerationtimeMs { get; set; }

        [JsonProperty("utc_offset_seconds")]
        public int UtcOffsetSeconds { get; set; }

        [JsonProperty("timezone")]
        public string Timezone { get; set; }

        [JsonProperty("timezone_abbreviation")]
        public string TimezoneAbbreviation { get; set; }

        [JsonProperty("elevation")]
        public double Elevation { get; set; }

        [JsonProperty("current_units")]
        public CurrentUnits CurrentUnits { get; set; }

        [JsonProperty("current")]
        public Current Current { get; set; }
    }

    public class CurrentUnits
    {
        [JsonProperty("time")]
        public string Time { get; set; }

        [JsonProperty("interval")]
        public string Interval { get; set; }

        [JsonProperty("temperature_2m")]
        public string Temperature2m { get; set; }
    }

    public class Current
    {
        [JsonProperty("time")]
        public string Time { get; set; }

        [JsonProperty("interval")]
        public int Interval { get; set; }

        [JsonProperty("temperature_2m")]
        public double Temperature2m { get; set; }
    }

    internal class Weather
    {
        public string Time { get; set; }
        public double Temperature2m { get; set; }
    }

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
