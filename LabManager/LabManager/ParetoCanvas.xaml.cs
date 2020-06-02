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
using System.Windows.Shapes;

namespace LabManager
{
    /// <summary>
    /// Interaction logic for PostProcessing.xaml
    /// </summary>
    public partial class ParetoCanvas : Window
    {
        public ParetoCanvas(IList<MainWindow.distancePoint> distancePointsList)
        {
            InitializeComponent();

            int dotsize = 7;
            Ellipse dotOrigin = new Ellipse();
            dotOrigin.Stroke = new SolidColorBrush(Colors.Black);
            dotOrigin.StrokeThickness = 3;
            Canvas.SetZIndex(dotOrigin, 3);
            dotOrigin.Height = dotsize;
            dotOrigin.Width = dotsize;
            dotOrigin.Fill = new SolidColorBrush(Colors.Black);
            dotOrigin.Margin = new Thickness(0, 0, 0, 0);
            paretoCanvas.Children.Add(dotOrigin);
            
            for (int i = 0; i < distancePointsList.Count(); i++)
            {
                int dotSize = 3;
                Ellipse currentDot = new Ellipse();
                currentDot.Stroke = new SolidColorBrush(Colors.Green);
                currentDot.StrokeThickness = 3;
                Canvas.SetZIndex(currentDot, 3);
                currentDot.Height = dotSize;
                currentDot.Width = dotSize;
                currentDot.Fill = new SolidColorBrush(Colors.Green);
                currentDot.Margin = new Thickness(distancePointsList[i].Coordinates.X / 3.0, distancePointsList[i].Coordinates.Y / 15.0, 0, 0); // Sets the position.
                paretoCanvas.Children.Add(currentDot);
            }
        }
    }
}
