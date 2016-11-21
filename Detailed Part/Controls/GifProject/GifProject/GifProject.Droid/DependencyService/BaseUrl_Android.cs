using GifProject.Droid.DependencyService;
using GifProject.Interface;
using Xamarin.Forms;

[assembly: Dependency(typeof(BaseUrl_Android))]
namespace GifProject.Droid.DependencyService
{
    public class BaseUrl_Android : IBaseUrl
    {
        public string Get()
        {
            return "file:///android_asset/";
        }
    }
}