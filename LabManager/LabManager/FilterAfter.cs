using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace LabManager
{
    public partial class FilterAfter : Form
    {
        public FilterAfter(IList<MainWindow.CandidatePoint> candidatePointsList)
        {
            InitializeComponent();
            createDistribution(candidatePointsList);
        }

        public void createDistribution(IList<MainWindow.CandidatePoint> candidatePointsList)
        {
            /*
            int dotsize = 7;
            Ellipse dotOrigin = new Ellipse();
            dotOrigin.Stroke = new SolidColorBrush(Colors.Black);
            dotOrigin.StrokeThickness = 3;
            Canvas.SetZIndex(dotOrigin, 3);
            dotOrigin.Height = dotsize;
            dotOrigin.Width = dotsize;
            dotOrigin.Fill = new SolidColorBrush(Colors.Black);
            dotOrigin.Margin = new Thickness(0, 0, 0, 0);
            distribution.Series["Series1"].Points.AddXY(dotOrigin.Margin.Left, dotOrigin.Margin.Top);
            */

            distribution.ChartAreas[0].AxisX.Minimum = -870;
            distribution.ChartAreas[0].AxisX.Maximum = 100;
            distribution.ChartAreas[0].AxisY.Minimum = 1000;

            for (int i = 0; i < candidatePointsList.Count(); i++)
            {
                int dotSize = 3;
                Ellipse currentDot = new Ellipse();
                currentDot.Stroke = new SolidColorBrush(Colors.Green);
                currentDot.StrokeThickness = 3;
                Canvas.SetZIndex(currentDot, 3);
                currentDot.Height = dotSize;
                currentDot.Width = dotSize;
                currentDot.Fill = new SolidColorBrush(Colors.Green);
                currentDot.Margin = new Thickness(candidatePointsList[i].Coordinates.X, candidatePointsList[i].Coordinates.Y, 0, 0); // Sets the position.
                distribution.Series["Series1"].Points.AddXY(currentDot.Margin.Left, currentDot.Margin.Top);
            }
        }
    }
}
