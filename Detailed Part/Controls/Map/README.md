Hi!

Welcome on the Map part of this repository. Here you have some functionnalities spread into differents projects, so it will be easier to add my solution to your code.

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

----

**Note:** Take a look at the [Adding a Map in Xamarin.Forms Tutorial](https://developer.xamarin.com/guides/xamarin-forms/user-interface/map/) if something doesn't work

----

I hope I'm clear and it helps you :)