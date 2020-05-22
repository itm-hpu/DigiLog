using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Forms;
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
            public DateTime? TimeStamp { get; set; }
            public double Coordinate_X { get; set; }
            public double Coordinate_Y { get; set; }
            public string objectID { get; set; }
        }

        /*
        public class PositionDataAGV
        {
            public DateTime TimeStamp { get; set; }
            public double Coordinate_X { get; set; }
            public double Coordinate_Y { get; set; }
        }
        */
        
        string[] tempReuslt_RTLS = new string[3];
        //string[] tempReuslt_AGV = new string[3];
        string[] subResult_RTLS = new string[3];
        //string[] subResult_AGV = new string[3];

        List<List<PositionDataRTLS>> position_RTLS = new List<List<PositionDataRTLS>>();
        //List<PositionDataAGV> position_AGV = new List<PositionDataAGV>();

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

            rClient.rtlsAddress = rtlsURI; 
            rClient.userName = username;
            rClient.userPassword = password;

            responseResult = rClient.MakeRTLSRequest();

            return responseResult;
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

        private void ButtonCheck_Click(object sender, RoutedEventArgs e)
        {
            string objectIDs = txtTAGIDs.Text;
            string[] objectIDsArray = objectIDs.Split(new char[] { '\n' });
            Array.Resize(ref objectIDsArray, objectIDsArray.Length - 1);

            string agvAddress = "http://130.237.5.89/api/v2.0.0/status";
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


        private async void ButtonAcquire_Click(object sender, RoutedEventArgs e)
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

            // To show origin point on canvas
            int dotsizeOrigin = 7;
            Ellipse dotOrigin = new Ellipse();
            Color colorOrigin = new Color();
            colorOrigin = Colors.Black;
            dotOrigin.Stroke = new SolidColorBrush(colorOrigin);
            dotOrigin.StrokeThickness = 3;
            Canvas.SetZIndex(dotOrigin, 3);
            dotOrigin.Height = dotsizeOrigin;
            dotOrigin.Width = dotsizeOrigin;
            dotOrigin.Fill = new SolidColorBrush(colorOrigin);
            dotOrigin.Margin = new Thickness(0, 0, 0, 0); 
            myCanvas.Children.Add(dotOrigin);

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

                /*
                // Get coordinates of AGV
                var agvTask = Task.Run(() => RequestServer_AGV(agvURI));
                tempReuslt_AGV = await agvTask;
                */

                // match RTLS point coordinatesand AGV point coordinates towards canvas coordinates system
                List<PositionDataRTLS> subResult_RTLS_list = new List<PositionDataRTLS>();
                
                for (int j = 0; j < tempReuslt_RTLS_list.Count(); j++)
                {
                    subResult_RTLS_list.Add(new PositionDataRTLS());

                    if (tempReuslt_RTLS_list[j][0] == "") subResult_RTLS_list[j].TimeStamp = null;
                    else subResult_RTLS_list[j].TimeStamp = Convert.ToDateTime(tempReuslt_RTLS_list[j][2]);

                    if (tempReuslt_RTLS_list[j][1] == "") subResult_RTLS_list[j].Coordinate_X = double.NaN;
                    else subResult_RTLS_list[j].Coordinate_X = (-Convert.ToDouble(tempReuslt_RTLS_list[j][1])); // X
                    //else subResult_RTLS_list[j].Coordinate_X = (-Convert.ToDouble(tempReuslt_RTLS_list[j][1]) * (-0.01116546) + 7.824105); // X

                    if (tempReuslt_RTLS_list[j][2] == "") subResult_RTLS_list[j].Coordinate_Y = double.NaN;
                    else subResult_RTLS_list[j].Coordinate_Y = (Convert.ToDouble(tempReuslt_RTLS_list[j][0])); // Y
                    //else subResult_RTLS_list[j].Coordinate_Y = (Convert.ToDouble(tempReuslt_RTLS_list[j][0]) * (-0.01116546) + 18.16041); // Y

                    subResult_RTLS_list[j].objectID = tempReuslt_RTLS_list[j][3]; // TAG ID
                }

                /*
                var subResult_AGV = new PositionDataAGV();
                subResult_AGV.TimeStamp = Convert.ToDateTime(timeStamp); // AGV timestamp means program time (there is no AGV's own timestamp)
                subResult_AGV.Coordinate_X = Convert.ToDouble(tempReuslt_AGV[1]);
                subResult_AGV.Coordinate_Y = Convert.ToDouble(tempReuslt_AGV[2]);
                */
                
                // store PositionData of RTLS tag and AGV into list
                position_RTLS.Add(subResult_RTLS_list);
                //position_AGV.Add(subResult_AGV);

                

                // show coordinates of RTLS tag and AGV
                string result_RTLS = "";
                //string result_AGV = "";
                for (int j = 0; j < position_RTLS[i].Count(); j++)
                {
                    if (position_RTLS[i][j].Coordinate_X != System.Double.NaN)
                    {
                        result_RTLS = result_RTLS + "Coordinates of RTLS tag {Time: " + position_RTLS[i][j].TimeStamp + ", X: " + position_RTLS[i][j].Coordinate_X + ", Y: " + position_RTLS[i][j].Coordinate_Y + ", objectID: " + position_RTLS[i][j].objectID + "}" + "\r\n";
                    }
                    else
                    {
                        result_RTLS = result_RTLS + "Coordinates of RTLS tag {Time: " + position_RTLS[i][j].TimeStamp + ", X: ---, Y: ---, objectID: " + position_RTLS[i][j].objectID + " }" + "\r\n";
                    }
                    //result_AGV = "Coordinates of MirAGV {Time: " + position_AGV[i].TimeStamp + ", X: " + position_AGV[i].Coordinate_X + ", Y: " + position_AGV[i].Coordinate_Y + "}";
                }
                txtResponse.Text = txtResponse.Text + (i + 1).ToString() + ", " + timeStamp + "\r\n" + result_RTLS + "\r\n"; //result_AGV + "\r\n";
                txtResponse.ScrollToEnd();

                
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

                    if (Double.IsNaN(position_RTLS[i][j].Coordinate_X))
                    {
                        continue;
                    }
                    else
                    {
                        currentDotRTLS.Margin = new Thickness(position_RTLS[i][j].Coordinate_X * 15.0, position_RTLS[i][j].Coordinate_Y * 15.0, 0, 0); // Set the position
                        myCanvas.Children.Add(currentDotRTLS);
                    }
                }
                
                /*
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
                */

                await Task.Delay(TimeSpan.FromMilliseconds(intervalTime * 1000));
            }

            string message = "Finish acquiring position data!";
            string caption = "Acquire module";
            System.Windows.MessageBox.Show(message, caption);
        }

        
        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            WriteCSVfile(position_RTLS);
            //WriteCSVfile(position_AGV);
        }


        private void BtnReset_Click(object sender, RoutedEventArgs e)
        {
            
            //System.Windows.Forms.Application.Restart();
            //System.Windows.Application.Current.Shutdown();
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

        public void WriteCSVfile(List<List<PositionDataRTLS>> positionDatas)
        {
            string timeStamp = System.DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");

            int iLength = positionDatas.Count();
            int jLength = positionDatas[0].Count();

            string[] filedir = new string[jLength];
            StreamWriter[] file = new StreamWriter[jLength];

            for (int j = 0; j < jLength; j++)
            {
                filedir[j] = Directory.GetCurrentDirectory();
                filedir[j] = filedir[j] + @"\PositionData_RTLS_" + timeStamp + "_" + positionDatas[0][j].objectID + ".csv";
                file[j] = new StreamWriter(filedir[j]);

                for (int i = 0; i < iLength; i++)
                {
                    file[j].Write(positionDatas[i][j].TimeStamp + ", " + positionDatas[i][j].Coordinate_X + ", " + positionDatas[i][j].Coordinate_Y);
                    file[j].Write("\n");
                }
                file[j].Close();
            }
            return;
        }

        /*
        public void WriteCSVfile(List<PositionDataAGV> positionDatas)
        {
            string filedir = Directory.GetCurrentDirectory();
            string timeStamp = System.DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss");

            filedir = filedir + @"\PositionData_AGV_" + timeStamp + ".csv";
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
        */
    }
}
