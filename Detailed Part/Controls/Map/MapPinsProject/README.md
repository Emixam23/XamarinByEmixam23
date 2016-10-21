Hi!

I made this project to be able to work with pins by lot of functionnalities. I wrote this tutorial to help you to use my code and/or include it into your project !

## Required Packages

- Newtonsoft.Json
- Xamarin.Forms
- Xamarin.Forms.Map

## Map Pins Project

    List of Options:

        -

    Platform Rendering:

        - CROSS_PLATFORM / PCL (Work in Progress)

        - Android (WIP)
        - iOS (KO)
        - UWP (WIP)

----

First, do not  forget that, to use my code, in order to add custom pins to your map, you will need to create a CustomMap object as I did (in *Project/CustomControl/*) and then, create a renderer for each platform you're focusing ! (Each renderers are put in CustomControlRenderer folder (Droid, iOS, UWP)).

Steps to make it works :

    - Create a CustomMap object :

    public class CustomMap : Map
    {
        // The object is Xamarin.Forms.Map..
        // Copy all of the code from the CustomMap.cs object in the Project/CustomControl/ directory
    }
    
Your map is made ! However, nothing will happens..

   - Create a Renderer for each platform :

I coded/developed one `Renderer` by platform, each renderer is available in Project/TARGET_PLATFORM/CustomRenderer directory.

**Note :** When you're copying my code, don't forget the `[assembly: ExportRenderer(typeof(CustomMap), typeof(CustomMapRenderer))]` on the top of each CustomRenderer declaration. If you don't refer it in assembly, then nothing will happens !

----

.................................................

    <?xml version="1.0" encoding="utf-8" ?>
    <ContentPage xmlns="http://xamarin.com/schemas/2014/forms"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             xmlns:control="clr-namespace:MapPinsProject.CustomControl;assembly=MapPinsProject"
             x:Class="MapPinsProject.Page.MainPage">
    
      <ContentPage.Content>
    
        <!-- 
        Here we put the map. 
        Do not forget to add 'xmlns:control':
    
        -The clr-namespace attribute is the namespace of your Custom object (PCL part).
        -Generally, the assembly is the name of your project.
        -->

        <control:CustomMap 
                                      VerticalOptions="Fill" HorizontalOptions="Fill"/>
    
      </ContentPage.Content>

    </ContentPage>

Here is a example of the MainPage.xaml.cs, the C# part.

    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            BindingContext = this;
            InitializeComponent();
        }
    }

Now, the last thing you will need to do is to update your `App.cs` file, located in the PCL part.

    public class App : Application
    {
        public static readonly string GOOGLE_MAP_API_KEY = "YOUR_API_KEY"; // If you don't know how to get the API key, take a look at the Map root folder, you have a README.md

        public App()
        {
            // The root page of your application
            MainPage = new MainPage();
        }
    }

------

I hope I'm clear and it helps you :)