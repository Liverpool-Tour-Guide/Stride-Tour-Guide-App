using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            //locator = new location();
            //position = locator.position;
            //Labels.Text = position.Latitude.ToString();
        }
    }
}