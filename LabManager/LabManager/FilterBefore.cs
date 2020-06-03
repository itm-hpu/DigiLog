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
    public partial class FilterBefore : Form
    {
        public FilterBefore(IList<MainWindow.distancePoint> distancePointsList, IList<MainWindow.ParetoFreqTable> freqTable)
        {
            InitializeComponent();
            createDistribution(distancePointsList);
            createPareto(freqTable);
        }

        public void createDistribution(IList<MainWindow.distancePoint> distancePointsList)
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

            distribution.ChartAreas[0].AxisX.Minimum = -1100;
            distribution.ChartAreas[0].AxisX.Maximum = 600;
            distribution.ChartAreas[0].AxisY.Minimum = 1000;

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
                currentDot.Margin = new Thickness(distancePointsList[i].Coordinates.X , distancePointsList[i].Coordinates.Y , 0, 0); // Sets the position.
                distribution.Series["Series1"].Points.AddXY(currentDot.Margin.Left, currentDot.Margin.Top);
            }
        }

        public void createPareto(IList<MainWindow.ParetoFreqTable> freqTable)
        {
            pareto.ChartAreas[0].AxisX.LabelStyle.Interval = 1;

            for (int i = 0; i < freqTable.Count(); i++)
            {
                pareto.Series["Frequency"].Points.AddXY(freqTable[i].Section, freqTable[i].Freq);
                pareto.Series["Percent"].Points.AddXY(freqTable[i].Section, freqTable[i].CumulPercent * 100.0);
            }
        }
    }
}
