using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Stride_App.Services;
using Stride_App.Views;

namespace Stride_App
{
    public partial class App : Application
    {

        public App()
        {
            InitializeComponent();

            DependencyService.Register<MockDataStore>();
            MainPage = new MainPage();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
