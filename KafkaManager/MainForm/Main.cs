using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Media;

using KafkaNet;
using KafkaNet.Model;

using DigiLogKafka;

using LiveCharts;
using LiveCharts.Wpf;

namespace MainForm
{
    public partial class Main : Form
    {
        Thread tConsumer;

        public Main()
        {
            InitializeComponent();
            TextBox.CheckForIllegalCrossThreadCalls = false;
            CheckServerOption();

            //chart
            cartesianChart1.Series = new SeriesCollection
            {
                new LineSeries
                {
                    Title = "Series 1",
                    Values = new ChartValues<double> {4, 6, 5, 2, 7}
                },
                new LineSeries
                {
                    Title = "Series 2",
                    Values = new ChartValues<double> {6, 7, 3, 4, 6},
                    PointGeometry = null
                },
                new LineSeries
                {
                    Title = "Series 2",
                    Values = new ChartValues<double> {5, 2, 8, 3},
                    PointGeometry = DefaultGeometries.Square,
                    PointGeometrySize = 15
                }
            };
            cartesianChart1.AxisX.Add(new Axis
            {
                Title = "Month",
                Labels = new[] { "Jan", "Feb", "Mar", "Apr", "May" }
            });
            cartesianChart1.AxisY.Add(new Axis
            {
                Title = "Sales",
                LabelFormatter = value => value.ToString("C")
            });
            cartesianChart1.LegendLocation = LegendLocation.Right;
            // modifying the series collection will animate and update the chart
            cartesianChart1.Series.Add(new LineSeries
            {
                Values = new ChartValues<double> { 5, 3, 2, 4, 5 },
                LineSmoothness = 0, //straight lines, 1 really smooth lines
                PointGeometry = Geometry.Parse("m 25 70.36218 20 -28 -20 22 -8 -6 z"),
                PointGeometrySize = 50,
                PointForeground = System.Windows.Media.Brushes.Gray
            });
            //modifying any series values will also animate and update the chart
            cartesianChart1.Series[2].Values.Add(5d);

            cartesianChart1.DataClick += CartesianChart1OnDataClick;

            //Pie chart
            Func<ChartPoint, string> labelPoint = chartPoint =>
                string.Format("{0} ({1:P})", chartPoint.Y, chartPoint.Participation);
            pieChart1.Series = new SeriesCollection
            {
                new PieSeries
                {
                    Title = "Maria",
                    Values = new ChartValues<double> {3},
                    PushOut = 15,
                    DataLabels = true,
                    LabelPoint = labelPoint
                },
                new PieSeries
                {
                    Title = "Charles",
                    Values = new ChartValues<double> {4},
                    DataLabels = true,
                    LabelPoint = labelPoint
                },
                new PieSeries
                {
                    Title = "Frida",
                    Values = new ChartValues<double> {6},
                    DataLabels = true,
                    LabelPoint = labelPoint
                },
                new PieSeries
                {
                    Title = "Frederic",
                    Values = new ChartValues<double> {2},
                    DataLabels = true,
                    LabelPoint = labelPoint
                }
            };

            pieChart1.LegendLocation = LegendLocation.Bottom;

            
            pieChart2.Series = new SeriesCollection
            {
                new PieSeries
                {
                    Title = "Maria",
                    Values = new ChartValues<double> {3},
                    PushOut = 15,
                    DataLabels = true,
                    LabelPoint = labelPoint
                },
                new PieSeries
                {
                    Title = "Charles",
                    Values = new ChartValues<double> {4},
                    DataLabels = true,
                    LabelPoint = labelPoint
                },
                new PieSeries
                {
                    Title = "Frida",
                    Values = new ChartValues<double> {6},
                    DataLabels = true,
                    LabelPoint = labelPoint
                },
                new PieSeries
                {
                    Title = "Frederic",
                    Values = new ChartValues<double> {2},
                    DataLabels = true,
                    LabelPoint = labelPoint
                }
            };

            pieChart2.LegendLocation = LegendLocation.Bottom;
        }

        private void CartesianChart1OnDataClick(object sender, ChartPoint chartPoint)
        {
            MessageBox.Show("You clicked (" + chartPoint.X + "," + chartPoint.Y + ")");
        }    

    private void btnSend_Click(object sender, EventArgs e)
        {
            Manager mMnanager = new Manager();

            Thread t = new Thread(() => { mMnanager.SendMessage(txtPServer.Text,txtPTopic.Text,txtPMessage.Text); });
            t.Start();
        }

        private void CheckServerOption()
        {
            if (chbConsumer.Checked == true)
            {
                txtCServer.Text = txtPServer.Text;
                txtCTopic.Text = txtPTopic.Text;

                txtCServer.Enabled = false;
                txtCTopic.Enabled = false;
            }
            else if (chbConsumer.Checked == false)
            {
                txtCServer.Text = "";
                txtCTopic.Text = "";

                txtCServer.Enabled = true;
                txtCTopic.Enabled = true;
            }
        }

        private void chbConsumer_CheckedChanged(object sender, EventArgs e)
        {
            CheckServerOption();
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            Manager mMnanager = new Manager();
            
            tConsumer = new Thread(() => 
            {
                IEnumerable<KafkaNet.Protocol.Message> messages = mMnanager.GetMessage(txtCServer.Text, txtCTopic.Text);

                foreach (var message in messages)
                {
                    if (txtCMessage.Text.Length > 0)
                    {
                        txtCMessage.AppendText(Environment.NewLine);
                    }
                    txtCMessage.AppendText(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffffff") + ", " + Encoding.UTF8.GetString(message.Value));                    
                }
            });
            tConsumer.Start();
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            tConsumer.Abort();
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            txtCMessage.Text = "";
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            Manager mManager = new Manager();
            string messages = txtCMessage.Text;

            mManager.ExportCSV(messages);

        }

        private void btnEnd_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnGetData_Click(object sender, EventArgs e)
        {

        }



    }
}
