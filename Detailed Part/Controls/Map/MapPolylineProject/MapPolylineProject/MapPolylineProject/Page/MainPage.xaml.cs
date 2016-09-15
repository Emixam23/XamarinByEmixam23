using System.Collections.Generic;
using Xamarin.Forms;

namespace MapPolylineProject.Page
{
    public partial class MainPage : ContentPage
    {
        public List<string> AddressPointList { get; set; }

        public MainPage()
        {
            BindingContext = this;

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
