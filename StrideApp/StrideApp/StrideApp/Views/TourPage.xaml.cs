using StrideApp.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace StrideApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TourPage : ContentPage
    {

        private ObservableCollection<TourGroup> _allGroups;
        private ObservableCollection<TourGroup> _expandedGroups;
        int selectedCityID;

        public TourPage(int cityID)
        {
            InitializeComponent();
            _allGroups = TourGroup.All;
            UpdateListContent();
            selectedCityID = cityID;

            /*tourListView.ItemsSource = new List<string>()
            {
                "Tour 1","Tour 2", "Tour 3", "Tour 4"
            }; */
        }

        private void HeaderTapped(object sender, EventArgs args)
        {
            int selectedIndex = _expandedGroups.IndexOf(
                ((TourGroup)((Button)sender).CommandParameter));
            _allGroups[selectedIndex].Expanded = !_allGroups[selectedIndex].Expanded;
            UpdateListContent();
        }

        private void UpdateListContent()
        {

            _expandedGroups = new ObservableCollection<TourGroup>();

            foreach (TourGroup group in _allGroups)
            {

                //Create new TourGroups so we do not alter original list
                TourGroup newGroup = new TourGroup(group.Title, group.ShortName, group.Expanded);
                //Add the count of tour items for Lits Header Titles to use
                newGroup.TourCount = group.Count;
                if (group.Expanded)
                {
                    foreach (Tours tour in group)
                    {
                        newGroup.Add(tour);
                    }
                }
                _expandedGroups.Add(newGroup);
            }
            GroupedView.ItemsSource = _expandedGroups;
        }

        async void OnNoteAddedClicked(object sender, EventArgs e)
        { //This is how you navigate between pages
            await Navigation.PushAsync(new TourTabbedPage(selectedCityID,4) //Passing cityID and tourID
            {
            });
        }
    }
}