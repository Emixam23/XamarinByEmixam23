using MapUIOptionsProject.CustomControl;
using MapUIOptionsProject.UWP.CustomRenderer;
using System.ComponentModel;
using Windows.UI.Xaml.Controls.Maps;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Maps.UWP;
using Xamarin.Forms.Platform.UWP;

[assembly: ExportRenderer(typeof(CustomMap), typeof(CustomMapRenderer))]
namespace MapUIOptionsProject.UWP.CustomRenderer
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
                UpdateUIOptions();
            }
        }

        /// <summary>
        /// The on element property changed callback.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="PropertyChangedEventArgs"/>Instance containing the event data.</param>
        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);
            if (this.Element == null || this.Control == null)
                return;

            if (e.PropertyName == CustomMap.IsUIOptionsEnableProperty.PropertyName)
                UpdateUIOptions();
        }

        /// <summary>
        /// Show or Hide the UI options of the Map.
        /// </summary>
        private void UpdateUIOptions()
        {
            if (customMap.IsUIOptionsEnable)
                Control.ZoomInteractionMode = MapInteractionMode.Auto;
            else
                Control.ZoomInteractionMode = MapInteractionMode.GestureOnly;
        }
    }
}
