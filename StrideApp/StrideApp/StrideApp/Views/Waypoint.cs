using Plugin.Geolocator.Abstractions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace StrideApp
{
    public class Waypoint : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void NotifyPropertyChanged(string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


        public string StorageIndex { get; set; }
        public string LandmarkID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string AudioURL { get; set; }
        public Position LandmarkGPSLocation { get; set; }
        public bool Visited { get; set; }


        public string MarkColor {
            get
            {
                if (Visited)
                {
                    return "Gray";
                }
                else
                {
                    return "DeepSkyBlue";
                }
            }
        }

        public string TextColor {
            get
            {
                if (Visited)
                {
                    return "Gray";
                }
                else
                {
                    return "Black";
                }
            }
        }


        private string buttonsource;

        public string ButtonSource
        {
            get
            {
                return buttonsource;
            }
            set
            {
                if(buttonsource != value)
                {
                    buttonsource = value;
                    NotifyPropertyChanged();
                }
            }
        }


       public int Toggle { get; set; }


        public override string ToString()
        {
            return Name;
        }

        public void changeVisitStatus()
        {
            Visited = true;
        }
    }
}
