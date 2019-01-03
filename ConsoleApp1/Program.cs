using Newtonsoft.Json;
using System;
using System.Net;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Please enter your city to get weather data");
            var cityName = Console.ReadLine();
            Console.WriteLine($"Getting data for city {cityName}");
            Console.WriteLine("--------------------------------------------------");
            GetData(cityName);
        }

        private static void GetData(string cityName)
        {
            string jsonContent = string.Empty;
            bool incorrectCityName = true;
            int count = 0;
            while (incorrectCityName)
            {
                if (count != 0)
                {
                    cityName = Console.ReadLine();
                }
                var url = $"https://api.openweathermap.org/data/2.5/weather?q={cityName},ro&units=metric&appid=134f60d139e7bb125d5068fdd4c7ed95";


                using (WebClient client = new WebClient())
                {
                    try
                    {
                        var json = client.DownloadString(url);
                        jsonContent = json;
                        incorrectCityName = false;
                    }
                    catch
                    {
                        count = 1;
                        incorrectCityName = true;
                        Console.WriteLine("City name does not exist");
                    }
                }
            }

            WeatherData weather = new WeatherData();
            WeatherData contents = JsonConvert.DeserializeObject<WeatherData>(jsonContent);
            Console.WriteLine($"City Name: {contents.Name}\n" +
                $"Country: {contents.Sys.Country}\n" +
                $"Current Temperature: {contents.Main.Temp}\n" +
                $"Temp min: {contents.Main.TempMin}\n" +
                $"Temp max: {contents.Main.TempMax}\n" +
                $"Wind Speed: {contents.Wind.Speed}km/h\n" +
                $"Wind Direction: {contents.Wind.Deg}°");
            Console.ReadLine();
        }
    }
}
