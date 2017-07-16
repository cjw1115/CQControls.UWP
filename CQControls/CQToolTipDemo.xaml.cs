using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace CQControls
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>

    public sealed partial class CQToolTipDemo : Page
    {
        public CQToolTipDemo()
        {
            this.InitializeComponent();

            InitData();
        }

        public void InitData()
        {

            var horizentalNames = Enum.GetNames(typeof(HorizontalAlignment));
            foreach (var item in horizentalNames)
            {
                this.horizentalList.Items.Add(item);
            }

            var verticalNames = Enum.GetNames(typeof(VerticalAlignment));
            foreach (var item in verticalNames)
            {
                this.verticalList.Items.Add(item);
            }
        }

        private void horizentalList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var name = e.AddedItems[0] as string;
            var value = (HorizontalAlignment)Enum.Parse(typeof(HorizontalAlignment), name);
            cqtooltip.HorizontalAlignment = value;
        }

        private void verticalList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var name = e.AddedItems[0] as string;
            var value = (VerticalAlignment)Enum.Parse(typeof(VerticalAlignment), name);
            cqtooltip.VerticalAlignment = value;
        }


        public void MakePath()
        {
        }

        private void Grid_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            var grid = sender as Grid;
            //Windows.UI.Xaml.Shapes.Path
            var width = grid.ActualWidth+20;
            var height = 3*grid.ActualHeight/2;

            
            var root = VisualTreeHelper.GetParent(grid) as Grid;
            root.Height = height;
            root.Width = width;
            var path = Helpers.VisualTreeHelperTool.FindNamedVisualChild<Windows.UI.Xaml.Shapes.Path>(root, "borderPath");

            var geometry = new PathGeometry();
            geometry.Figures = new PathFigureCollection();

            var arrowHeight = 6;
            grid.Margin = new Thickness(grid.Margin.Left, grid.Margin.Top, grid.Margin.Right, arrowHeight);

            var figure = new PathFigure();
            figure.Segments = new PathSegmentCollection();
            figure.StartPoint = new Point(10, 0);

            figure.Segments.Add(new LineSegment() { Point = new Point(width - 10, 0) });
            figure.Segments.Add(new ArcSegment() { Point = new Point(width , 10),SweepDirection= SweepDirection.Clockwise, Size=new Size(10,10) });

            figure.Segments.Add(new LineSegment() { Point = new Point(width , height-10 - arrowHeight) });

            figure.Segments.Add(new ArcSegment() { Point = new Point(width-10, height- arrowHeight), SweepDirection = SweepDirection.Clockwise, Size = new Size(10, 10) });

            figure.Segments.Add(new LineSegment() { Point = new Point(width / 2 + arrowHeight, height - arrowHeight) });
            figure.Segments.Add(new LineSegment() { Point = new Point(width / 2, height) });
            figure.Segments.Add(new LineSegment() { Point = new Point(width / 2 - arrowHeight, height - arrowHeight) });


            figure.Segments.Add(new LineSegment() { Point = new Point(10, height- arrowHeight) });

            figure.Segments.Add(new ArcSegment() { Point = new Point(0, height-10- arrowHeight), SweepDirection = SweepDirection.Clockwise, Size = new Size(10, 10) });

            figure.Segments.Add(new LineSegment() { Point = new Point(0, 10) });

            figure.Segments.Add(new ArcSegment() { Point = new Point(10, 0), SweepDirection = SweepDirection.Clockwise, Size = new Size(10, 10) });

            geometry.Figures.Add(figure);

            path.Data = geometry;
        }
    }
}
