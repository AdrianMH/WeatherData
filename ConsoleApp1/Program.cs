using Newtonsoft.Json;
using System;
using System.Device.Location;
using System.Net;
using System.Threading;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Do you want to enter city name?");
            var decision = Console.ReadLine();
            if (decision.Equals("Y".ToLower()))
            {
                Console.WriteLine("Please enter your city to get weather data");
                var cityName = Console.ReadLine();
                Console.WriteLine($"Getting data for city {cityName}");
                Console.WriteLine("--------------------------------------------------");
                GetDataByCityName(cityName);
            }
            else if (decision.Equals("N".ToLower()))
            {
                Console.WriteLine("Getting information from device location");
                Console.WriteLine("--------------------------------------------------");
                GetDataByLocation();
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
                    catch
                    {
                        count = 1;
                        incorrectCityName = true;
                        Console.WriteLine("City name does not exist");
                    }
                }
            }

            WriteData(weatherData);
            Console.ReadLine();
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
            Console.WriteLine($"City Name: {weatherData.Name}\n" +
                            $"Country: {weatherData.Sys.Country}\n" +
                            $"Current Temperature: {weatherData.Main.Temp}\n" +
                            $"Wind Speed: {weatherData.Wind.Speed}km/h\n" +
                            $"Wind Direction: {weatherData.Wind.Deg}°\n" +
                            $"Temp min: {weatherData.Main.TempMin}\n" +
                            $"Temp max: {weatherData.Main.TempMax}");
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
