using GifProject.Interface;
using GifProject.UWP.DependencyService;
using Xamarin.Forms;

[assembly: Dependency(typeof(BaseUrl_UWP))]
namespace GifProject.UWP.DependencyService
{
    public class BaseUrl_UWP : IBaseUrl
    {
        public string Get()
        {
            return "ms-appx-web:///Assets/";
        }
    }
}
