Hi!

I made this project to be able to set the style of the map, set the tiles of the map. I wrote this tutorial to help you to use my code and/or include it into your project !

## Required Packages

- Xamarin.Forms
- Xamarin.Forms.Map

## Map Tiles Project

    List of Options:

        - Tiles customization

----

First, do not  forget that, to use my code, in order to set the tiles of your map, you will need to create a CustomMap object as I did (in *Project/CustomControl/*) and then, create a renderer for each platform you're focusing ! (Each renderers are put in CustomControlRenderer folder (Droid, iOS, UWP)).

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

Now, for your custom aspect, you need to design a 'style' from a website that propose it, I used [MapBox](www.mapbox.com). To use your custom set, you need to get a link like the following: 
 
[https://api.mapbox.com/styles/v1/apseudo/aprivatetoken/tiles/256/{z}/{x}/{y}?access_token=abcdefghijklmnopqrstuvwxyz0123456789](https://api.mapbox.com/styles/v1/apseudo/aprivatetoken/tiles/256/{z}/{x}/{y}?access_token=abcdefghijklmnopqrstuvwxyz0123456789)

**Note:** This link, for mapbox uses, can be find in style menu. You have a list of map (or you create one), you click at the right of your created map, on the right button of **[EDIT]**. You should see a *</> Share, develop & use* option. Click on it. A list of option appears on your screen and *Develop with this style* is what we are looking for. Two options are given, on top of *Style URL:*, [Mapbox] or [Leaflet], select **Leaflet**. The link given is what you need, so copy it !

When you want to use a map and edit the tile with my code, then declare a map in xaml and add this link to the `MapTileTemplate` property.

    <?xml version="1.0" encoding="utf-8" ?>
    <ContentPage xmlns="http://xamarin.com/schemas/2014/forms"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             xmlns:control="clr-namespace:MapTileProject.CustomControl;assembly=MapTileProject"
             x:Class="MapTileProject.Page.MainPage">
    
      <ContentPage.Content>
    
        <!-- 
        Here we put the map. 
        Do not forget to add 'xmlns:control':
    
        -The clr-namespace attribute is the namespace of your Custom object (PCL part).
        -Generally, the assembly is the name of your project.
        -->

        <control:CustomMap MapTileTemplate="https://api.mapbox.com/styles/v1/apseudo/aprivatetoken/tiles/256/{z}/{x}/{y}?access_token=abcdefghijklmnopqrstuvwxyz0123456789"
                                      VerticalOptions="Fill" HorizontalOptions="Fill"/>
    
      </ContentPage.Content>

    </ContentPage>

------

**Note:** If you use another website which provides tile set, it's ok, but you have to search for a link which contains a `{z}/{x}/{y}` element, like in the link for my example ! (It's a regex)

------

I hope I'm clear and it helps you :)