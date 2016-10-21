Hi!

I made this project to be able to show/hide the UI options of the map, but also the possibility of get the current user's location. I wrote this tutorial to help you to use my code and/or include it into your project !

## Required Packages

- Xamarin.Forms
- Xamarin.Forms.Map
- Xam.Plugin.Geolocator

## Map UIOptions Project

    List of Options:

        - Show/Hide UI options
        - Get the current location of the user as Position

    Platform Rendering:

        - Android (OK)
        - iOS (OK)
        - UWP (OK)

----

First, do not  forget that, to use my code, in order to show/hide the UI components of your map, or to get the current user's location, you will need to create a CustomMap object as I did (in *Project/CustomControl/*) and then, create a renderer for each platform you're focusing ! (Each renderers are put in CustomControlRenderer folder (Droid, iOS, UWP)).

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

When you want to use a map and hide UI buttons with my code, just set the `IsUIOptionsEnable` property of the map.

    <?xml version="1.0" encoding="utf-8" ?>
    <ContentPage xmlns="http://xamarin.com/schemas/2014/forms"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             xmlns:control="clr-namespace:MapUIOptionsProject.CustomControl;assembly=MapUIOptionsProject"
             x:Class="MapUIOptionsProject.Page.MainPage">
    
      <ContentPage.Content>
    
        <!-- 
        Here we put the map. 
        Do not forget to add 'xmlns:control':
    
        -The clr-namespace attribute is the namespace of your Custom object (PCL part).
        -Generally, the assembly is the name of your project.
        -->

        <control:CustomMap IsUIOptionsEnable="False"
                                      VerticalOptions="Fill" HorizontalOptions="Fill"/>
    
      </ContentPage.Content>

    </ContentPage>

Here is a example of the MainPage.xaml.cs, the C# part.

    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }
    }

**Note :** If you want to get the current location, then just call the GetPosition() of the map object. Also, to use this functionnality, you'll need to add the `Location` permission to your app. Basically, everything should work if you red the root tutorial about **Using Xamarin Forms Map**, however it's not mentionned that for UWP, you need to add this permission. To add it, right-click on the **Package.appxmanifest** of the UWP part. Then, by clicking on `Capabilities`, you should find the **Location** one.

------

I hope I'm clear and it helps you :)