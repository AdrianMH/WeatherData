using System.Device.Location;
using System.Threading;

namespace ConsoleApp1
{
    class Location
    {
        public void GetLocationProperty(out double latitude, out double longitude)
        {
            GeoCoordinateWatcher watcher = new GeoCoordinateWatcher();
            watcher.Start();
            if (watcher.Position.Location.IsUnknown)
            {
                Thread.Sleep(500);
            }
            GeoCoordinate coord = watcher.Position.Location;
            latitude = coord.Latitude;
            longitude = coord.Longitude;
        }
    }
}
