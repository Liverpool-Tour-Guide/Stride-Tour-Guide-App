using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs.Host.Bindings;
using StrideApp.Models;
using StrideApp.Views;
using Xamarin.Forms;

namespace StrideApp
{
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {
        public List<City> Cities { get; set; }

        public MainPage()
        {
            InitializeComponent();
            Cities = ParseCsv();
            BindingContext = this;
        }

        private string[] ReadFile()
        {
            var csvFilePath = "StrideApp.POPULATED_DATA_STRIDE.City.txt";
            var assembly = IntrospectionExtensions.GetTypeInfo(typeof(LoadResourceText)).Assembly;
            var stream = assembly.GetManifestResourceStream(csvFilePath);
            // using invokes Dispose function to unlock file being read after return
            using (var reader = new System.IO.StreamReader(stream))
            {
                var text = reader.ReadToEnd();
                return text.Split(Environment.NewLine.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            }
        }

        private List<City> ParseCsv()
        {
            var cities = new List<City>();
            var csvOutput = ReadFile();
           
           
            for (int i = 1; i < csvOutput.Length; i++)
            {
                var splitLine = csvOutput[i].Split(',');

                var city = new City
                {
                    Id = int.Parse(splitLine[0]),
                    Name = splitLine[1],
                    ImageFileName = splitLine[2],
                    ImageSource = ImageSource.FromResource($"StrideApp.POPULATED_DATA_STRIDE.{splitLine[2]}", typeof(EmbeddedImages).GetTypeInfo().Assembly)
                };

                cities.Add(city);
            }

            return cities;
        }

        async void OnNoteAddedClicked(object sender, EventArgs e)
        {//This is how you navigate between pages
            await Navigation.PushAsync(new TourPage
            {
            });
        }

        async void CitySelected_Tapped(System.Object sender, System.EventArgs e)
        {
            var currentCity = (sender as BindableObject).BindingContext as City;
            Debug.WriteLine($"{currentCity.Name} has been selected");

            await Navigation.PushAsync(new TourPage
            {
                // City = currentCity this is a city object (see models folder)
            });

        }
    }
}
