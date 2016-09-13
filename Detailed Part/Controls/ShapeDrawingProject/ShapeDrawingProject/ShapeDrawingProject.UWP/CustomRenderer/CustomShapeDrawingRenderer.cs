using ShapeDrawingProject.CustomControl;
using ShapeDrawingProject.UWP.CustomRenderer;
using System;
using System.ComponentModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;
using Xamarin.Forms;
using Xamarin.Forms.Platform.UWP;
using static ShapeDrawingProject.CustomControl.CustomShapeDrawing;

[assembly: ExportRenderer(typeof(CustomShapeDrawing), typeof(CustomShapeDrawingRenderer))]
namespace ShapeDrawingProject.UWP.CustomRenderer
{
    public class CustomShapeDrawingRenderer : ViewRenderer<CustomShapeDrawing, Polygon>
    {
        CustomShapeDrawing customShapeDrawing;

        public CustomShapeDrawingRenderer()
        {
            SizeChanged += CustomShapeDrawingRenderer_SizeChanged;
        }

        protected override void OnElementChanged(ElementChangedEventArgs<CustomShapeDrawing> e)
        {
            base.OnElementChanged(e);

            if (e.NewElement != null)
            {
                customShapeDrawing = e.NewElement as CustomShapeDrawing;
                DrawCustomShapeDrawing();
            }
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            if (e.PropertyName == CustomShapeDrawing.BorderColorProperty.PropertyName)
                DrawCustomShapeDrawing();
        }

        private void CustomShapeDrawingRenderer_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            DrawCustomShapeDrawing();
        }

        private void DrawCustomShapeDrawing()
        {
            if (customShapeDrawing != null)
            {
                var poly = new Polygon();

                poly.DataContext = Element;

                poly.Fill = (SolidColorBrush)new ColorConverter().Convert(customShapeDrawing.ShapeColor, null, null, null);
                poly.Stroke = (SolidColorBrush)new ColorConverter().Convert(customShapeDrawing.BorderColor, null, null, null);

                poly.Points = new PointCollection();

                Xamarin.Forms.Rectangle rect = new Xamarin.Forms.Rectangle(0, 0, customShapeDrawing.Width, customShapeDrawing.Height);

                rect.Size = new Size(customShapeDrawing.Width, customShapeDrawing.Height);
                foreach (XYCoordinate xy in customShapeDrawing.PointsShapeCoordinateList)
                {
                    poly.Points.Add(new Windows.Foundation.Point(rect.Width * xy.X, rect.Height * xy.Y));
                }
                poly.FillRule = FillRule.EvenOdd;

                SetNativeControl(poly);
            }
        }
    }
}
