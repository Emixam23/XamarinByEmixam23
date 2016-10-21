Hi!

I made this project to be able to handle long press click of Phone and Tablet, but also to handle Right click of Desktop uses. I wrote this tutorial to help you to use my code and/or include it into your project !

## Required Packages

- Xamarin.Forms

## Button Project

    List of Options:

        - Desktop: Generation of events if a right click is make on this Button.
        - Phone/Tablet : Generation of events if a long press is make on this Button.

    Platform Rendering:

        - Android (KO)
        - iOS (KO)
        - UWP (OK)

----

First, do not  forget that, to use my code, in order to set the tiles of your map, you will need to create a CustomButton object as I did (in *Project/CustomControl/*) and then, create a renderer for each platform you're focusing ! (Each renderers are put in CustomControlRenderer folder (Droid, iOS, UWP)).

Steps to make it works :

    - Create a CustomButton object :

    public class CustomButton : Button
    {
        // The object is Xamarin.Forms.Button..
        // Copy all of the code from the CustomButton.cs object in the Project/CustomControl/ directory
    }
    
Your button is made ! However, nothing will happens..

   - Create a Renderer for each platform :

I coded/developed one `Renderer` by platform, each renderer is available in Project/TARGET_PLATFORM/CustomRenderer directory.

**Note :** When you're copying my code, don't forget the `[assembly: ExportRenderer(typeof(CustomMap), typeof(CustomMapRenderer))]` on the top of each CustomRenderer declaration. If you don't refer it in assembly, then nothing will happens !

----

When you want to use the button for long press or right click, declare a button in xaml and add a method to the `LongPress ` property.

    <?xml version="1.0" encoding="utf-8" ?>
    <ContentPage xmlns="http://xamarin.com/schemas/2014/forms"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             xmlns:control="clr-namespace:ButtonProject.CustomControl;assembly=ButtonProject"
             x:Class="ButtonProject.Page.MainPage">
    
      <ContentPage.Content>
    
        <!-- 
        Here we put the button 
        Do not forget to add 'xmlns:control':
    
        -The clr-namespace attribute is the namespace of your Custom object (PCL part).
        -Generally, the assembly is the name of your project.
        -->

        <control:CustomButton VerticalOptions="Center" HorizontalOptions="Center"
                                              Clicked="OnCustomButtonClicked" LongPress="OnCustomButtonLongPress"/>/>
    
      </ContentPage.Content>

    </ContentPage>

Now the part of the MainPage.xaml.cs, the C# part.

    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private void OnCustomButtonLongPress(object sender, EventArgs ea)
        {
            Debug.WriteLine("Button Long Pressed and/or Right Click got !");
        }

        private void OnCustomButtonClicked(object sender, EventArgs ea)
        {
            Debug.WriteLine("Button clicked !");
        }
    }

------

I hope I'm clear and it helps you :)