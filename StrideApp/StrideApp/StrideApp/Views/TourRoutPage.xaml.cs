using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Plugin.Geolocator;
using Plugin.Geolocator.Abstractions;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace StrideApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TourRoutPage : ContentPage
    {
        public Position position { set; get; }
        public location locator { set; get; }
        public TourRoutPage()
        {
            InitializeComponent();
            locator = new location();
            position = locator.position;

			Long.Text = position.Longitude.ToString();
            Lat.Text = position.Latitude.ToString();

            // Starts a time on the main thread that calls the refresh function every 5 seconds
            Device.StartTimer(TimeSpan.FromSeconds(5), () =>
            {
                Device.BeginInvokeOnMainThread(() => refreshPosition());
                return true;
            });
        }

        private void refreshPosition()
        {
            position = locator.position;
            Long.Text = position.Longitude.ToString();
            Lat.Text = position.Latitude.ToString();
        }

    }
}