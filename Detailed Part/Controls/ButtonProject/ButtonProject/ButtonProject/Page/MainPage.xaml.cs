using System;
using System.Diagnostics;
using Xamarin.Forms;

namespace ButtonProject.Page
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private void OnCustomButtonLongPress(object sender, EventArgs ea)
        {
            Debug.WriteLine("Button Long Pressed !");
        }

        private void OnCustomButtonClicked(object sender, EventArgs ea)
        {
            Debug.WriteLine("Button clicked !");
        }
    }
}
