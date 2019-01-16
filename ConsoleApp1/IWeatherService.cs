namespace ConsoleApp1
{
    internal interface IWeatherService
    {
        //uses Location service
        void GetData(ServiceType serviceType);
        void GetData(string cityName, ServiceType serviceType, string country = "ro");
        void AnimateLoading();
    }
}
