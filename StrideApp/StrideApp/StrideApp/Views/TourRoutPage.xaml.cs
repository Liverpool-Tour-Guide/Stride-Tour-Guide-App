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
        ImageButton currentActiveButton;

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

                ImageButton current_button = (ImageButton)FindViewByID(ID);

                current_button.Source = "pause_circle.png";
                current_button.WidthRequest = 64;
                current_button.HeightRequest = 64;
                current_button.BackgroundColor = Color.Transparent;
                current_button.Pressed += OnPauseButtonClicked;

                currentActiveButton = current_button;
            } else
            {
                currentActiveButton.Source = "play_circle.png";
                currentActiveButton.WidthRequest = 64;
                currentActiveButton.HeightRequest = 64;
                currentActiveButton.BackgroundColor = Color.Transparent;
                currentActiveButton.Pressed += OnPlayButtonClicked;

                AudioPlayer tempAudioPlayer = new AudioPlayer
                {
                    audioName = audioName
                };
                currentAudioPlayer.pauseAudio();
                currentAudioPlayer = tempAudioPlayer;
                currentAudioPlayer.startAudio(audioName);

                ImageButton current_button = (ImageButton)FindByName(Waypoints[ID].Name);

                current_button.Source = "pause_circle.png";
                current_button.WidthRequest = 64;
                current_button.HeightRequest = 64;
                current_button.BackgroundColor = Color.Transparent;
                current_button.Pressed += OnPauseButtonClicked;

                currentActiveButton = current_button;
            }
        }

        async void OnPlayButtonClicked(object sender, EventArgs e)
        {//This is how you navigate between pages
            var button = (ImageButton)sender;
            int index = Int32.Parse(button.ClassId);
            string audioName = landmark_data[index, 4];

            if (currentAudioPlayer != null)
            {
                if (currentAudioPlayer.audioName == audioName)
                {
                    if (!currentAudioPlayer.audioPlaying)
                    {
                        currentAudioPlayer.playAudio();
                        toggle = -1;
                    }
                } else
                {
                    currentActiveButton.Source = "play_circle.png";
                    button.WidthRequest = 64;
                    button.HeightRequest = 64;
                    button.BackgroundColor = Color.Transparent;
                    currentActiveButton.Pressed += OnPlayButtonClicked;

                    AudioPlayer tempAudioPlayer = new AudioPlayer
                    {
                        audioName = audioName
                    };
                    currentAudioPlayer = tempAudioPlayer;
                    currentAudioPlayer.startAudio(audioName);
                    currentActiveButton = button;
                    Waypoints[index].changeVisitStatus();

                    toggle = -1;
                }
            } else
            {
                AudioPlayer tempAudioPlayer = new AudioPlayer
                {
                    audioName = audioName
                };
                currentAudioPlayer = tempAudioPlayer;
                currentAudioPlayer.startAudio(audioName);
                currentActiveButton = button;
                Waypoints[index].changeVisitStatus();

                toggle = -1;
            }

        }

        async void OnPauseButtonClicked(object sender, EventArgs e)
        {//This is how you navigate between pages
            if (currentAudioPlayer.audioPlaying)
            {
                currentAudioPlayer.pauseAudio();
                toggle = 1;
            }
        }

        async void ToggleClickedHandler(object sender, EventArgs e)
        {
            var button = (ImageButton)sender;

            if (toggle == 1)
            {
                //Play Displayed
                button.Source = "play_circle.png";
                button.WidthRequest = 64;
                button.HeightRequest = 64;
                button.BackgroundColor = Color.Transparent;
                button.Pressed += OnPlayButtonClicked;

            } else if (toggle == -1)
            {
                //Pause Displayed
                button.Source = "pause_circle.png";
                button.WidthRequest = 64;
                button.HeightRequest = 64;
                button.BackgroundColor = Color.Transparent;
                button.Pressed += OnPauseButtonClicked;
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

                Position current_position = locator.position;
                double user_lat = current_position.Latitude;
                double user_long = current_position.Longitude;

                Console.WriteLine($"JAKOB MESSAGES: Location at {user_lat}, {user_long}");

                for (int i = 0; i < Waypoints.Count; i++)
                {
                    double waypoint_lat = Waypoints[i].LandmarkGPSLocation.Latitude;
                    double waypoint_long = Waypoints[i].LandmarkGPSLocation.Longitude;

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

                string[] position_values = landmark_data[i, 5].Split(',');

                Double.TryParse(position_values[0],out latitude);
                Double.TryParse(position_values[1], out longitude);
                Position position = new Position(latitude,longitude);


                Waypoints.Add(new Waypoint
                {
                    StorageIndex = i.ToString(),
                    LandmarkID = landmark_data[i, 0],
                    Name = landmark_data[i, 1],
                    Description = landmark_data[i, 3],
                    AudioURL = landmark_data[i, 4],
                    LandmarkGPSLocation = position,
                    Visited = false
                });
            }

            BindingContext = this;
        }

    }
}