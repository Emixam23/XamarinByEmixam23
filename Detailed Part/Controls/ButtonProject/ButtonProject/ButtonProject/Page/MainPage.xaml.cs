using System;
using System.Diagnostics;

using Xamarin.Forms;

namespace ButtonProject.Page
{
    public partial class MainPage : ContentPage
    {
        public Action<object> OnCustomButtonLongPressCallback { get; set; }

        public MainPage()
        {
            base.BindingContext = this;
            OnCustomButtonLongPressCallback = OnCustomButtonLongPress;
            InitializeComponent();

            //Debug test
            button.test();
        }

        private void OnCustomButtonClicked(object sender, EventArgs ea)
        {
            Debug.WriteLine("Button Clicked !");
        }

        private void OnCustomButtonLongPress(object sender)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                Debug.WriteLine("Button Long Pressed !");
            });
        }
    }
}
