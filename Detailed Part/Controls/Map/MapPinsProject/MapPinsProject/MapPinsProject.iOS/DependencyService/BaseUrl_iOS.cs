using Foundation;
using MapPinsProject.Interface;
using MapPinsProject.iOS.DependencyService;
using Xamarin.Forms;

[assembly: Dependency(typeof(BaseUrl_iOS))]
namespace MapPinsProject.iOS.DependencyService
{
    public class BaseUrl_iOS : IBaseUrl
    {
        public string Get()
        {
            return NSBundle.MainBundle.BundlePath;
        }
    }
}