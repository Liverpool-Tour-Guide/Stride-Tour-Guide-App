using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace StrideApp
{
    class TourImageCell : ImageCell
    {
        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();
            var tour = (Tours)BindingContext;
        }
    }
}
