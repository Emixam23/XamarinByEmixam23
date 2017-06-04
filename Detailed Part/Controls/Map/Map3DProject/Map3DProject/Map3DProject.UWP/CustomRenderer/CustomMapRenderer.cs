using Map3DProject.CustomControl;
using Map3DProject.UWP.CustomRenderer;
using Windows.Devices.Geolocation;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Maps;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Maps.UWP;
using Xamarin.Forms.Platform.UWP;
using System;
using System.Diagnostics;

[assembly: ExportRenderer(typeof(CustomMap), typeof(CustomMapRenderer))]
namespace Map3DProject.UWP.CustomRenderer
{
    /// <summary>
    /// CustomRenderer for the CustomMap created in the PCL part.
    /// This Renderer gives us the possibility to add/override some functionalities.
    /// </summary>
    public class CustomMapRenderer : MapRenderer
    {
        /// <summary>
        /// Instance of the native map for this plateform.
        /// </summary>
        MapControl nativeMap;

        /// <summary>
        /// Instance of our Custom control declared in the PCL part.
        /// </summary>
        CustomMap customMap;

        /// <summary>
        /// We override the OnElementChanged() event handler to get the desired instance. We also use it for updates.
        /// </summary>
        /// <param name="e">It contains either the NewElement or the OldElement</param>
        protected override void OnElementChanged(ElementChangedEventArgs<Map> e)
        {
            base.OnElementChanged(e);

            if (e.NewElement != null)
            {
                customMap = (CustomMap)e.NewElement;
                nativeMap = Control as MapControl;

                nativeMap.Loaded += ((sender, re) =>
                {
                    customMap.MapLoaded();
                });
            }
        }

        /// <summary>
        /// The on element property changed callback.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="PropertyChangedEventArgs"/>Instance containing the event data.</param>
        protected override void OnElementPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);
            if (this.customMap == null || this.nativeMap == null)
                return;

            if (e.PropertyName == CustomMap.ZoomLevelProperty.PropertyName)
                UpdateCameraView();
        }

        private async void UpdateCameraView()
        {
            if (customMap == null || nativeMap == null)
                return;

            if (nativeMap.Is3DSupported && customMap.IsMapLoaded)
            {
                try
                {
                    // Set the aerial 3D view.
                    //nativeMap.Style = MapStyle.Aerial3DWithRoads;

                    // Specify the location.
                    /*BasicGeoposition hwGeoposition = new BasicGeoposition() { Latitude = customMap.VisibleRegion.LatitudeDegrees, Longitude = customMap.VisibleRegion.LongitudeDegrees };
                    Geopoint hwPoint = new Geopoint(hwGeoposition);
                    // Create the map scene.
                    MapScene hwScene = MapScene.CreateFromLocationAndRadius(hwPoint,
                                                                                         80, //show this many meters around
                                                                                         0, //looking at it to the North
                                                                                         60); //degrees pitch*/

                    MapScene hwScene = MapScene.CreateFromCamera(nativeMap.ActualCamera);

                    // Set the 3D view with animation.
                    await nativeMap.TrySetSceneAsync(hwScene, MapAnimationKind.Bow);
                } catch (Exception e)
                {
                    Debug.WriteLine("------------------------");
                    Debug.WriteLine(e.ToString());
                    Debug.WriteLine(e.StackTrace);
                    Debug.WriteLine("------------------------");
                }
            }
            else
            {
                // If 3D views are not supported, display dialog.
                ContentDialog viewNotSupportedDialog = new ContentDialog()
                {
                    Title = "3D is not supported",
                    Content = "\n3D views are not supported on this device.",
                    PrimaryButtonText = "OK"
                };
                await viewNotSupportedDialog.ShowAsync();
            }
        }
    }
}
