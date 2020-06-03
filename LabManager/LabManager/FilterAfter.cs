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
using System.IO;

namespace LabManager
{
    public partial class FilterAfter : Form
    {

        public class CandidatePoint
        {
            public DateTime TimeStamp { get; set; }
            public System.Windows.Point Coordinates { get; set; } // ?
            public string ObjectID { get; set; }
        }

        IList<MainWindow.CandidatePoint> source = new List<MainWindow.CandidatePoint>();


        public FilterAfter(IList<MainWindow.CandidatePoint> candidatePointsList)
        {
            InitializeComponent();
            source = candidatePointsList;
            createDistribution(source);
        }

        public void SaveCandidatesPoints(IList<MainWindow.CandidatePoint> source)
        {
            int iLength = source.Count();

            string filedir = Directory.GetCurrentDirectory();
            filedir = filedir + @"\CandidatesPoints_" + source[0].ObjectID + ".csv";
            StreamWriter file = new StreamWriter(filedir);

            for (int i = 0; i < iLength; i++)
            {
                file.Write((i+1) + "," + "\t" + source[i].ObjectID + "," + source[i].TimeStamp + "," + source[i].Coordinates.X + "," + source[i].Coordinates.Y);
                file.Write("\n");
            }
            file.Close();

            return;
        }

        public void createDistribution(IList<MainWindow.CandidatePoint> source)
        {
            //distribution.ChartAreas[0].AxisX.Enabled = AxisEnabled.False;
            //distribution.ChartAreas[0].AxisY.Enabled = AxisEnabled.False;

            double AxisXmax = source.Max(r => r.Coordinates.X);
            double AxisXmin = source.Min(r => r.Coordinates.X);
            double AxisYmax = source.Max(r => r.Coordinates.Y);
            double AxisYmin = source.Min(r => r.Coordinates.Y);

            distribution.ChartAreas[0].AxisX.Maximum = AxisXmax;
            distribution.ChartAreas[0].AxisX.Minimum = AxisXmin;
            distribution.ChartAreas[0].AxisY.Maximum = AxisYmax;
            distribution.ChartAreas[0].AxisY.Minimum = AxisYmin;

            for (int i = 0; i < source.Count(); i++)
            {
                distribution.Series["Series1"].Points.AddXY(source[i].Coordinates.X, source[i].Coordinates.Y);
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            SaveCandidatesPoints(source);

            string message = "Finish saving candidates points!";
            string caption = "Post-processing";
            System.Windows.MessageBox.Show(message, caption);
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
