using System;
using Xamarin.Forms;

namespace ButtonProject.CustomControl
{
    /// <summary>
    /// CustomButton inherits from Button, which give us the same behavior of a basic Xamarin.Forms.Button with the possibility of add some functionalities.
    /// </summary>
    public class CustomButton : Button
    {
        /// <summary>
        /// Event handler for LongPress of Phone/Tablet uses, but also for Desktop right click.
        /// </summary>
        public event EventHandler LongPress;

        /// <summary>
        /// Method call when the Button is pressed during a while for Phone/Tablet, but also call by Right clicking from the Desktop. 
        /// </summary>
        public void OnLongPress()
        {
            if (LongPress != null)
            {
                LongPress(this, new EventArgs());
            }
        }
    }
}
