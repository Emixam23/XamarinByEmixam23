using System.ComponentModel;
using Xamarin.Forms;

namespace ImageCircleProject
{
    public partial class MainPage : ContentPage, INotifyPropertyChanged
    {
        private ImageSource imagePath;
        public ImageSource ImagePath
        {
            get { return this.imagePath; }
            set
            {
                this.imagePath = value;
                OnPropertyChanged();
            }
        }

        public MainPage()
        {
            base.BindingContext = this;
            ImagePath = ImageSource.FromResource("ImageCircleProject.Image.FullBlack.png");
            InitializeComponent();
        }
    }
}
