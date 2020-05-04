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
using System.IO;


namespace LabManager
{

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public class PositionData
        {
            public string TimeStamp { get; set; }
            public string Coordinate_X { get; set; }
            public string Coordinate_Y { get; set; }
        }


        string[] tempReuslt_RTLS = new string[3];
        string[] tempReuslt_AGV = new string[3];
        string[] subResult_RTLS = new string[3];
        string[] subResult_AGV = new string[3];
        string result_RTLS = "";
        string result_AGV = "";

        List<PositionData> position_RTLS = new List<PositionData>();
        List<PositionData> position_AGV = new List<PositionData>();
        

        public MainWindow()
        {
            InitializeComponent();
        }


        private string[] RequestServer_AGV(string agvURI)
        {
            string[] responseResult = new string[3];

            RESTClinet rClient = new RESTClinet();
            
            rClient.agvAddress = agvURI; 

            responseResult = rClient.MakeAGVRequest();

            return responseResult;
        }
        

        private string[] RequestServer_RTLS(string rtlsURI, string username, string password)
        {
            string[] responseResult = new string[3];

            RESTClinet rClient = new RESTClinet();

            //rClient.endPoint = txtURI.Text;
            //rClient.userName = txtUserName.Text;
            //rClient.userPassword = txtPassword.Text;
            rClient.rtlsAddress = rtlsURI; // max_age criteria ?
            rClient.userName = username;
            rClient.userPassword = password;

            responseResult = rClient.MakeRTLSRequest();

            return responseResult;
        }


