Hi!

I made this project to be able to set the style of the map, set the tiles of the map. I wrote this tutorial to help you to use my code and/or include it into your project !

First, for Android and Windows, you need a key (not iOS).

## Android -> https://console.developers.google.com/apis/credentials

The MainActivity.cs file has to look like it, you have to add `Xamarin.FormsMaps.Init(this, bundle);`

    [Activity(Label = "MapTileProject", Icon = "@drawable/icon", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsApplicationActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            global::Xamarin.Forms.Forms.Init(this, bundle);
            Xamarin.FormsMaps.Init(this, bundle);

            LoadApplication(new App());
        }
    }

However, you also need to add the key you got from the **[link](https://console.developers.google.com/apis/credentials)** in the title. You add this into => *Project.Droid/Properties/AndroidManifest.xml*

    <?xml version="1.0" encoding="utf-8"?>
    <manifest xmlns:android="http://schemas.android.com/apk/res/android" android:installLocation="auto">
	<uses-sdk android:minSdkVersion="15" />
	<uses-permission android:name="android.permission.ACCESS_FINE_LOCATION" />
	<uses-permission android:name="android.permission.ACCESS_COARSE_LOCATION" />
	<uses-permission android:name="android.permission.ACCESS_LOCATION_EXTRA_COMMANDS" />
	<uses-permission android:name="android.permission.ACCESS_MOCK_LOCATION" />
	<uses-permission android:name="android.permission.ACCESS_NETWORK_STATE" />
	<uses-permission android:name="android.permission.ACCESS_WIFI_STATE" />
	<uses-permission android:name="android.permission.INTERNET" />
	<application android:label="$safeprojectname$">
        <meta-data android:name="com.google.android.maps.v2.API_KEY" android:value="YOUR_API_KEY" />
    </application>
    </manifest>

**Replace the "YOUR_API_KEY"  value by your api key ! it could be something like this: `a12b3c4d5eyou_api_keyplop42`**

## iOS

You need to set the AppDelegate.cs like that:

     [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {
        //
        // This method is invoked when the application has loaded and is ready to run. In this 
        // method you should instantiate the window, load the UI into it and then make the window
        // visible.
        //
        // You have 17 seconds to return from this method, or iOS will terminate your application.
        //
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            global::Xamarin.Forms.Forms.Init();
            Xamarin.FormsMaps.Init();

            LoadApplication(new App());

            return base.FinishedLaunching(app, options);
        }
    }

Also, you need to add two things to the Infos.plist file. (root file in Project.iOS)

    <key>NSLocationAlwaysUsageDescription</key>
        <string>Can we use your location</string>
    <key>NSLocationWhenInUseUsageDescription</key>
        <string>We are using your location</string>

**Note:** Of course, to add these  both line, you have to open the Infos.plist by **Right-Click** ! Open With -> XML (Text) Editor. Put these four lines at the end as I do :

    <?xml version="1.0" encoding="UTF-8"?>
    <!DOCTYPE plist PUBLIC "-//Apple//DTD PLIST 1.0//EN" "http://www.apple.com/DTDs/PropertyList-1.0.dtd">
    <plist version="1.0">
        <dict>
             .....
             .....
            </array>
            <key>UILaunchStoryboardName</key>
            <string>LaunchScreen</string>
            <key>NSLocationAlwaysUsageDescription</key>
            <string>Can we use your location</string>
            <key>NSLocationWhenInUseUsageDescription</key>
            <string>We are using your location</string>
        </dict>
    </plist>


## UWP -> https://www.bingmapsportal.com/Application

For UWP, as the others, you need to edit the MainPage.xaml.cs **/!\ in the Project.UWP, not in the PCL (shared) part !** The file should looks like it:

    public sealed partial class MainPage
    {
        public MainPage()
        {
            this.InitializeComponent();
            FormsMaps.Init("YOUR_BING_KEY");
            LoadApplication(new MapTileProject.App());
        }
    }

**Note:** This is here that you have to put your bing map key ! You can get one key from this **[Link](https://www.bingmapsportal.com/Application)**

---------

Once you did all of it, it should be good ! Now, do not  forget that, to use my code in order to set the tiles of your map, you need to re-create a CustomMap object as I did (in *Project/CustomControl/*) and then, re-create a renderer for each platform you're focusing ! (Each renderers are put in CustomControlRenderer folder (Droid, iOS, UWP)).

Now, for your custom aspect, you need to design it from website that propose it, I use [MapBox](www.mapbox.com). To use your custom set, you need to get a link like the following: 

[https://api.mapbox.com/styles/v1/apseudo/aprivatetoken/tiles/256/{z}/{x}/{y}?access_token=abcdefghijklmnopqrstuvwxyz0123456789](https://api.mapbox.com/styles/v1/apseudo/aprivatetoken/tiles/256/{z}/{x}/{y}?access_token=abcdefghijklmnopqrstuvwxyz0123456789)

**Note:** This link, for mapbox uses, can be find in style menu. You have a list of map (or you create one), you click at the right of your created map, on the right button of **[EDIT]**. You should see a *</> Share, develop & use* option. Click on it. A list of option appears on your screen and *Develop with this style* is what we are looking for. Two options are given, on top of *Style URL:*, [Mapbox] or [Leaflet], select **Leaflet**. The link given is what you need, so copy it !

When you want to use a map and edit the tile with my code, then declare a map in xaml and add this link for the `MapTileTemplate`.

    <?xml version="1.0" encoding="utf-8" ?>
    <ContentPage xmlns="http://xamarin.com/schemas/2014/forms"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             xmlns:local="clr-namespace:MapTileProject.CustomControl;assembly=MapTileProject"
             x:Class="MapTileProject.Page.MainPage">
    
      <ContentPage.Content>
    
        <!-- 
        Here we put the map. 
        Do not forget to add 'xmlns:local':
    
        -The clr-namespace attribute is the namespace of your Custom object (PCL part).
        -Generally, the assembly is the name of your project.
        -->

        <local:CustomMap MapTileTemplate="https://api.mapbox.com/styles/v1/apseudo/aprivatetoken/tiles/256/{z}/{x}/{y}?access_token=abcdefghijklmnopqrstuvwxyz0123456789"
                     VerticalOptions="Fill" HorizontalOptions="Fill"/>
    
      </ContentPage.Content>

    </ContentPage>

------

**Note:** If you use another website which provides tile set, it's ok, but you have to search for a link which contains a `{z}/{x}/{y}` element, like in the link for my example !

**Note:** Take a look at the [Adding a Map in Xamarin.Forms Tutorial](https://developer.xamarin.com/guides/xamarin-forms/user-interface/map/)

------

I hope I'm clear and it helps you :)