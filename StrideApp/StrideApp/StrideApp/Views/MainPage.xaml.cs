using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace StrideApp
{
    
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            
        }

        async void OnNoteAddedClicked(object sender, EventArgs e)
        {//This is how you navigate between pages
            await Navigation.PushAsync(new TourPage
            {
            });
        }
    }
}
