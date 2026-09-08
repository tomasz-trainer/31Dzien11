using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace P06ZadaniePogoda
{
    internal class ManagerPogody
    {
        public double PodajTemperature(string miasto, string jednostka)
        {
            string url = $"https://wttr.in/{miasto}?format=%t";

            using (WebClient client = new WebClient())
            {
               // client.Encoding = Encoding.UTF8;
                string odpowiedz = client.DownloadString(url);
                string liczba = odpowiedz.Replace("+", "").Replace("°C", "").Trim();
                double tempCelsjusz = Convert.ToDouble(liczba);
                
                return transformujTemperature(jednostka, Convert.ToDouble(tempCelsjusz));

            }
        }

        private double transformujTemperature(string jednostka, double temp)
        {
            if (jednostka == "C")
                return temp;
            else if (jednostka == "F")
                return (temp * 1.8) + 32;
            else if (jednostka == "K")
                return temp + 273.15;
            else
                throw new ArgumentException("Nieznana jednostka temperatury");


        }
    }
}
