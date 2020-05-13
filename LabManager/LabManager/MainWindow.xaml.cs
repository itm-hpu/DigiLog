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

        public class PositionDataRTLS
        {
            public string TimeStamp { get; set; }
            public string Coordinate_X { get; set; }
            public string Coordinate_Y { get; set; }
            public string objectID { get; set; }
        }

        public class PositionDataAGV
        {
            public string TimeStamp { get; set; }
            public string Coordinate_X { get; set; }
            public string Coordinate_Y { get; set; }
        }

        public class TimeForActivity
        {
            public string DepartTime { get; set; }
            public string ArriveTime { get; set; }
            public string DepartPos { get; set; }
            public string ArrivePos { get; set; }
        }
        
        string[] tempReuslt_RTLS = new string[3];
        string[] tempReuslt_AGV = new string[3];
        string[] subResult_RTLS = new string[3];
        string[] subResult_AGV = new string[3];

        List<List<PositionDataRTLS>> position_RTLS = new List<List<PositionDataRTLS>>();
        List<PositionDataAGV> position_AGV = new List<PositionDataAGV>();

        string result_IDs = "";
        
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
        

        private string[] RequestServer_RTLS(string rtlsURI, string username, string password, string objectID)
        {
            string[] responseResult = new string[4];

            RESTClinet rClient = new RESTClinet();

            //rClient.endPoint = txtURI.Text;
            //rClient.userName = txtUserName.Text;
            //rClient.userPassword = txtPassword.Text;
            rClient.rtlsAddress = rtlsURI; 
            rClient.userName = username;
            rClient.userPassword = password;

            responseResult = rClient.MakeRTLSRequest();

            return responseResult;
        }


        /// <summary>
        /// Calculate distance between the first and second point
        /// </summary>
        /// <param name="firstPoint_X"></param>
        /// <param name="firstPoint_Y"></param>
        /// <param name="secondPoint_X"></param>
        /// <param name="secondPoint_Y"></param>
        /// <returns></returns>
        private double GetDistance(string firstPoint_X, string firstPoint_Y, string secondPoint_X, string secondPoint_Y) 
        {
            double dist = 0.0;

            double dist1 = (Convert.ToDouble(firstPoint_X) - Convert.ToDouble(secondPoint_X));
            double dist2 = (Convert.ToDouble(firstPoint_Y) - Convert.ToDouble(secondPoint_Y));
            dist = Math.Sqrt(dist1 * dist1 + dist2 * dist2);

            return dist;
        }


        private void ButtonCheck_Click(object sender, RoutedEventArgs e)
        {
            string objectIDs = txtTAGIDs.Text;
            string[] objectIDsArray = objectIDs.Split(new char[] { '\n' });
            Array.Resize(ref objectIDsArray, objectIDsArray.Length - 1);

            string agvAddress = "http://130.237.2.106/api/v2.0.0/status";
            txtAGVuri.Text = agvAddress;

            string[] rtlsAddressArray = new string[objectIDsArray.Length];
            for (int i = 0; i < objectIDsArray.Length; i++)
            {
                rtlsAddressArray[i] = "https://p186-geps-production-api.hd-rtls.com/objects/" + objectIDsArray[i] + "/pos?max_age=" + txtCheckSeconds.Text;
                txtRTLSuri.Text = txtRTLSuri.Text + rtlsAddressArray[i] + '\n';
            }

            string userName = "KTH";
            string password = "!Test4KTH";
            txtUserName.Text = userName;
            txtPassword.Text = password;
        }


        private async void ButtonGo_Click(object sender, RoutedEventArgs e)
        {
            string agvURI = txtAGVuri.Text;

            string objectIDs = txtTAGIDs.Text;
            string[] objectIDsArray = objectIDs.Split(new char[] { '\n' });
            Array.Resize(ref objectIDsArray, objectIDsArray.Length - 1);

            string rtlsURI = txtRTLSuri.Text;
            string[] rtlsURIArray = rtlsURI.Split(new char[] { '\n' });
            Array.Resize(ref rtlsURIArray, rtlsURIArray.Length - 1);

            string userName = txtUserName.Text;
            string password = txtPassword.Text;
             
            int iterNum = Convert.ToInt32(txtIterationNum.Text); // iteration number
            double intervalTime = Convert.ToDouble(txtIntervalTime.Text); // interval time

            for (int i = 0; i < iterNum; i++)
            {
                // https://docs.microsoft.com/en-us/dotnet/standard/base-types/custom-date-and-time-format-strings
                string timeStamp = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ff");

                // Get coordinates of HDW tags
                List<string[]> tempReuslt_RTLS_list = new List<string[]>();
                for (int j = 0; j < rtlsURIArray.Length; j++)
                {
                    var rtlsTask = Task.Run(() => RequestServer_RTLS(rtlsURIArray[j], userName, password, objectIDsArray[j]));
                    tempReuslt_RTLS_list.Add(await rtlsTask);
                }

                // Get coordinates of AGV
                var agvTask = Task.Run(() => RequestServer_AGV(agvURI));
                tempReuslt_AGV = await agvTask;

                // match RTLS point coordinatesand AGV point coordinates towards canvas coordinates system
                List<PositionDataRTLS> subResult_RTLS_list = new List<PositionDataRTLS>();
                
                for (int j = 0; j < tempReuslt_RTLS_list.Count(); j++)
                {
                    subResult_RTLS_list.Add(new PositionDataRTLS());

                    subResult_RTLS_list[j].TimeStamp = tempReuslt_RTLS_list[j][2];

                    if (tempReuslt_RTLS_list[j][1] == "") subResult_RTLS_list[j].Coordinate_X = string.Empty;
                    else subResult_RTLS_list[j].Coordinate_X = (-Convert.ToDouble(tempReuslt_RTLS_list[j][1]) * (-0.01116546) + 7.824105).ToString(); // X

                    if (tempReuslt_RTLS_list[j][2] == "") subResult_RTLS_list[j].Coordinate_Y = string.Empty;
                    else subResult_RTLS_list[j].Coordinate_Y = (Convert.ToDouble(tempReuslt_RTLS_list[j][0]) * (-0.01116546) + 18.16041).ToString(); // Y

                    subResult_RTLS_list[j].objectID = tempReuslt_RTLS_list[j][3]; // TAG ID
                }
 
                var subResult_AGV = new PositionDataAGV();
                subResult_AGV.TimeStamp = timeStamp; // AGV timestamp means program time (there is no AGV's own timestamp)
                subResult_AGV.Coordinate_X = tempReuslt_AGV[1];
                subResult_AGV.Coordinate_Y = tempReuslt_AGV[2];

                
                // store PositionData of RTLS tag and AGV into list
                position_RTLS.Add(subResult_RTLS_list);
                position_AGV.Add(subResult_AGV);

                /*
                // check whether RTLS tag point is outlier or not
                if (position_RTLS.Count > 1)
                {
                    if (position_RTLS[i].Coordinate_X != "" && position_RTLS[i - 1].Coordinate_X != "")
                    {
                        // distance between RTLS tag's time (i) point and time (i-1) point
                        double dist = GetDistance(position_RTLS[i].Coordinate_X, position_RTLS[i].Coordinate_Y, position_RTLS[i - 1].Coordinate_X, position_RTLS[i - 1].Coordinate_Y);

                        // outlier criteria = ? 
                        if (dist > 3.0)
                        {
                            position_RTLS[i].Coordinate_X = position_RTLS[i - 1].Coordinate_X;
                            position_RTLS[i].Coordinate_Y = position_RTLS[i - 1].Coordinate_Y;
                        }
                    }
                }
                */

                // show coordinates of RTLS tag and AGV
                string result_RTLS = "";
                string result_AGV = "";
                for (int j = 0; j < position_RTLS[i].Count(); j++)
                {
                    if (position_RTLS[i][j].Coordinate_X != "")
                    {
                        result_RTLS = result_RTLS + "Coordinates of RTLS tag {Time: " + position_RTLS[i][j].TimeStamp + ", X: " + position_RTLS[i][j].Coordinate_X + ", Y: " + position_RTLS[i][j].Coordinate_Y + ", objectID: " + position_RTLS[i][j].objectID + "}" + "\r\n";
                    }
                    else
                    {
                        result_RTLS = result_RTLS + "Coordinates of RTLS tag {Time: " + position_RTLS[i][j].TimeStamp + ", X: ---, Y: ---, objectID: " + position_RTLS[i][j].objectID + " }" + "\r\n";
                    }
                    result_AGV = "Coordinates of MirAGV {Time: " + position_AGV[i].TimeStamp + ", X: " + position_AGV[i].Coordinate_X + ", Y: " + position_AGV[i].Coordinate_Y + "}";
                }
                txtResponse.Text = txtResponse.Text + (i + 1).ToString() + ", " + timeStamp + "\r\n" + result_RTLS + result_AGV + "\r\n";
                txtResponse.ScrollToEnd();

                /*
                // calculate distance between RTLS tag and AGV for judging same place or redZone
                if (position_RTLS[i].Coordinate_X != "")
                {
                    // calculate distance between RTLS tag and AGV
                    double dist = GetDistance(position_RTLS[i].Coordinate_X, position_RTLS[i].Coordinate_Y, position_AGV[i].Coordinate_X, position_AGV[i].Coordinate_Y);

                    // same place criteria = ?
                    if (dist < 2.0) 
                    {

                    }
                }
                */

                
                // Show where the RTLS tag has been
                for (int j = 0; j < position_RTLS[i].Count(); j++)
                {
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

                    if (position_RTLS[i][j].Coordinate_X != "")
                    {
                        currentDotRTLS.Margin = new Thickness(Convert.ToDouble(position_RTLS[i][j].Coordinate_X) * 15.0, Convert.ToDouble(position_RTLS[i][j].Coordinate_Y) * 15.0, 0, 0); // Set the position
                        myCanvas.Children.Add(currentDotRTLS);
                    }
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

            WriteCSVfile(position_RTLS);
            WriteCSVfile(position_AGV);
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
        /// Write position data into txt file, "positionDatas": object name to store, "system": select system where position data from { 0: RTLS, 1: AGV }
        /// </summary>
        /// <param name="positionDatas"></param>
        /// <param name="system"></param>
        public void WriteCSVfile(List<List<PositionDataRTLS>> positionDatas)
        {
            string filedir = Directory.GetCurrentDirectory();

            filedir = filedir + @"\PositionData_RTLS_" + System.DateTime.Now.ToString("yyyy-MM-dd") + ".csv";

            StreamWriter file = new StreamWriter(filedir);

            int iLength = positionDatas.Count();
            int jLength = positionDatas[0].Count();

            for (int i = 0; i < iLength; i++)
            {
                for (int j = 0; j < jLength; j++)
                {
                    file.Write(positionDatas[i][j].objectID + ", " + positionDatas[i][j].TimeStamp + ", " + positionDatas[i][j].Coordinate_X + ", " + positionDatas[i][j].Coordinate_Y);
                    file.Write("\n");
                }
            }

            file.Close();

            return;
        }

        public void WriteCSVfile(List<PositionDataAGV> positionDatas)
        {
            string filedir = Directory.GetCurrentDirectory();

            filedir = filedir + @"\PositionData_AGV_" + System.DateTime.Now.ToString("yyyy-MM-dd") + ".csv";

            StreamWriter file = new StreamWriter(filedir);

            int iLength = positionDatas.Count();

            for (int i = 0; i < iLength; i++)
            {
                file.Write(positionDatas[i].TimeStamp + ", " + positionDatas[i].Coordinate_X + ", " + positionDatas[i].Coordinate_Y);
                file.Write("\n");
            }

            file.Close();

            return;
        }

        private void BtnGetID_Click(object sender, RoutedEventArgs e)
        {
            string rtlsURI = "https://p186-geps-production-api.hd-rtls.com/objects";
            string userName = "KTH";
            string password = "!Test4KTH";

            RESTClinet rClient = new RESTClinet();

            rClient.rtlsAddress = rtlsURI;
            rClient.userName = userName;
            rClient.userPassword = password;

            string[] responseResult = rClient.GetIDs();

            for (int i = 0; i < responseResult.Count(); i++)
            {
                result_IDs = result_IDs + responseResult[i] + "\n"; 
            }

            txtTAGIDs.Text = result_IDs;
        }
    }
}
