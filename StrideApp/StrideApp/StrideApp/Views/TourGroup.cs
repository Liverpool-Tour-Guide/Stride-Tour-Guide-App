using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using System.Diagnostics;
using System.Reflection;
using StrideApp.Models;
using StrideApp.Views;


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
                 new TourGroup("Albert Dock Tour","C"){
                     new Tours { Name = "International SlaveryMuseum", Description = "Hear the untold stories of enslaved people and learn about historical and contemporary slavery.", Icon="slavery_museum.jpg" },
                     new Tours { Name = "Merseyside Maritime Museum", Description = "Discover Liverpool's seafaring past and find out about a life at sea.", Icon="museum.jpg" },
                     new Tours { Name = "Royal Albert Dock", Description = "The Royal Albert Dock is a complex of dock buildings and warehouses in Liverpool, England. Designed by Jesse Hartley and Philip Hardwick, it was opened in 1846, and was the first structure in Britain to be built from cast iron, brick and stone, with no structural wood.", Icon="albertdock.jpg" },
                     new Tours { Name = "Tate Liverpool", Description = "A Museum of Modern Art.", Icon="tate.jpg" },
                     new Tours { Name = "The Beatles Story", Description = "The Beatles Story is a museum in Liverpool about the Beatles and their history. It is located on the historical Royal Albert Dock.", Icon="beatles.jpg" },
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