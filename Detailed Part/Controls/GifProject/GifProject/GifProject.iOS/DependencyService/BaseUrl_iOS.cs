using Foundation;
using GifProject.Interface;
using GifProject.iOS.DependencyService;
using Xamarin.Forms;

[assembly: Dependency(typeof(BaseUrl_iOS))]
namespace GifProject.iOS.DependencyService
{
    public class BaseUrl_iOS : IBaseUrl
    {
        public string Get()
        {
            return NSBundle.MainBundle.BundlePath;
        }
    }
}
