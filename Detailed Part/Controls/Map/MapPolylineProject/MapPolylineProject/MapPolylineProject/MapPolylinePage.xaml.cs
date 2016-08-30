using MapPolylineProject.CustomControl;
using System.Collections.Generic;
using System.Diagnostics;

using Xamarin.Forms;
using static MapPolylineProject.CustomControl.CustomMap;

namespace MapPolylineProject
{
    public partial class MapPolylinePage : ContentPage
    {
        public List<string> AddressPointList { get; set; }

        public MapPolylinePage()
        {
            base.BindingContext = this;

            AddressPointList = new List<string>()
            {
                "72230 Ruaudin, France",
                "72100 Le Mans, France",
                "77500 Chelles, France"
            };
            
            InitializeComponent();
        }
    }
}
