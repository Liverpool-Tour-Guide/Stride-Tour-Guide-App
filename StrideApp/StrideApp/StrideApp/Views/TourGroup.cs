using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace StrideApp
{
    /// <summary>
    /// A list of lists
    /// </summary>

    public class TourGroup : ObservableCollection<Tours>, INotifyPropertyChanged
    {

        private bool _expanded;

        public string Title { get; set; }

        public string TitleWithItemCount
        {
            get { return string.Format("{0} ({1})", Title, TourCount); }
        }

        public string ShortName { get; set; }

        public bool Expanded
        {
            get { return _expanded; }
            set
            {
                if (_expanded != value)
                {
                    _expanded = value;
                    OnPropertyChanged("Expanded");
                    OnPropertyChanged("StateIcon");
                }
            }
        }
        

        public string StateIcon
        {
            

            get { return Expanded ? "expanded_blue.png" : "collapsed_blue.png"; }
        }

        public int TourCount { get; set; }

        public TourGroup(string title, string shortName, bool expanded = false)
        {
            Title = title;
            ShortName = shortName;
            Expanded = expanded;
        }

        public static ObservableCollection<TourGroup> All { private set; get; }

        static TourGroup()
        {
            ObservableCollection<TourGroup> Groups = new ObservableCollection<TourGroup>{
                new TourGroup("Tour The Beatles","C"){
                    new Tours { Name = "Place 1", Description = "Description", Icon="pasta.png" },
                    new Tours { Name = "Place 2", Description = "Description", Icon="potato.png" },
                    new Tours { Name = "Place 3", Description = "Description", Icon="bread.png" },
                    new Tours { Name = "Place 4", Description = "Description", Icon="rice.png" },
                },
                new TourGroup("University Tour","F"){
                    new Tours { Name = "Place 1", Description = "Description", Icon="apple.png"},
                    new Tours { Name = "Place 2", Description = "Description", Icon="banana.png"},
                    new Tours { Name = "Place 3", Description = "Description", Icon="pear.png"},
                },
                new TourGroup("Pubs & Bars Tour","V"){
                    new Tours { Name = "Place 1", Description = "Description", Icon="carrot.png"},
                    new Tours { Name = "Place 2", Description = "Description", Icon="greenbean.png"},
                    new Tours { Name = "Place 3", Description = "Description", Icon="broccoli.png"},
                    new Tours { Name = "Place 4", Description = "Description", Icon="peas.png"},
                },
                new TourGroup("Nature Tour","D"){
                    new Tours { Name = "Place 1", Description = "Description", Icon="milk.png"},
                    new Tours { Name = "Place 2", Description = "Description", Icon="cheese.png"},
                    new Tours { Name = "Place 3", Description = "Description", Icon="icecream.png"},

                } };
            All = Groups;
        }


        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }

}