namespace P04WeatherForecastConsole.Client
{
    internal class Program
    {
        static async Task Main(string[] args)
        {

            OpenMeteoService oms = new OpenMeteoService();
            var cities =  await  oms.GetLocationsAsync("warsz");

            var weather= await oms.GetCurrentConditionsAsync(cities[0].Latitude, cities[0].Longitude);
        }
    }
}
