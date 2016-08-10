using MapPolylineProject.CustomControl;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                "51 rue Mangeard, 72100 Le Mans, France",
                "5 rue Maudoux, 72100 Le Mans, France",
                "7 cour Alphonse  Daudet, 72230 Ruaudin, France"
            };
            
            InitializeComponent();

            test();
            /*Debug.WriteLine("--DEBUG ADDRESS--");
            foreach (string item in MapTest.PolylineAddressPoints)
            {
                Debug.WriteLine("Adress => [{0}]", item);
            }
            Debug.WriteLine("--DEBUG ADDRESS--");

            Debug.WriteLine("--DEBUG POSITION--");
            foreach (GeoPosition item in MapTest.PolylineCoordinates)
            {
                Debug.WriteLine("GeoPosition => [{0}/{1}]", item.Latitude, item.Longitude);
            }
            Debug.WriteLine("--DEBUG POSITION--");*/
        }

        private async void test()
        {
            Debug.WriteLine("--DEBUG ADDRESS--");
            foreach (string item in MapTest.PolylineAddressPoints)
            {
                Debug.WriteLine("Adress => [{0}]", item);
            }
            Debug.WriteLine("--DEBUG ADDRESS--");

            List<GeoPosition> list = await CustomMap.GeneratePolylineCoordinates(AddressPointList);

            Debug.WriteLine("--DEBUG POSITION--");
            /*foreach (GeoPosition item in list)
            {
                Debug.WriteLine("GeoPosition => [{0}/{1}]", item.Latitude, item.Longitude);
            }*/
            Debug.WriteLine("--DEBUG POSITION--");
            MapTest.PolylineCoordinates = list;
        }
    }
}
