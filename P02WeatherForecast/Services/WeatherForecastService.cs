using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace P02WeatherForecast.Services
{
    internal class WeatherForecastService
    {
        public double GetTemperature(string city)
        {
            string url = $"https://wttr.in/{city}?format=%t";

            using (WebClient client = new WebClient())
            {
                // client.Encoding = Encoding.UTF8;
                string odpowiedz = client.DownloadString(url);
                string liczba = odpowiedz.Replace("+", "").Replace("°C", "").Trim();
                double tempCelsjusz = Convert.ToDouble(liczba);

                return transformujTemperature("C", Convert.ToDouble(tempCelsjusz));

            }
        }

        private double transformujTemperature(string jednostka, double temp)
        {
          //    Thread.Sleep(1000); // symulacja opóźnienia 1 sekundy

            if (jednostka == "C")
                return temp;
            else if (jednostka == "F")
                return (temp * 1.8) + 32;
            else if (jednostka == "K")
                return temp + 273.15;
            else
                throw new ArgumentException("Nieznana jednostka temperatury");
        }

        public async Task<double> GetTemperatureAsync(string city)
        {
            string url = $"https://wttr.in/{city}?format=%t";

            using (WebClient client = new WebClient())
            {
                // client.Encoding = Encoding.UTF8;
                string data = await client.DownloadStringTaskAsync(url);
                string res = data.Replace("+", "").Replace("°C", "").Trim();
                return Convert.ToDouble(res);
                
            }
        }

        public async Task<double> GetTemperatureAsync2(string city)
        {

            return await Task.Run(() =>
            {

                string url = $"https://wttr.in/{city}?format=%t";

                using (WebClient client = new WebClient())
                {
                    // client.Encoding = Encoding.UTF8;
                    string data = client.DownloadString(url);
                    string res = data.Replace("+", "").Replace("°C", "").Trim();
                    return Convert.ToDouble(res);

                }


            });


           
        }



    }
}
