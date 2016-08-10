using ImageCircleProject.CustomControl;
using ImageCircleProject.UWP.CustomRenderer;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.Storage;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Shapes;
using Xamarin.Forms;
using Xamarin.Forms.Platform.UWP;

[assembly: ExportRenderer(typeof(CustomImageCircle), typeof(CustomImageCircleRenderer))]
namespace ImageCircleProject.UWP.CustomRenderer
{
    /// <summary>
    /// CustomRenderer for the CustomImageCircle created in the PCL part.
    /// This Renderer gives us the possibility to add/override some functionalities.
    /// </summary>
    public class CustomImageCircleRenderer : ViewRenderer<CustomImageCircle, Ellipse>
    {
        /// <summary>
        /// We override the OnElementChanged() event handler to get the desired instance. We also use it for updates.
        /// </summary>
        /// <param name="e">It contains either the NewElement or the OldElement</param>
        protected override void OnElementChanged(ElementChangedEventArgs<CustomImageCircle> e)
        {
            base.OnElementChanged(e);

            Ellipse ellipse = new Ellipse();
            SetNativeControl(ellipse);
        }

        /// <summary>
        /// We override OnElementPropertyChanged event handle to make circular the image.
        /// </summary>
        /// <param name="sender">CustomImage object instance (of XAML).</param>
        /// <param name="e">Information about the event handled.</param>
        protected async override void OnElementPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            if (Control != null)
            {
                var min = Math.Min(Element.Width, Element.Height) / 2.0f;
                if (min <= 0)
                    return;

                double radius;
                if (Element.Width < Element.Height)
                {
                    radius = Element.Width;
                }
                else
                {
                    radius = Element.Height;
                }

                Control.Width = radius;
                Control.Height = radius;

                // That will be our fallback fill if can't make sense of the ImageSource.
                Control.Fill = new SolidColorBrush(Windows.UI.Color.FromArgb(255, 52, 152, 219));

                BitmapImage bitmapImage = null;

                // Handle file images
                if (Element.Source is FileImageSource)
                {
                    FileImageSource fi = Element.Source as FileImageSource;
                    string myFile = System.IO.Path.Combine(Package.Current.InstalledLocation.Path, fi.File);
                    StorageFolder myFolder = await StorageFolder.GetFolderFromPathAsync(System.IO.Path.GetDirectoryName(myFile));

                    using (Stream s = await myFolder.OpenStreamForReadAsync(System.IO.Path.GetFileName(myFile)))
                    {
                        var memStream = new MemoryStream();
                        await s.CopyToAsync(memStream);
                        memStream.Position = 0;
                        bitmapImage = new BitmapImage();
                        bitmapImage.SetSource(memStream.AsRandomAccessStream());
                    }

                }
                // handle embedded images
                else if (Element.Source is StreamImageSource)
                {
                    using (Stream s = await GetStreamFromImageSourceAsync(Element.Source as StreamImageSource))
                    {
                        var memStream = new MemoryStream();
                        await s.CopyToAsync(memStream);
                        memStream.Position = 0;
                        bitmapImage = new BitmapImage();
                        bitmapImage.SetSource(memStream.AsRandomAccessStream());
                    }
                }
                // Handle uri images
                else if (Element.Source is UriImageSource)
                {
                    bitmapImage = new BitmapImage((Element.Source as UriImageSource).Uri);
                }

                if (bitmapImage != null)
                    Control.Fill = new ImageBrush() { ImageSource = bitmapImage };
            }
        }

        /// <summary>
        /// Convert StreamImageSource into a simple Stram instance.
        /// </summary>
        /// <param name="imageSource">The StreamImageSource instance.</param>
        /// <param name="cancellationToken">Optional parameter for CancellationToken.</param>
        /// <returns></returns>
        private static async Task<Stream> GetStreamFromImageSourceAsync(StreamImageSource imageSource, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (imageSource.Stream != null)
            {
                return await imageSource.Stream(cancellationToken);
            }
            return null;
        }
    }
}
