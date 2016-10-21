using ButtonProject.CustomControl;
using ButtonProject.UWP.CustomRenderer;
using Windows.UI.Xaml.Input;
using Xamarin.Forms;
using Xamarin.Forms.Platform.UWP;

[assembly: ExportRenderer(typeof(CustomButton), typeof(CustomButtonRenderer))]
namespace ButtonProject.UWP.CustomRenderer
{
    /// <summary>
    /// CustomRenderer for the CustomButton created in the PCL part.
    /// This Renderer gives us the possibility to add/override some functionalities.
    /// </summary>
    public class CustomButtonRenderer : ButtonRenderer
    {
        /// <summary>
        /// Instance of our Custom control declared in the PCL part.
        /// </summary>
        CustomButton customButton;

        /// <summary>
        /// isHolding is a boolean which disable the double event generation for long press, but also the click after the release of the press.
        /// </summary>
        private bool isHolding;

        /// <summary>
        /// We override the OnElementChanged() event handler to get the desired instance. We also use it for updates.
        /// </summary>
        /// <param name="e">It contains either the NewElement or the OldElement</param>
        protected override void OnElementChanged(ElementChangedEventArgs<Button> e)
        {
            base.OnElementChanged(e);

            if (e.NewElement != null)
            {
                customButton = e.NewElement as CustomButton;

                isHolding = false;

                Control.IsHoldingEnabled = true;

                if (Device.Idiom == TargetIdiom.Desktop)
                {
                    Control.RightTapped += RightClickOverride;
                }
                else if (Device.Idiom == TargetIdiom.Phone || Device.Idiom == TargetIdiom.Tablet)
                {
                    Control.Holding += HoldingOverride;
                }
            }
        }

        /// <summary>
        /// Because on Desktop you can't press, it doesn't exist, then you'll have to use RightMouseClick. This function handle it.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="PropertyChangedEventArgs"/>Instance containing the event data.</param>
        private void RightClickOverride(object sender, RightTappedRoutedEventArgs e)
        {
            customButton.OnLongPress();
        }

        /// <summary>
        /// For Phone & Tablet, this method is call for every long press action.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="PropertyChangedEventArgs"/>Instance containing the event data.</param>
        private void HoldingOverride(object sender, HoldingRoutedEventArgs e)
        {
            if (!isHolding)
            {
                isHolding = true;
                customButton.IsEnabled = false;
                customButton.OnLongPress();
            }
            else
            {
                isHolding = false;
                customButton.IsEnabled = true;
            }
        }
    }
}
