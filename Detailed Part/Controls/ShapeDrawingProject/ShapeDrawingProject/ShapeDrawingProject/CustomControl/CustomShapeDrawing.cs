using System;
using System.Collections.Generic;
using System.Diagnostics;
using Xamarin.Forms;

namespace ShapeDrawingProject.CustomControl
{
    public class CustomShapeDrawing : View
    {
        /// <summary>
        /// The BorderColor property.
        /// </summary>
        public static readonly BindableProperty BorderColorProperty =
            BindableProperty.Create(nameof(BorderColor), typeof(Color), typeof(CustomShapeDrawing), Color.Black);

        /// <summary>
        /// Assessor for the BorderColor property.
        /// </summary>
        public Color BorderColor
        {
            get { return (Color)GetValue(BorderColorProperty); }
            set { SetValue(BorderColorProperty, value); }
        }

        /// <summary>
        /// The BorderColor property.
        /// </summary>
        public static readonly BindableProperty ShapeColorProperty =
            BindableProperty.Create(nameof(ShapeColor), typeof(Color), typeof(CustomShapeDrawing), Color.White);

        /// <summary>
        /// Assessor for the BorderColor property.
        /// </summary>
        public Color ShapeColor
        {
            get { return (Color)GetValue(ShapeColorProperty); }
            set { SetValue(ShapeColorProperty, value); }
        }

        /// <summary>
        /// The PointsShapeCoordinate property.
        /// </summary>
        public static readonly BindableProperty PointsShapeCoordinateProperty =
            BindableProperty.Create(nameof(PointsShapeCoordinate), typeof(string), typeof(CustomShapeDrawing), null);

        /// <summary>
        /// Assessor for the PointsShapeCoordinate property.
        /// </summary>
        public string PointsShapeCoordinate
        {
            get { return (string)GetValue(PointsShapeCoordinateProperty); }
            set { SetValue(PointsShapeCoordinateProperty, value); }
        }

        /// <summary>
        /// Return a list of X/Y coordinates from the PointsShapeCoordinate string property.
        /// </summary>
        public List<XYCoordinate> PointsShapeCoordinateList
        {
            get
            {
                return (PointsShapeCoordinateToList(this.PointsShapeCoordinate));
            }
        }

        /// <summary>
        /// Return a list of X/Y coordinates from a string which contains the point as string format.
        /// </summary>
        /// <param name="PointsShapeCoordinate">X/Y coordinate as string format.</param>
        /// <returns>Return a list of X/Y coordinates from the PointsShapeCoordinate string property.</returns>
        public static List<XYCoordinate> PointsShapeCoordinateToList(string PointsShapeCoordinate)
        {
            List<XYCoordinate> Points = new List<XYCoordinate>();

            if (PointsShapeCoordinate != null)
            {
                string[] pointsTab = PointsShapeCoordinate.Split(",".ToCharArray());
                foreach (string coordinate in pointsTab)
                {
                    string[] pointXY = coordinate.Split("/".ToCharArray());
                    XYCoordinate xy = new XYCoordinate();

                    try
                    {
                        xy.X = Convert.ToDouble(pointXY[0]);
                        xy.Y = Convert.ToDouble(pointXY[1]);
                    }
                    catch (FormatException e)
                    {
                        Debug.WriteLine("FormatException: " + e.ToString());
                    }
                    catch (NullReferenceException e)
                    {
                        Debug.WriteLine("NullReferenceException: " + e.ToString());
                    }
                    catch (Exception e)
                    {
                        Debug.WriteLine("Exception: " + e.ToString());
                    }
                    finally
                    {
                        Points.Add(xy);
                    }
                }
            }

            return (Points);
        }

        public class XYCoordinate
        {
            public double X { get; set; }
            public double Y { get; set; }
        }
    }
}
