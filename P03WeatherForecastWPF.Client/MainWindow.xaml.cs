using P03WeatherForecastWPF.Client.Models;
using P04WeatherForecastConsole.Client;
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

namespace P03WeatherForecastWPF.Client
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        OpenMeteoService openMeteoService;
        public MainWindow()
        {
            InitializeComponent();
            openMeteoService = new OpenMeteoService();
        }

        private async void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            City[] cities = await openMeteoService.GetLocationsAsync(txtCity.Text);
            lbData.ItemsSource = cities;
        }

        private async void lbData_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedCity = lbData.SelectedItem as City;
            if(selectedCity != null)
            {
                var weather = await openMeteoService.GetCurrentConditionsAsync(selectedCity.Latitude, selectedCity.Longitude);

                lblCityName.Content = selectedCity.FullName;
                lblTemperatureValue.Content = $"{weather.Temperature2m} °C";
            }

        }
    }
}