        /// <summary>
        /// Calculate distance by method, "firstResource" and "secondResource": resources to calculate, 
        /// "method": { 0: calculate RTLS tag's between time (i) and time (i-1), 1: calculate RTLS tag and AGV on time (i) },
        /// "index": data index to calculate
        /// </summary>
        /// <param name="firstResource"></param>
        /// <param name="secondResource"></param>
        /// <param name="method"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        private double GetDistance(List<PositionData> firstResource, List<PositionData> secondResource, int method, int index)
        {
            double dist = 0.0;

            if (method == 0)
            {
                double dist1 = (Convert.ToDouble(firstResource[index].Coordinate_X) - Convert.ToDouble(firstResource[index - 1].Coordinate_X));
                double dist2 = (Convert.ToDouble(firstResource[index].Coordinate_Y) - Convert.ToDouble(firstResource[index - 1].Coordinate_Y));
                dist = Math.Sqrt(dist1 * dist1 + dist2 * dist2);
            }
            else if (method == 1)
            {
                double dist1 = (Convert.ToDouble(firstResource[index].Coordinate_X) - Convert.ToDouble(secondResource[index].Coordinate_X));
                double dist2 = (Convert.ToDouble(firstResource[index].Coordinate_Y) - Convert.ToDouble(secondResource[index].Coordinate_Y));
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
                var rtlsTask = Task.Run(() => RequestServer_RTLS(rtlsURI, userName, password));
                tempReuslt_RTLS = await rtlsTask;

                // Get coordinates of AGV
                var agvTask = Task.Run(() => RequestServer_AGV(agvURI));
                tempReuslt_AGV = await agvTask;

                // match RTLS point coordinatesand AGV point coordinates towards canvas coordinates system
                var subResult_RTLS = new PositionData();
                subResult_RTLS.TimeStamp = tempReuslt_RTLS[2];
                if (tempReuslt_RTLS[1] == "") subResult_RTLS.Coordinate_X = string.Empty;
                else subResult_RTLS.Coordinate_X = (-Convert.ToDouble(tempReuslt_RTLS[1]) * (-0.01116546) + 7.824105).ToString(); // X
                if (tempReuslt_RTLS[2] == "") subResult_RTLS.Coordinate_Y = string.Empty;
                else subResult_RTLS.Coordinate_Y = (Convert.ToDouble(tempReuslt_RTLS[0]) * (-0.01116546) + 18.16041).ToString(); // Y

                var subResult_AGV = new PositionData();
                subResult_AGV.TimeStamp = timeStamp; // AGV timestamp means program time (there is no AGV's own timestamp)
                subResult_AGV.Coordinate_X = tempReuslt_AGV[1];
                subResult_AGV.Coordinate_Y = tempReuslt_AGV[2];


                // store PositionData of RTLS tag and AGV into list
                position_RTLS.Add(subResult_RTLS);
                position_AGV.Add(subResult_AGV);
                

                // check whether RTLS tag point is outlier or not
                if (position_RTLS.Count > 1)
                {
                    if (position_RTLS[i].Coordinate_X != "" && position_RTLS[i - 1].Coordinate_X != "")
                    {
                        // distance between RTLS tag's time (i) point and time (i-1) point
                        double dist = GetDistance(position_RTLS, position_RTLS, 0, i);

                        // outlier criteria = ? 
                        if (dist > 3.0)
                        {
                            position_RTLS[i].Coordinate_X = position_RTLS[i - 1].Coordinate_X;
                            position_RTLS[i].Coordinate_Y = position_RTLS[i - 1].Coordinate_Y;
                        }
                    }
                }
                

                // show coordinates of RTLS tag and AGV
                if (position_RTLS[i].Coordinate_X != "")
                {
                    result_RTLS = "Coordinates of RTLS tag {Time: " + position_RTLS[i].TimeStamp + ", X: " + position_RTLS[i].Coordinate_X + ", Y: " + position_RTLS[i].Coordinate_Y + "}";
                }
                else
                {
                    result_RTLS = "Coordinates of RTLS tag {Time: " + position_RTLS[i].TimeStamp + ", X: ---, Y: --- }";
                }
                result_AGV = "Coordinates of MirAGV {Time: " + position_AGV[i].TimeStamp + ", X: " + position_AGV[i].Coordinate_X + ", Y: " + position_AGV[i].Coordinate_Y + "}";
                txtResponse.Text = txtResponse.Text + (i + 1).ToString() + ", " + timeStamp + "\r\n" + result_RTLS + "\r\n" + result_AGV + "\r\n";
                txtResponse.ScrollToEnd();
                

                
                // calculate distance between RTLS tag and AGV for judging same place or redZone
                if (position_RTLS[i].Coordinate_X != "")
                {
                    // calculate distance between RTLS tag and AGV
                    double dist = GetDistance(position_RTLS, position_AGV, 1, i);

                    // same place criteria = ?
                    if (dist < 2.0) 
                    {

                    }
                }
                
                
                
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

                if (position_RTLS[i].Coordinate_X != "")
                {
                    currentDotRTLS.Margin = new Thickness(Convert.ToDouble(position_RTLS[i].Coordinate_X) * 15.0, Convert.ToDouble(position_RTLS[i].Coordinate_Y) * 15.0, 0, 0); // Set the position
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
                currentDotAGV.Margin = new Thickness(Convert.ToDouble(position_AGV[i].Coordinate_X) * 15.0, Convert.ToDouble(position_AGV[i].Coordinate_Y) * 15.0, 0, 0); // Set the position
                myCanvas.Children.Add(currentDotAGV);
                
                
                await Task.Delay(TimeSpan.FromMilliseconds(intervalTime * 1000));
                
            }

            WriteCSVfile(position_RTLS, 0);
            WriteCSVfile(position_AGV, 1);
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

        /// <summary>
        /// Write position data into txt file, "positionDatas": object name to store, "system": { 0: RTLS, 1: AGV }
        /// </summary>
        /// <param name="positionDatas"></param>
        /// <param name="system"></param>
        public void WriteCSVfile(List<PositionData> positionDatas, int system)
        {
            string filedir = Directory.GetCurrentDirectory();

            if (system == 0)
            {
                filedir = filedir + @"\PositionData_RTLS_" + System.DateTime.Now.ToString("yyyy-MM-dd") + ".txt";
            }
            else if (system == 1)
            {
                filedir = filedir + @"\PositionData_AGV_" + System.DateTime.Now.ToString("yyyy-MM-dd") + ".txt";
            }

            StreamWriter file = new StreamWriter(filedir);

            int inputLength = positionDatas.Count();

            for (int i = 0; i < inputLength; i++)
            {
                file.Write(positionDatas[i].TimeStamp + ", " + positionDatas[i].Coordinate_X + ", " + positionDatas[i].Coordinate_Y);
                file.Write("\n");
            }

            file.Close();

            return;
        }
        
    }
}
