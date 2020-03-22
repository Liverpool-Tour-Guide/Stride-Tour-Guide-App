using System;
using System.Collections.Generic;
using System.IO;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Reflection;
using System.Text.RegularExpressions;

namespace StrideApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TourRoutPage : ContentPage
    {
        string[,] landmark_data = new string[20,6];
        public IList<Waypoint> Waypoints { get; private set; }

        public int getWaypoints(int cityID, int tourID) 
        {
            int rowCounter = 0;

            String CityID = cityID.ToString();
            String TourID = tourID.ToString();

            var assembly = IntrospectionExtensions.GetTypeInfo(typeof(LoadResourceText)).Assembly;
            Stream stream = assembly.GetManifestResourceStream("StrideApp.POPULATED_DATA_STRIDE.Landmark.txt");

            using (var reader = new StreamReader(stream))
            {

                while (!reader.EndOfStream)
                {
                    string line = reader.ReadLine();
                    String[] values = Regex.Split(line,",(?=(?:[^\"]*\"[^\"]*\")*[^\"]*$)");

                    string temp = values[2];

                    if ((values[1] == CityID) && (temp.Contains(TourID)))
                    {
                        for (int i = 3; i < 9; i++)
                        {
                            landmark_data[rowCounter, i - 3] = values[i];
                        }

                        rowCounter++;
                    }
                }
            }

            return rowCounter;
        
        }

        public TourRoutPage()
        {
            InitializeComponent();

            Waypoints = new List<Waypoint>();

            int counter = getWaypoints(1,3);

            for (int i = 0; i < counter; i++)
            {
                Waypoints.Add(new Waypoint
                {
                    Name = landmark_data[i, 0],
                    Description = landmark_data[i, 2],
                    AudioURL = landmark_data[i, 3],
                    Visited = false
                });
            }

            BindingContext = this;

        }
    }
}