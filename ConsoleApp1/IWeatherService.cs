namespace ConsoleApp1
{
    internal interface IWeatherService
    {
        //uses Location service
        void GetData();
        void GetData(string cityName, string country = "ro");
    }
}
