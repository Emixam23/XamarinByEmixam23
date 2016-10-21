Hi!

I made this project to be able to create polylines on your map. I wrote this tutorial to help you to use my code and/or include it into your project !

## Required Packages

- Newtonsoft.Json
- Xamarin.Forms
- Xamarin.Forms.Map

## Map Polyline Project

    List of Options:

        - Polyline generation (by `GeoPosition` collection or `string` mail address collection)
        - Polyline options (Width, Color)
        - Camera Focus (None or OnPolyline)

    Platform Rendering:

        - Android (OK)
        - iOS (OK)
        - UWP (OK)

----

First, do not  forget that, to use my code, in order to create polylines on your map, you will need to create a CustomMap object as I did (in *Project/CustomControl/*) and then, create a renderer for each platform you're focusing ! (Each renderers are put in CustomControlRenderer folder (Droid, iOS, UWP)).

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

When you want to use a map and generate polyline with my code, declare a map in xaml and add a collection to the `PolylineAddressPoints` property.

**Note :** *I only bind mail address and it works?*. **Yes because you'll add your GOOGLE_API_KEY in the App.cs, so the map will use it to get information.**

    <?xml version="1.0" encoding="utf-8" ?>
    <ContentPage xmlns="http://xamarin.com/schemas/2014/forms"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             xmlns:control="clr-namespace:MapPolylineProject.CustomControl;assembly=MapPolylineProject"
             x:Class="MapPolylineProject.Page.MainPage">
    
      <ContentPage.Content>
    
        <!-- 
        Here we put the map. 
        Do not forget to add 'xmlns:control':
    
        -The clr-namespace attribute is the namespace of your Custom object (PCL part).
        -Generally, the assembly is the name of your project.
        -->

        <control:CustomMap PolylineAddressPoints="{Binding AddressPointList}"
                                      VerticalOptions="Fill" HorizontalOptions="Fill"/>
    
      </ContentPage.Content>

    </ContentPage>

I use Binding, why? Because it gives you the possibility to update this list and then, invok an event. From that, the map will update automatically by itself.

Here is a example of the MainPage.xaml.cs, the C# part.

    public partial class MainPage : ContentPage
    {
        public List<string> AddressPointList { get; set; }

        public MainPage()
        {
            BindingContext = this;

            AddressPointList = new List<string>()
            {
                "72230 Ruaudin, France",
                "72100 Le Mans, France",
                "77500 Chelles, France"
            };

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