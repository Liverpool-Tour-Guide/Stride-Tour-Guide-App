using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Plugin.SimpleAudioPlayer;


using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Plugin.Geolocator;
using Plugin.Geolocator.Abstractions;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Text.RegularExpressions;

namespace StrideApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TourRoutPage : ContentPage
    {
        public location locator;
        public Position position;
        string[,] landmark_data = new string[20,7];
        public IList<Waypoint> Waypoints { get; private set; } = new List<Waypoint>();

        AudioPlayer currentAudioPlayer;
        int toggle;
        int currentActiveButton;

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
                        landmark_data[rowCounter, 0] = values[0];

                        for (int i = 3; i < 9; i++)
                        {
                            landmark_data[rowCounter, i - 2] = values[i];
                        }

                        rowCounter++;
                    }
                }
            }

            return rowCounter;
        
        }

        public void AudioTrigger(int ID)
        {
            string audioName = landmark_data[ID, 4];

            if (currentAudioPlayer == null)
            {
                AudioPlayer tempAudioPlayer = new AudioPlayer
                {
                    audioName = audioName
                };
                currentAudioPlayer = tempAudioPlayer;
                currentAudioPlayer.startAudio(audioName);

                Waypoints[ID].ButtonSource = "pause_circle.png";
                Waypoints[ID].Toggle = -1;
                currentActiveButton = ID;

            } else
            {
                int index = currentActiveButton;
                Waypoints[index].ButtonSource = "play_circle.png";
                Waypoints[index].Toggle = 1;

                AudioPlayer tempAudioPlayer = new AudioPlayer
                {
                    audioName = audioName
                };
                currentAudioPlayer.pauseAudio();
                currentAudioPlayer = tempAudioPlayer;
                currentAudioPlayer.startAudio(audioName);

                Waypoints[ID].ButtonSource = "pause_circle.png";
                Waypoints[ID].Toggle = -1;
                currentActiveButton = ID;
            }
        }

        async void OnPlayButtonClicked(object sender, EventArgs e)
        {//This is how you navigate between pages
            var button = (ImageButton)sender;
            int index = Int32.Parse(button.ClassId);
            string audioName = landmark_data[index, 4];

            if (Waypoints[index].Toggle == 1)
            {

                if (currentAudioPlayer != null)
                {
                    if (currentAudioPlayer.audioName == audioName)
                    {
                        if (!currentAudioPlayer.audioPlaying)
                        {
                            currentAudioPlayer.playAudio();
                            toggle = -1;
                        }
                    }
                    else
                    {
                        int temp_index = currentActiveButton;
                        Waypoints[temp_index].ButtonSource = "play_circle.png";
                        button.WidthRequest = 64;
                        button.HeightRequest = 64;
                        button.BackgroundColor = Color.Transparent;
                        Waypoints[temp_index].Toggle = 1;

                        AudioPlayer tempAudioPlayer = new AudioPlayer
                        {
                            audioName = audioName
                        };
                        currentAudioPlayer = tempAudioPlayer;
                        currentAudioPlayer.startAudio(audioName);
                        currentActiveButton = index;
                        Waypoints[index].changeVisitStatus();

                        toggle = -1;
                    }
                }
                else
                {
                    AudioPlayer tempAudioPlayer = new AudioPlayer
                    {
                        audioName = audioName
                    };
                    currentAudioPlayer = tempAudioPlayer;
                    currentAudioPlayer.startAudio(audioName);
                    currentActiveButton = index;
                    Waypoints[index].changeVisitStatus();

                    toggle = -1;
                }
            } else
            {
                if (currentAudioPlayer.audioPlaying)
                {
                    currentAudioPlayer.pauseAudio();
                    toggle = 1;
                }
            }

        }

        async void ToggleClickedHandler(object sender, EventArgs e)
        {
            var button = (ImageButton)sender;
            int index = Int32.Parse(button.ClassId);

            if (toggle == 1)
            {
                //Play Displayed
                Waypoints[index].ButtonSource = "play_circle.png";
                button.WidthRequest = 64;
                button.HeightRequest = 64;
                button.BackgroundColor = Color.Transparent;
                Waypoints[index].Toggle = 1;

            } else if (toggle == -1)
            {
                //Pause Displayed
                Waypoints[index].ButtonSource = "pause_circle.png";
                button.WidthRequest = 64;
                button.HeightRequest = 64;
                button.BackgroundColor = Color.Transparent;
                Waypoints[index].Toggle = -1;
            }
        }

        private void refreshPosition()
        {
            position = locator.position;

        }


        public TourRoutPage()
        {
  
            InitializeComponent();
            locator = new location();
            position = locator.position;

            // Starts a time on the main thread that calls the refresh function every 5 seconds
            Device.StartTimer(TimeSpan.FromSeconds(5), () =>
            {
                Device.BeginInvokeOnMainThread(() => refreshPosition());

                Console.WriteLine("JAKOB MESSAGES: Checking location...");

                Position current_position = position;
                double user_lat = current_position.Latitude;
                double user_long = current_position.Longitude;

                Console.WriteLine($"JAKOB MESSAGES: Location at {user_lat}, {user_long}");

                for (int i = 0; i < Waypoints.Count; i++)
                {
                    double waypoint_lat = Waypoints[i].LandmarkGPSLocation.Latitude;
                    double waypoint_long = Waypoints[i].LandmarkGPSLocation.Longitude;

                    Console.WriteLine($"{Waypoints[i].Name} has latitude of {waypoint_lat} and longitude of {waypoint_long}");

                    double lat_upper = waypoint_lat + 0.0001;
                    double lat_lower = waypoint_lat - 0.0001;
                    double long_upper = waypoint_long + 0.0001;
                    double long_lower = waypoint_long - 0.0001;

                    if ((!Waypoints[i].Visited) && ((user_lat >= lat_lower) && (user_lat <= lat_upper)) && ((user_long >= long_lower) && (user_long <= long_upper)))
                    {
                        Console.WriteLine($"JAKOB MESSAGES: Triggering audio for Landmark {Waypoints[i].Name}");
                        Waypoints[i].Visited = true;
                        AudioTrigger(i);
                    }
                }
                return true;
            });

            int counter = getWaypoints(1, 4);

            for (int i = 0; i < counter; i++)
            {
                double latitude, longitude;

                Console.WriteLine($"JAKOB MESSAGE: LandmarkData Reading = {landmark_data[i,5]}");

                string[] position_values = landmark_data[i, 5].Split(',');
                char[] charsToTrim = { '"' };
                position_values[0] = position_values[0].Trim(charsToTrim);
                position_values[1] = position_values[1].Trim(charsToTrim);

                Console.WriteLine($"JAKOB MESSAGE: Split array = {position_values[0]}, {position_values[1]}");

                Double.TryParse(position_values[0],out latitude);
                Double.TryParse(position_values[1], out longitude);

                Console.WriteLine($"JAKOB MESSAGE: Calculated latitude = {latitude}, calculated longitude = {longitude}");

                Position position = new Position(latitude,longitude);


                Waypoints.Add(new Waypoint
                {
                    StorageIndex = i.ToString(),
                    LandmarkID = landmark_data[i, 0],
                    Name = landmark_data[i, 1],
                    Description = landmark_data[i, 3],
                    AudioURL = landmark_data[i, 4],
                    LandmarkGPSLocation = position,
                    ButtonSource = "play_circle.png",
                    Toggle = 1,
                    Visited = false
                });
            }

            BindingContext = this;
        }

    }
}