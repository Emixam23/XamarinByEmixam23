using MapPinsProject.Page;
using Xamarin.Forms;

namespace MapPinsProject
{
    public class App : Application
    {
        /// <summary>
        /// Public Google API Key for the whole application.
        /// It's mandatory for the map function of lat/lng finder or address reverser.
        /// </summary>
        public static readonly string GOOGLE_MAP_API_KEY = "YOUR_API_KEY";

        public App()
        {
            // The root page of your application
            MainPage = new MainPage();
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
