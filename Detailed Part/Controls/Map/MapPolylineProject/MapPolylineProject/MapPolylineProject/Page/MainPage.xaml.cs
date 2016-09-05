using MapPolylineProject.CustomControl;
using System.Collections.Generic;
using System.Diagnostics;
using Xamarin.Forms;
using static MapPolylineProject.CustomControl.CustomMap;

namespace MapPolylineProject.Page
{
    public partial class MainPage : ContentPage
    {
        public List<string> AddressPointList { get; set; }

        public MainPage()
        {
            base.BindingContext = this;

            AddressPointList = new List<string>()
            {
                "72230 Ruaudin, France",
                "72100 Le Mans, France",
                "77500 Chelles, France"
            };

            Debug.WriteLine("HALLOOOO");

            InitializeComponent();

            MapTest.PolylineAddressPoints = AddressPointList;

            //test();
        }

        private async void test()
        {
            List<GeoPosition> list = await GeneratePolylineCoordinates(AddressPointList);
            MapTest.PolylineCoordinates = list;
        }
    }
}
