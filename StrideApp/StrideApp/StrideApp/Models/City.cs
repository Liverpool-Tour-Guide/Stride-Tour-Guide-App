using System;
using Xamarin.Forms;

namespace StrideApp.Models
{
    public class City
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string UpperCaseName => Name.ToUpper();
        public string ImageFileName { get; set; }
        public ImageSource ImageSource { get; set; }
    }
}
