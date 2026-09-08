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
                lvLogger.Items.Add($"Getting temperature for {city}...");

                var t = await Task.Run<double>(() => // to co jest w ciele metody GetTemperature() jest wykonywane w osobnym wątku
                {
                   
                    double temp = wfs.GetTemperature(city);
                    return temp;
                });

                tbTemperature.Text += $"Temperature in {city}: {t} °C\n";
            }
        }

        // scenariusz 1: wywołanie metody asynchronicznej w pętli foreach: czekamy az wszystkie zadania się wykonają i dopiero wtedy wyświetlamy wyniki
        private async void btnGetTemperatureAsync2_Click(object sender, RoutedEventArgs e)
        {
            tbTemperature.Text = string.Empty;
            lvLogger.Items.Clear();

            WeatherForecastService wfs = new WeatherForecastService();
            string[] cities = { "Warsaw", "London", "New York", "Tokyo", "Sydney" };
            //string[] cities =  txtCity.Text.Split(Environment.NewLine);
            
            List<Task<double>> tasks = new List<Task<double>>();
            foreach (string city in cities)
            {
             

                var t = Task.Run<double>(() => // to co jest w ciele metody GetTemperature() jest wykonywane w osobnym wątku
                {

                    double temp = wfs.GetTemperature(city);
                    return temp;
                });
                tasks.Add(t);

               
            }


            lvLogger.Items.Add($"Started processng all cities");
            await Task.WhenAll(tasks);
            lvLogger.Items.Add($"Finished processng all cities");

            foreach (var task in tasks)
            {
                double temp = task.Result;
                tbTemperature.Text += $"Temperature: {temp} °C\n";
            };
        }
    }
}