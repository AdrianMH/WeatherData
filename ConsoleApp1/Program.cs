using System;
using System.Text;
using System.Threading;

namespace ConsoleApp1
{
    partial class Program
    {
        static string continueDecision = "y";

        static void Main(string[] args)
        {
            IWeatherService weatherService = new WeatherService();
            Console.OutputEncoding = Encoding.UTF8;

            while (continueDecision.Equals("y"))
            {
                Console.WriteLine("Do you want to enter city name? Y/N");
                var cityDecision = Console.ReadLine().ToLower();
                if (cityDecision.Equals("y"))
                {
                    Console.WriteLine(@"Please enter city and country(e.g. B, ro) to get weather data:");
                    var cityAndCountry = Console.ReadLine().Split(',');
                    var city = cityAndCountry[0];
                    var country = cityAndCountry.Length > 1 ? cityAndCountry[1] : string.Empty;
                    if (!string.IsNullOrEmpty(country))
                    {
                        Console.WriteLine($"Getting data for city {city}:");
                        AnimateLoading();
                        weatherService.GetData(city, country);
                    }
                    else
                    {
                        AnimateLoading();
                        weatherService.GetData(city);
                    }
                }
                else if (cityDecision.Equals("n"))
                {
                    Console.WriteLine("Getting information from device location");
                    AnimateLoading();
                    weatherService.GetData();
                }
                AnimateLoading();
                Console.WriteLine("Do you want to search another city? Y/N");
                continueDecision = Console.ReadLine().ToLower();
            }
        }

        static void AnimateLoading()
        {
            for (int i = 0; i < 50; i++)
            {
                Thread.Sleep(10);
                Console.Write("-");
            }
            Console.WriteLine();
        }
    }
}
