using ButtonProject.CustomControl;
using ButtonProject.UWP.CustomRenderer;
using System.Diagnostics;
using Windows.UI.Xaml.Input;
using Xamarin.Forms;
using Xamarin.Forms.Platform.UWP;

[assembly: ExportRenderer(typeof(CustomButton), typeof(CustomButtonRenderer))]
namespace ButtonProject.UWP.CustomRenderer
{
    internal class CustomButtonRenderer : ButtonRenderer
    {
        CustomButton customButton;

        protected override void OnElementChanged(ElementChangedEventArgs<Button> e)
        {
            base.OnElementChanged(e);

            if (e.NewElement != null)
            {
                customButton = e.NewElement as CustomButton;
                Control.Holding += OnHold;
                Debug.WriteLine("Hey !");
            }
        }

        private bool isHolding = false;
        private void OnHold(object sender, HoldingRoutedEventArgs e)
        {
            if (!isHolding)
            {
                customButton.LongPressCallback(sender);
                isHolding = true;
            }
            else
                isHolding = false;
            //Debug test
            customButton.test();
        }
    }
}
