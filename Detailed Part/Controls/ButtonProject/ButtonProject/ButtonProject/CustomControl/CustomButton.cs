using System;
using System.Diagnostics;
using Xamarin.Forms;

namespace ButtonProject.CustomControl
{
    public class CustomButton : Button
    {
        public static readonly BindableProperty LongPressCallbackProperty = 
            BindableProperty.Create(nameof(LongPressCallback), typeof(CustomButton), typeof(Action<object>), null);
        public Action<object> LongPressCallback
        {
            get { return (Action<object>)GetValue(LongPressCallbackProperty); }
            set { SetValue(LongPressCallbackProperty, value); }
        }

        public void test()
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                Debug.WriteLine("Should work no?");
            });
        }
    }
}
