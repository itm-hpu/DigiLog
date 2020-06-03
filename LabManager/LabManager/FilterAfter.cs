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
            //distribution.ChartAreas[0].AxisX.Enabled = AxisEnabled.False;
            //distribution.ChartAreas[0].AxisY.Enabled = AxisEnabled.False;

            double AxisXmax = candidatePointsList.Max(r => r.Coordinates.X);
            double AxisXmin = candidatePointsList.Min(r => r.Coordinates.X);
            double AxisYmax = candidatePointsList.Max(r => r.Coordinates.Y);
            double AxisYmin = candidatePointsList.Min(r => r.Coordinates.Y);

            distribution.ChartAreas[0].AxisX.Maximum = AxisXmax;
            distribution.ChartAreas[0].AxisX.Minimum = AxisXmin;
            distribution.ChartAreas[0].AxisY.Maximum = AxisYmax;
            distribution.ChartAreas[0].AxisY.Minimum = AxisYmin;

            for (int i = 0; i < candidatePointsList.Count(); i++)
            {
                distribution.Series["Series1"].Points.AddXY(candidatePointsList[i].Coordinates.X, candidatePointsList[i].Coordinates.Y);
            }
        }
    }
}
