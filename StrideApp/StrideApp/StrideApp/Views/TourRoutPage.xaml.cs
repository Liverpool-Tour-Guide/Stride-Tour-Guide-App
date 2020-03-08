using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace StrideApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TourRoutPage : ContentPage
    {

        public IList<Waypoint> Waypoints { get; private set; }

        public TourRoutPage()
        {
            InitializeComponent();

            Waypoints = new List<Waypoint>();

            Waypoints.Add(new Waypoint
            {
                Name = "International Slavery Museum",
                Description = "Hear the untold stories of enslaved people and learn about historical and contemporary slavery.",
                AudioURL = "INSERT URL",
                Visited = true
            });

            Waypoints.Add(new Waypoint
            {
                Name = "Merseyside Maritime Museum",
                Description = "Discover Liverpool's seafaring past and find out about a life at sea.",
                AudioURL = "INSERT URL",
                Visited = true
            });

            Waypoints.Add(new Waypoint
            {
                Name = "Royal Albert Dock",
                Description = "The Royal Albert Dock is a complex of dock buildings and warehouses in Liverpool, England. Designed by Jesse Hartley and Philip Hardwick, it was opened in 1846, and was the first structure in Britain to be built from cast iron, brick and stone, with no structural wood.",
                AudioURL = "INSERT URL",
                Visited = false
            });

            Waypoints.Add(new Waypoint
            {
                Name = "Tate Liverpool",
                Description = "Liverpool's Museum of Modern Art",
                AudioURL = "INSERT URL",
                Visited = false
            });

            Waypoints.Add(new Waypoint
            {
                Name = "The Beatles Story",
                Description = "Experience the Beatles from their childhood to their stardom.",
                AudioURL = "INSERT URL",
                Visited = false
            });

            BindingContext = this;

        }
    }
}