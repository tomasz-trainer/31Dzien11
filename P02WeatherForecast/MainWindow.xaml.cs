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

        // scenariusz 2: wywołanie metody asynchronicznej w pętli foreach: czekamy az wszystkie zadania się wykonają i dopiero wtedy wyświetlamy wyniki
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

        // scenariusz 3: wywołanie metody asynchronicznej w pętli foreach: czekamy az wszystkie zadania się wykonają i dopiero wtedy wyświetlamy wyniki
        // dodatkowo : w tasku zwracany kilka wartości (temp, city) w postaci krotki (tuple)
        private async void btnGetTemperatureAsync3_Click(object sender, RoutedEventArgs e)
        {
            tbTemperature.Text = string.Empty;
            lvLogger.Items.Clear();

            WeatherForecastService wfs = new WeatherForecastService();
            string[] cities = { "Warsaw", "London", "New York", "Tokyo", "Sydney" };
            //string[] cities =  txtCity.Text.Split(Environment.NewLine);

            List<Task> tasks = new List<Task>();
            foreach (string city in cities)
            {


                var t = Task.Run(() => // to co jest w ciele metody GetTemperature() jest wykonywane w osobnym wątku
                {

                    double temp = wfs.GetTemperature(city);
                    return (temp,city);
                });
                tasks.Add(t);
            }


            lvLogger.Items.Add($"Started processng all cities");
            await Task.WhenAll(tasks);
            lvLogger.Items.Add($"Finished processng all cities");

            foreach (Task<(double Temperature, string City)> task in tasks)
            {
                double temp = task.Result.Temperature;
                string city = task.Result.City;
                tbTemperature.Text += $"Temperature in {city} is currently :  {temp} °C\n";
            }
            ;
        }

        // Scenariusz 4: wywołanie metody asynchronicznej w pętli foreach: czekamy az wszystkie zadania się wykonają i dopiero wtedy wyświetlamy wyniki
        // wyniki są zwracane do wątku UI
        private async void btnGetTemperatureAsync4_Click(object sender, RoutedEventArgs e)
        {
            tbTemperature.Text = string.Empty;
            lvLogger.Items.Clear();

            WeatherForecastService wfs = new WeatherForecastService();
            string[] cities = { "Warsaw", "London", "New York", "Tokyo", "Sydney" };
            //string[] cities =  txtCity.Text.Split(Environment.NewLine);

            foreach (var city in cities)
            {
                var t = Task.Run(() => 
                { 
                    double temp = wfs.GetTemperature(city);
                    return (temp, city);
                });

                t.GetAwaiter().OnCompleted(() =>
                {// tutaj definiuje kod, który wykona się po zakończeniu zadania t
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        tbTemperature.Text += $"Temperature in {city} is currently :  {t.Result.temp} °C\n";
                    });
                });
            }
        }

        // scenariusz 5: podobny do nr 1 ale dodany progress bar 
        private async void btnGetTemperatureAsync5_Click(object sender, RoutedEventArgs e)
        {
            tbTemperature.Text = string.Empty;
            lvLogger.Items.Clear();
            WeatherForecastService wfs = new WeatherForecastService();
            string[] cities = { "Warsaw", "London", "New York", "Tokyo", "Sydney" };

            pbProgressbar.Maximum = cities.Length;
            pbProgressbar.Value = 0;

            foreach (var city in cities)
            {
                lvLogger.Items.Add($"Currently processing: {city}");
                await Task.Run(() =>
                {
                    double temp = wfs.GetTemperature(city);
                    
                    return temp;
                });
                pbProgressbar.Value += 1;

                tbTemperature.Text += $"Temperature in {city} is currently :  {wfs.GetTemperature(city)} °C\n";
            }
        }

        private async void btnGetTemperatureAsync6_Click(object sender, RoutedEventArgs e)
        {
            tbTemperature.Text = string.Empty;
            lvLogger.Items.Clear();
            WeatherForecastService wfs = new WeatherForecastService();
            string[] cities = { "Warsaw", "London", "New York", "Tokyo", "Sydney" };


            pbProgressbar.Maximum = cities.Length;
            pbProgressbar.Value = 0;

            foreach (var city in cities)
            {
                lvLogger.Items.Add("Currently processing: " + city);
                double temp = await wfs.GetTemperatureAsync(city);
                pbProgressbar.Value += 1;
                tbTemperature.Text += $"Temperature in {city} is currently :  {temp} °C\n";
            }


        }
    }
}