using Newtonsoft.Json;
using System;
using System.Net;
using System.Threading;

namespace ConsoleApp1
{
    class WeatherService : IWeatherService
    {
        public void GetData(ServiceType serviceType)
        {
            Location location = new Location();
            WeatherData weatherData = new WeatherData();
            ForecastData forecast = new ForecastData();
            double latitude, longitude;
            location.GetLocationProperty(out latitude, out longitude);
            string url = string.Empty;
            if (serviceType == ServiceType.CurrentWeather)
                url = $"https://api.openweathermap.org/data/2.5/weather?lat={latitude}&lon={longitude}&units=metric&appid=134f60d139e7bb125d5068fdd4c7ed95";
            else if (serviceType == ServiceType.Forecast)
                url = $"https://api.openweathermap.org/data/2.5/forecast?lat={latitude}&lon={longitude}&units=metric&appid=134f60d139e7bb125d5068fdd4c7ed95";

            using (WebClient client = new WebClient())
            {
                try
                {
                    if (serviceType == ServiceType.CurrentWeather)
                        weatherData = JsonConvert.DeserializeObject<WeatherData>(client.DownloadString(url));
                    if (serviceType == ServiceType.Forecast)
                        forecast = JsonConvert.DeserializeObject<ForecastData>(client.DownloadString(url));
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Invalid coordinates");
                    Console.WriteLine(ex);
                    Console.ReadLine();
                }
            }
            if (serviceType == ServiceType.CurrentWeather)
                WriteData(weatherData);
            else if (serviceType == ServiceType.Forecast)
                WriteData(forecast);
        }

        public void GetData(string cityName, ServiceType serviceType, string country = "ro")
        {
            bool incorrectCityName = true;
            int count = 0;
            WeatherData weatherData = new WeatherData();
            ForecastData forecast = new ForecastData();
            while (incorrectCityName)
            {
                if (count != 0)
                {
                    cityName = Console.ReadLine();
                    AnimateLoading();
                }
                string url = string.Empty;
                if (serviceType == ServiceType.CurrentWeather)
                    url = $"https://api.openweathermap.org/data/2.5/weather?q={cityName},{country}&units=metric&appid=134f60d139e7bb125d5068fdd4c7ed95";
                else if (serviceType == ServiceType.Forecast)
                    url = $"https://api.openweathermap.org/data/2.5/forecast?q={cityName},{country}&units=metric&appid=134f60d139e7bb125d5068fdd4c7ed95";


                using (WebClient client = new WebClient())
                {
                    try
                    {
                        if (serviceType == ServiceType.CurrentWeather)
                            weatherData = JsonConvert.DeserializeObject<WeatherData>(client.DownloadString(url));
                        else if (serviceType == ServiceType.Forecast)
                            forecast = JsonConvert.DeserializeObject<ForecastData>(client.DownloadString(url));
                        incorrectCityName = false;

                    }
                    catch (WebException ex)
                    {
                        if (ex.Message.Contains("403"))
                        {
                            Console.WriteLine(ex);
                            Console.WriteLine("Forbidden access");
                            return;
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
            if (serviceType == ServiceType.CurrentWeather)
                WriteData(weatherData);
            else if (serviceType == ServiceType.Forecast)
                WriteData(forecast);
        }

        private void WriteData(WeatherData weatherData)
        {
            var sunriseTime = DateTimeOffset.FromUnixTimeSeconds(weatherData.Sys.Sunrise).LocalDateTime.ToShortTimeString();
            var sunsetTime = DateTimeOffset.FromUnixTimeSeconds(weatherData.Sys.Sunset).LocalDateTime.ToShortTimeString();
            Console.WriteLine($"City Name: {weatherData.Name}\n" +
                            $"Country: {weatherData.Sys.Country}\n" +
                            $"Weather: {weatherData.Weather[0].Main}\n" +
                            $"Weather description: {weatherData.Weather[0].Description}\n" +
                            $"Current Temperature: {weatherData.Main.Temp}°C\n" +
                            $"Wind Speed: {weatherData.Wind.Speed} km/h\n" +
                            $"Wind Direction: {weatherData.Wind.Deg}°\n" +
                            $"Temp min: {weatherData.Main.TempMin}°C\n" +
                            $"Temp max: {weatherData.Main.TempMax}°C\n" +
                            $"Sunrise: {sunriseTime}\n" +
                            $"Sunset: {sunsetTime}");
        }

        private void WriteData(ForecastData forecastData)
        {
            foreach (var item in forecastData.Forecast)
            {
                var dateTime = DateTimeOffset.FromUnixTimeSeconds(item.Dt).LocalDateTime;
                string dt = item.Dt_txt;
                Console.WriteLine($"City Name: {forecastData.City.Name}\n" +
                                $"Country: {forecastData.City.Country}\n" +
                                $"Time of day: {dateTime}\n" +
                                $"Weather: {item.Weather[0].Main}\n" +
                                $"Weather description: {item.Weather[0].Description}\n" +
                                $"Temp min: {item.Main.TempMin}°C\n" +
                                $"Temp max: {item.Main.TempMax}°C\n" +
                                $"Wind Speed: {item.Wind.Speed} km/h\n" +
                                $"Wind Direction: {item.Wind.Deg}°\n");
                Thread.Sleep(1000);
            }
        }

        public void AnimateLoading()
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
