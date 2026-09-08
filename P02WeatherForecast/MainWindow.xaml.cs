using P02WeatherForecast.Services;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace P02WeatherForecast
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void btnGetTemperature_Click(object sender, RoutedEventArgs e)
        {
            WeatherForecastService wfs = new WeatherForecastService();
             string[] cities = { "Warsaw", "London", "New York", "Tokyo", "Sydney" };
            //string[] cities =  txtCity.Text.Split(Environment.NewLine);

            foreach (string city in cities) 
            {
                double temp = wfs.GetTemperature(city);
                tbTemperature.Text += $"Temperature in {city}: {temp} °C\n";
            }
        }

        // scenariusz 1: wywołanie metody asynchronicznej w pętli foreach: miasto jedno po drugim 
        private async void btnGetTemperatureAsync1_Click(object sender, RoutedEventArgs e)
        {
            WeatherForecastService wfs = new WeatherForecastService();
            string[] cities = { "Warsaw", "London", "New York", "Tokyo", "Sydney" };
            //string[] cities =  txtCity.Text.Split(Environment.NewLine);
            lvLogger.Items.Clear();

            foreach (string city in cities)
            {

                var t = Task.Run<double>(() => // to co jest w ciele metody GetTemperature() jest wykonywane w osobnym wątku
                {
                    lvLogger.Items.Add($"Getting temperature for {city}...");
                    double temp = wfs.GetTemperature(city);
                    return temp;
                });

                tbTemperature.Text += $"Temperature in {city}: {t} °C\n";
            }
        }
    }
}