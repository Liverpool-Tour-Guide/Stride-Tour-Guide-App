using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;


namespace StrideApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TourTabbedPage : TabbedPage
    {
        
        public TourTabbedPage(int cityID, int tourID)
        {
            Children.Add(new MapPage(cityID, tourID));
            Children.Add(new TourRoutPage(cityID, tourID));
            InitializeComponent();
            
        }
    }
}

