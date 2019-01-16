using System;
using System.Text;
using System.Threading;

namespace ConsoleApp1
{
    partial class Program
    {
        static IWeatherService weatherService = new WeatherService();

        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            string continueDecision = "y";

            while (continueDecision.Equals("y"))
            {
                var serviceType = ServiceType.CurrentWeather;
                Console.WriteLine("Do you need current weather or forecast for 5 days?(default current)\n\n1. Current Weather\n2. Forecast");
                weatherService.AnimateLoading();
                var weatherDecision = Console.ReadLine().ToLower();
                if (weatherDecision.Equals("2"))
                    serviceType = ServiceType.Forecast;

                Console.WriteLine("Do you want to enter city name? Y/N");
                var cityDecision = Console.ReadLine().ToLower();
                if (cityDecision == "exit".ToLower())
                    Environment.Exit(0);
                if (cityDecision.Equals("y"))
                {
                    Console.WriteLine(@"Please enter city and country(e.g. B, ro) to get weather data:");
                    var cityAndCountry = Console.ReadLine().Split(',');
                    var city = cityAndCountry[0];
                    if (city.ToLower().Equals("exit"))
                        Environment.Exit(0);
                    var country = cityAndCountry.Length > 1 ? cityAndCountry[1] : string.Empty;

                    if (!string.IsNullOrEmpty(country))
                    {
                        Console.WriteLine($"Getting data for city {city}:");
                        weatherService.AnimateLoading();
                        weatherService.GetData(city, serviceType, country);
                    }
                    else
                    {
                        weatherService.AnimateLoading();
                        weatherService.GetData(city, serviceType);
                    }
                }
                else if (cityDecision.Equals("n"))
                {
                    Console.WriteLine("Getting information from device location");
                    weatherService.AnimateLoading();
                    weatherService.GetData(serviceType);
                }
                weatherService.AnimateLoading();
                Console.WriteLine("Do you want to search another city? Y/N");
                continueDecision = Console.ReadLine().ToLower();
                if (continueDecision.ToLower().Equals("exit"))
                    Environment.Exit(0);
            }
        }
    }
}
