using Newtonsoft.Json;
using System;
using System.Net;

namespace ConsoleApp1
{
    class WeatherService : IWeatherService
    {
        public void GetData()
        {
            Location location = new Location();
            WeatherData weatherData = new WeatherData();
            location.GetLocationProperty(out double latitude, out double longitude);

            var url = $"https://api.openweathermap.org/data/2.5/weather?lat={latitude}&lon={longitude}&units=metric&appid=134f60d139e7bb125d5068fdd4c7ed95";

            using (WebClient client = new WebClient())
            {
                try
                {
                    weatherData = JsonConvert.DeserializeObject<WeatherData>(client.DownloadString(url));
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Invalid coordinates");
                    Console.WriteLine(ex);
                    Console.ReadLine();
                }
            }
            WriteData(weatherData);
        }

        public void GetData(string cityName, string country = "ro")
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
                var url = $"https://api.openweathermap.org/data/2.5/weather?q={cityName},{country}&units=metric&appid=134f60d139e7bb125d5068fdd4c7ed95";


                using (WebClient client = new WebClient())
                {
                    try
                    {
                        weatherData = JsonConvert.DeserializeObject<WeatherData>(client.DownloadString(url));
                        incorrectCityName = false;
                    }
                    catch (WebException ex)
                    {
                        if (ex.Message.Contains("403"))
                        {
                            Console.WriteLine(ex);
                            Console.WriteLine("Forbidden access");
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

        private void WriteData(WeatherData weatherData)
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
}
