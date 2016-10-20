using ButtonProject.CustomControl;
using ButtonProject.UWP.CustomRenderer;
using System;
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

        private bool isHolding;
        protected override void OnElementChanged(ElementChangedEventArgs<Button> e)
        {
            base.OnElementChanged(e);

            if (e.NewElement != null)
            {
                customButton = e.NewElement as CustomButton;

                isHolding = false;

                Control.IsHoldingEnabled = true;
                Control.Holding += (s, ea) =>
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
                };
            }
        }
    }
}
