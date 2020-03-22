using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Plugin.SimpleAudioPlayer;


using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Text.RegularExpressions;

namespace StrideApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TourRoutPage : ContentPage
    {
        string[,] landmark_data = new string[20,7];
        public IList<Waypoint> Waypoints { get; private set; } = new List<Waypoint>();

        AudioPlayer currentAudioPlayer;
        int toggle;
        Button currentActiveButton;

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
            var button = (Button)sender;
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
                    currentActiveButton.Text = "Play";
                    currentActiveButton.BorderColor = Color.Green;
                    currentActiveButton.Pressed += OnPlayButtonClicked;

                    AudioPlayer tempAudioPlayer = new AudioPlayer
                    {
                        audioName = audioName
                    };
                    currentAudioPlayer = tempAudioPlayer;
                    currentAudioPlayer.startAudio(audioName);
                    currentActiveButton = button;
                    Waypoints[index].Visited= true;

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
                Waypoints[index].Visited = true;

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
            var button = (Button)sender;

            if (toggle == 1)
            {
                //Play Displayed
                button.Text = "Play";
                button.BorderColor = Color.Green;
                button.Pressed += OnPlayButtonClicked;

            } else if (toggle == -1)
            {
                //Pause Displayed
                button.Text = "Pause";
                button.BorderColor = Color.Red;
                button.Pressed += OnPauseButtonClicked;
            }
        }


        public TourRoutPage()
        {
            InitializeComponent();


            //Waypoints = new List<Waypoint>();

            int counter = getWaypoints(1,3);

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