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
        string[] rtlsTempResult = new string[3];
        string[] agvTempResult = new string[3];
        string[] rtlsSubResult = new string[3];
        string[] agvSubResult = new string[3];
        string rtlsResult = "";
        string agvResult = "";
        
        List<List<string>> rtlsPosition = new List<List<string>>();
        List<List<string>> agvPosition = new List<List<string>>();
        
        
        public MainWindow()
        {
            InitializeComponent();
        }

        private string[] requestAgvServer(string agvURI)
        {
            string[] responseResult = new string[3];

            RESTClinet rClient = new RESTClinet();
            
            rClient.agvAddress = agvURI; 

            responseResult = rClient.makeAgvRequest();

            return responseResult;
        }
        

        private string[] requestRtlsServer(string rtlsURI, string username, string password)
        {
            string[] responseResult = new string[3];

            RESTClinet rClient = new RESTClinet();

            //rClient.endPoint = txtURI.Text;
            //rClient.userName = txtUserName.Text;
            //rClient.userPassword = txtPassword.Text;
            rClient.rtlsAddress = rtlsURI; // max_age criteria ?
            rClient.userName = username;
            rClient.userPassword = password;

            responseResult = rClient.makeRtlsRequest();

            return responseResult;
        }
        

        /// <summary>
        /// calculate distance by method,
        /// method 1: calculating RTLS tag's on time (i) and time (i - 1),
        /// method 2: calculating RTLS tag and AGV on time (i)
        /// </summary>
        /// <param name="firstResource"></param>
        /// <param name="secondResource"></param>
        /// <param name="i"></param>
        /// <param name="method"></param>
        /// <returns></returns>
        private double getDistance(List<List<string>> firstResource, List<List<string>> secondResource, int method, int i)
        {
            double dist = 0.0;

            if (method == 1)
            {
                double dist1 = (Convert.ToDouble(firstResource[i][1]) - Convert.ToDouble(firstResource[i - 1][1]));
                double dist2 = (Convert.ToDouble(firstResource[i][2]) - Convert.ToDouble(firstResource[i - 1][2]));
                dist = Math.Sqrt(dist1 * dist1 + dist2 * dist2);
            }
            else if (method == 2)
            {
                double dist1 = (Convert.ToDouble(firstResource[i][1]) - Convert.ToDouble(secondResource[i][1]));
                double dist2 = (Convert.ToDouble(firstResource[i][2]) - Convert.ToDouble(secondResource[i][2]));
                dist = Math.Sqrt(dist1 * dist1 + dist2 * dist2);
            }

            return dist;
        }

        private void ButtonCheck_Click(object sender, RoutedEventArgs e)
        {
            string agvAddress = "http://130.237.2.106/api/v2.0.0/status";
            string rtlsAddress = "https://p186-geps-production-api.hd-rtls.com/objects/00000011/pos?max_age=" + txtCheckSeconds.Text; // max_age criteria ?
            string userName = "KTH";
            string password = "!Test4KTH";

            txtAGVuri.Text = agvAddress;
            txtRTLSuri.Text = rtlsAddress;
            txtUserName.Text = userName;
            txtPassword.Text = password;

            
        }


        private async void ButtonGo_Click(object sender, RoutedEventArgs e)
        {
            string agvURI = txtAGVuri.Text;
            string rtlsURI = txtRTLSuri.Text;
            string userName = txtUserName.Text;
            string password = txtPassword.Text;
            int iterNum = Convert.ToInt32(txtIterationNum.Text); // iteration number
            double intervalTime = Convert.ToDouble(txtIntervalTime.Text); // interval time

            for (int i = 0; i < iterNum; i++)
            {
                string timeStamp = System.DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss.ff");

                // Get coordinates of HDW tag on AGV
                var rtlsTask = Task.Run(() => requestRtlsServer(rtlsURI, userName, password));
                rtlsTempResult = await rtlsTask;

                // Get coordinates of AGV
                var agvTask = Task.Run(() => requestAgvServer(agvURI));
                agvTempResult = await agvTask;
                

                // match RTLS point coordinatesand AGV point coordinates towards canvas coordinates system
                rtlsSubResult[0] = timeStamp; // timeStamp 
                if (rtlsTempResult[1] == "") rtlsSubResult[1] = string.Empty;
                else rtlsSubResult[1] = (-Convert.ToDouble(rtlsTempResult[1]) * (-0.01116546) + 7.824105).ToString(); // X
                if (rtlsTempResult[2] == "") rtlsSubResult[2] = string.Empty;
                else rtlsSubResult[2] = (Convert.ToDouble(rtlsTempResult[0]) * (-0.01116546) + 18.16041).ToString(); // Y

                agvSubResult[0] = timeStamp; // timeStamp
                agvSubResult[1] = agvTempResult[1]; // X
                agvSubResult[2] = agvTempResult[2]; // Y


                // store coordinates and timeStamp of RTLS tag and AGV into list object
                rtlsPosition.Add(new List<string>());
                rtlsPosition[i].Add(rtlsSubResult[0]);
                rtlsPosition[i].Add(rtlsSubResult[1]);
                rtlsPosition[i].Add(rtlsSubResult[2]);

                agvPosition.Add(new List<string>());
                agvPosition[i].Add(agvSubResult[0]);
                agvPosition[i].Add(agvSubResult[1]);
                agvPosition[i].Add(agvSubResult[2]);


                // check whether RTLS tag point is outlier or not
                if (rtlsPosition.Count > 1)
                {
                    if (rtlsPosition[i][1] != "" && rtlsPosition[i - 1][1] != "")
                    {
                        // distance between RTLS tag's time (i) point and time (i-1) point
                        double dist = getDistance(rtlsPosition, rtlsPosition, 1, i);

                        // outlier criteria = ? 
                        if (dist > 3.0)
                        {
                            rtlsPosition[i][1] = rtlsPosition[i - 1][1];
                            rtlsPosition[i][2] = rtlsPosition[i - 1][2];
                        }
                    }
                }
                

                // show coordinates of RTLS tag and AGV
                if (rtlsPosition[i][1] != "")
                {
                    rtlsResult = "Coordinates of RTLS tag {Time: " + rtlsPosition[i][0] + ", X: " + rtlsPosition[i][1] + ", Y: " + rtlsPosition[i][2] + "}";
                }
                else
                {
                    rtlsResult = "Coordinates of RTLS tag {Time: " + rtlsPosition[i][0] + ", X: ---, Y: --- }";
                }
                agvResult = "Coordinates of MirAGV {Time: " + agvPosition[i][0] + ", X: " + agvPosition[i][1] + ", Y: " + agvPosition[i][2] + "}";
                txtResponse.Text = txtResponse.Text + (i + 1).ToString() + ", " + timeStamp + "\r\n" + rtlsResult + "\r\n" + agvResult + "\r\n";
                txtResponse.ScrollToEnd();
                
                /*
                // calculate distance between RTLS tag and AGV for judging same place or redZone
                if (rtlsPosition[i][1] != "")
                {
                    // calculate distance between RTLS tag and AGV
                    double dist = getDistance(rtlsPosition, agvPosition, 2, i);

                    // same place criteria = ?
                    if (dist < 2.0) 
                    {

                    }
                }
                */
                
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

                if (rtlsPosition[i][1] != "")
                {
                    currentDotRTLS.Margin = new Thickness(Convert.ToDouble(rtlsPosition[i][1]) * 15.0, Convert.ToDouble(rtlsPosition[i][2]) * 15.0, 0, 0); // Set the position
                    myCanvas.Children.Add(currentDotRTLS);
                }


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
                currentDotAGV.Margin = new Thickness(Convert.ToDouble(agvPosition[i][1]) * 15.0, Convert.ToDouble(agvPosition[i][2]) * 15.0, 0, 0); // Set the position
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
