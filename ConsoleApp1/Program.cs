using Newtonsoft.Json;
using System;
using System.Device.Location;
using System.Net;
using System.Text;
using System.Threading;

namespace ConsoleApp1
{
    class Program
    {
        static string continueDecision = "y";
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            Console.WriteLine("Do you want to enter city name? Y/N");
            var decision = Console.ReadLine().ToLower();
            while (continueDecision.Equals("y"))
            {
                if (decision.Equals("y"))
                {
                    Console.WriteLine("Please enter city to get weather data:");
                    var cityName = Console.ReadLine();
                    Console.WriteLine($"Getting data for city {cityName}:");
                    Console.WriteLine("--------------------------------------------------");
                    GetDataByCityName(cityName);
                }
                else if (decision.Equals("n"))
                {
                    Console.WriteLine("Getting information from device location");
                    Console.WriteLine("--------------------------------------------------");
                    GetDataByLocation();
                }
                Console.WriteLine("--------------------------------------------------");
                Console.WriteLine("Do you want to search another city? Y/N");
                continueDecision = Console.ReadLine().ToLower();
            }
        }

        private static void GetDataByCityName(string cityName)
        {

            bool incorrectCityName = true;
            int count = 0;
            WeatherData weatherData = new WeatherData();
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
                        weatherData = JsonConvert.DeserializeObject<WeatherData>(json);
                        incorrectCityName = false;
                    }
                    catch (WebException ex)
                    {
                        if (ex.Message.Contains("403"))
                        {
                            Console.WriteLine(ex);
                            Console.WriteLine("Forbidden access");
                            continueDecision = "n";
                        }
                        else
                        {
                            count = 1;
                            incorrectCityName = true;
                            Console.WriteLine("City name does not exist");
                            Console.WriteLine("Enter correct city name:");
                        }
                    }
                }
            }

            WriteData(weatherData);
        }

        private static void GetDataByLocation()
        {
            Location location = new Location();
            WeatherData weatherData = new WeatherData();
            double latitude;
            double longitude;
            location.GetLocationProperty(out latitude, out longitude);

            var url = $"https://api.openweathermap.org/data/2.5/weather?lat={latitude}&lon={longitude}&units=metric&appid=134f60d139e7bb125d5068fdd4c7ed95";

            using (WebClient client = new WebClient())
            {
                try
                {
                    var json = client.DownloadString(url);
                    weatherData = JsonConvert.DeserializeObject<WeatherData>(json);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Invalid coordinates");
                    Console.WriteLine(ex);
                    Console.ReadLine();
                    return;
                }
            }

            WriteData(weatherData);
            Console.ReadLine();
        }

        private static void WriteData(WeatherData weatherData)
        {
            var sunriseTime = DateTimeOffset.FromUnixTimeSeconds(weatherData.Sys.Sunrise).LocalDateTime.ToShortTimeString();
            var sunsetTime = DateTimeOffset.FromUnixTimeSeconds(weatherData.Sys.Sunset).LocalDateTime.ToShortTimeString();
            Console.WriteLine($"City Name: {weatherData.Name}\n" +
                            $"Country: {weatherData.Sys.Country}\n" +
                            $"Current Temperature: {weatherData.Main.Temp}°C\n" +
                            $"Wind Speed: {weatherData.Wind.Speed}km/h\n" +
                            $"Wind Direction: {weatherData.Wind.Deg}°\n" +
                            $"Temp min: {weatherData.Main.TempMin}°C\n" +
                            $"Temp max: {weatherData.Main.TempMax}°C\n" +
                            $"Sunrise: {sunriseTime}\n" +
                            $"Sunset: {sunsetTime}");
        }
    }

    class Location
    {
        public void GetLocationProperty(out double latitude, out double longitude)
        {
            GeoCoordinateWatcher watcher = new GeoCoordinateWatcher();
            watcher.Start();
            if (watcher.Position.Location.IsUnknown)
            {
                Thread.Sleep(200);
            }
            GeoCoordinate coord = watcher.Position.Location;
            latitude = coord.Latitude;
            longitude = coord.Longitude;
        }
    }
}
