using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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
using System.Net.Http;
using Newtonsoft.Json.Linq;
using System.Net;

namespace LabManager
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        double[] RTLStempResult = new double[3];
        double[] AGVtempResult = new double[3];
        string[] RTLSsubResult = new string[3];
        string[] AGVsubResult = new string[3];
        string RTLSresult = "";
        string AGVresult = "";
        
        List<List<string>> RTLSpos = new List<List<string>>();
        List<List<string>> AGVpos = new List<List<string>>();
        
        public MainWindow()
        {
            InitializeComponent();
        }

        private double[] AGVRequestToServer()
        {
            double[] responseResult = new double[3];

            restClient rClient = new restClient();
            
            rClient.AGVaddress = "http://130.237.5.131/api/v2.0.0/status"; 

            responseResult = rClient.makeAGVRequest();

            return responseResult;
        }

        private double[] RTLSRequestToServer()
        {
            double[] responseResult = new double[3];

            restClient rClient = new restClient();

            //rClient.endPoint = txtURI.Text;
            //rClient.userName = txtUserName.Text;
            //rClient.userPassword = txtPassword.Text;
            rClient.RTLSaddress = "https://p186-geps-production-api.hd-rtls.com/objects/00000011/pos?max_age=60000"; 
            rClient.userName = "KTH";
            rClient.userPassword = "!Test4KTH";

            responseResult = rClient.makeRTLSRequest();

            return responseResult;
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {                    
            int iterNum = Convert.ToInt32(txtIterationNum.Text); // iteration number
            double intervalTime = Convert.ToDouble(txtIntervalTime.Text); // interval time
            //int iterNum = 2000;
            //double intervalTime = 1;

            for (int i = 0; i < iterNum; i++)
            {
                string TimeStamp = System.DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss.ff");

                // Get coordinates of HDW tag on AGV
                Task<double[]> RTLStask = new Task<double[]>(RTLSRequestToServer);
                RTLStask.Start();
                RTLStempResult = await RTLStask;

                // Get coordinates of AGV
                //Task<double[]> AGVtask = new Task<double[]>(AGVRequestToServer);
                //AGVtask.Start();
                //AGVtempResult = await AGVtask;

                // match RTLS point coordinatesand AGV point coordinates towards canvas coordinates system
                RTLSsubResult[0] = TimeStamp; // timestamp 
                RTLSsubResult[1] = (-RTLStempResult[1] * (-0.01116546) + 7.824105).ToString(); // X
                RTLSsubResult[2] = (RTLStempResult[0] * (-0.01116546) + 18.16041).ToString(); //Y
                
                AGVsubResult[0] = TimeStamp; // timestamp
                AGVsubResult[1] = AGVtempResult[1].ToString(); // X
                AGVsubResult[2] = AGVtempResult[2].ToString(); // Y

                // store coordinates and timestamp of RTLS tag and AGV into list object
                RTLSpos.Add(new List<string>());
                RTLSpos[i].Add(RTLSsubResult[0]);
                RTLSpos[i].Add(RTLSsubResult[1]);
                RTLSpos[i].Add(RTLSsubResult[2]);

                AGVpos.Add(new List<string>());
                AGVpos[i].Add(AGVsubResult[0]);
                AGVpos[i].Add(AGVsubResult[1]);
                AGVpos[i].Add(AGVsubResult[2]);

                // check whether RTLS tag point is outlier or not 
                if (i > 0)
                {
                    // distance between RTLS tag's time (i) point and time (i-1) point
                    double dist = Math.Sqrt(((Convert.ToDouble(RTLSpos[i][1]) - Convert.ToDouble(RTLSpos[i - 1][1])) * (Convert.ToDouble(RTLSpos[i][1]) - Convert.ToDouble(RTLSpos[i - 1][1]))
                        + ((Convert.ToDouble(RTLSpos[i][2]) - Convert.ToDouble(RTLSpos[i - 1][2])) * (Convert.ToDouble(RTLSpos[i][2]) - Convert.ToDouble(RTLSpos[i - 1][2])))));

                    // outlier criteria = ? (in AGV coordinates system)
                    if (dist > 3)
                    {
                        RTLSpos[i][0] = RTLSpos[i - 1][0];
                        RTLSpos[i][1] = RTLSpos[i - 1][1];
                    }
                }
                
                // show coordinates of RTLS tag and AGV
                RTLSresult = "Coordinates of RTLS tag {Time: " + RTLSpos[i][0] + ", X: " + RTLSpos[i][1] + ", Y: " + RTLSpos[i][2] + "}";
                AGVresult = "Coordinates of MirAGV {Time: " + AGVpos[i][0] + ", X: " + AGVpos[i][1] + ", Y: " + AGVpos[i][2] + "}";
                //txtResponse.Text = txtResponse.Text + (i + 1).ToString() + ", " + System.DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss.fff") + "\r\n" + RTLSresult + "\r\n" + AGVresult + "\r\n";
                txtResponse.Text = txtResponse.Text + (i + 1).ToString() + ", " + TimeStamp + "\r\n" + RTLSresult + "\r\n" + AGVresult + "\r\n";
                txtResponse.ScrollToEnd();
                
                // Show where the RTLS tag has been
                int dotSizeRTLS = 5;

                Ellipse currentDotRTLS = new Ellipse();

                Color colorRTLS = new Color();
                double tempProgressRTLS = (double)i / (double)intervalTime;
                colorRTLS = Colors.Red;

                currentDotRTLS.Stroke = new SolidColorBrush(colorRTLS);
                currentDotRTLS.StrokeThickness = 3;
                Canvas.SetZIndex(currentDotRTLS, 3);
                currentDotRTLS.Height = dotSizeRTLS;
                currentDotRTLS.Width = dotSizeRTLS;

                currentDotRTLS.Fill = new SolidColorBrush(colorRTLS);
                currentDotRTLS.Margin = new Thickness(Convert.ToDouble(RTLSpos[i][1]) * 15.0, Convert.ToDouble(RTLSpos[i][2]) * 15.0, 0, 0); // Set the position
                myCanvas.Children.Add(currentDotRTLS);

                // Show where the MiRAGV has been
                int dotSizeAGV = 3;

                Ellipse currentDotAGV = new Ellipse();

                Color colorAGV = new Color();
                double tempProgressAGV = (double)i / (double)intervalTime;
                colorAGV = Colors.Blue;

                currentDotAGV.Stroke = new SolidColorBrush(colorAGV);
                currentDotAGV.StrokeThickness = 3;
                Canvas.SetZIndex(currentDotAGV, 3);
                currentDotAGV.Height = dotSizeAGV;
                currentDotAGV.Width = dotSizeAGV;

                currentDotAGV.Fill = new SolidColorBrush(colorAGV);
                currentDotAGV.Margin = new Thickness(Convert.ToDouble(AGVpos[i][1]) * 15.0, Convert.ToDouble(AGVpos[i][2]) * 15.0, 0, 0); // Set the position

                myCanvas.Children.Add(currentDotAGV);
                
                await Task.Delay(TimeSpan.FromMilliseconds(intervalTime * 1000));
            }
        }

        public static Color Rainbow(float progress)
        {
            float div = (Math.Abs(progress % 1) * 6);
            int ascending = (int)((div % 1) * 255);
            int descending = 255 - ascending;

            switch ((int)div)
            {
                case 0:
                    return Color.FromArgb(255, 255, Convert.ToByte(ascending), 0);
                case 1:
                    return Color.FromArgb(255, Convert.ToByte(descending), 255, 0);
                case 2:
                    return Color.FromArgb(255, 0, 255, Convert.ToByte(ascending));
                case 3:
                    return Color.FromArgb(255, 0, Convert.ToByte(descending), 255);
                case 4:
                    return Color.FromArgb(255, Convert.ToByte(ascending), 0, 255);
                default: // case 5:
                    return Color.FromArgb(255, 255, 0, Convert.ToByte(descending));
            }
        }
    }
}
