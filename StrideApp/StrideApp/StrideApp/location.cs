using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Plugin.Geolocator;
using Plugin.Geolocator.Abstractions;


namespace StrideApp
{
    public class location
    {
        public Position position { set; get; }
        public location()
        {
            getInitLocation().Wait();
            //startListening();
        }

        private async Task getInitLocation()
        {
            var locator = CrossGeolocator.Current;
            locator.DesiredAccuracy = 5;

            position = await locator.GetPositionAsync(TimeSpan.FromSeconds(10));
        }

        private async Task startListening()
        {
            await CrossGeolocator.Current.StartListeningAsync(TimeSpan.FromSeconds(5), 10, true);

            CrossGeolocator.Current.PositionChanged += PositionChanged;
        }

        private void PositionChanged(object sender, PositionEventArgs e)
        {
            position = e.Position;
        }
    }
}
