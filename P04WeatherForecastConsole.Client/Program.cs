namespace P04WeatherForecastConsole.Client
{
    internal class Program
    {
        static async Task Main(string[] args)
        {

            OpenMeteoService oms = new OpenMeteoService();
            var cities =  await  oms.GetLocationsAsync("warsz");
        }
    }
}
