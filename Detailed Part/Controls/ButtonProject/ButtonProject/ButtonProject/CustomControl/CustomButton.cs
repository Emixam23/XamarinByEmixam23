using System;
using Xamarin.Forms;

namespace ButtonProject.CustomControl
{
    public class CustomButton : Button
    {
        public event EventHandler LongPress;

        public void OnLongPress()
        {
            if (LongPress != null)
            {
                LongPress(this, new EventArgs());
            }
        }
    }
}
