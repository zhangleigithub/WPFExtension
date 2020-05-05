using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WPFExtension
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// 
        /// </summary>
        public double DataLength { get; set; }

        public MainWindow()
        {
            InitializeComponent();

            this.DataContext = this;
            this.DataLength = 1000;

            for (int i = 0; i < 2000; i++)
            {
                DrawingLine(new Point(100 + i * 10, 100 + i * 10), new Point(300 + i * 10, 200 + i * 10));
                DrawingLine(new Point(100 + i * 10, 200 + i * 10), new Point(300 + i * 10, 100 + i * 10));
            }

            //GeometryGroup group1 = new GeometryGroup();
            //var g1 = GetSinGeometry(1, 100);
            //group1.Children.Add(g1);
            //path2.Data = group1;
            ////path2.Fill = Brushes.White;
            //path2.Stroke = Brushes.White;

            this.canvas1.NeedData += Canvas1_NeedData;
        }

        int i = 1;

        private void Canvas1_NeedData(object sender, Rect e, List<UIElement> elements)
        {
            Label txt = new Label();
            txt.Margin = new Thickness(400 + i * 50, 300, 0, 0);
            txt.Content = i;
            txt.Foreground = Brushes.White;
            txt.BorderBrush = Brushes.Black;
            txt.BorderThickness = new Thickness(0, 0, 0, 1);
            elements.Add(txt);
            i++;
        }

        public StreamGeometry GetSinGeometry(int dx, int dy)
        {
            StreamGeometry g = new StreamGeometry();
            using (StreamGeometryContext ctx = g.Open())
            {
                int x0 = 360;
                double y0 = Math.Sin(-x0 * Math.PI / 180.0);
                ctx.BeginFigure(new Point(-x0, dy * y0), false, false);
                for (int x = -x0; x < x0; x += dx)
                {
                    double y = Math.Sin(x * Math.PI / 180.0);
                    ctx.LineTo(new Point(x, dy * y), true, true);
                }
            }
            g.Freeze();
            return g;
        }

        protected void DrawingLine(Point startPt, Point endPt)
        {
            LineGeometry myLineGeometry = new LineGeometry();
            myLineGeometry.StartPoint = startPt;
            myLineGeometry.EndPoint = endPt;

            Path myPath = new Path();
            myPath.Stroke = Brushes.White;
            myPath.Fill = Brushes.White;
            myPath.StrokeThickness = 1;
            myPath.Data = myLineGeometry;
            //myPath.Visibility = Visibility.Hidden;
            canvas1.Children.Add(myPath);
        }
    }
}
