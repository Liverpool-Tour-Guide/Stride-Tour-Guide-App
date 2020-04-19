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
                return true;
            });

            int counter = getWaypoints(1, 3);

            for (int i = 0; i < counter; i++)
            {
                Waypoints.Add(new Waypoint
                {
                    StorageIndex = i.ToString(),
                    LandmarkID = landmark_data[i, 0],
                    Name = landmark_data[i, 1],
                    Description = landmark_data[i, 3],
                    AudioURL = landmark_data[i, 4],
                    Visited = false
                });
            }

            BindingContext = this;
        }

    }
}