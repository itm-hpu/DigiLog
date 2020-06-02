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
        public MainWindow()
        {
            InitializeComponent();
        }

        // #######################################################################################
        // ############################### 1. Acquire raw data ###################################
        // #######################################################################################

        public class PositionData
        {
            public DateTime? TimeStamp { get; set; }
            public Point Coordinates { get; set; }
            public string ObjectID { get; set; }
            public string Type { get; set; }
        }

        List<List<PositionData>> result = new List<List<PositionData>>();



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


        public void WriteCSVfile(List<List<PositionData>> positionDatas)
        {
            string timeStamp = System.DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");

            int iLength = positionDatas.Count();
            int jLength = positionDatas[0].Count();

            string[] filedir = new string[jLength];
            StreamWriter[] file = new StreamWriter[jLength];

            for (int j = 0; j < jLength; j++)
            {

                filedir[j] = Directory.GetCurrentDirectory();
                filedir[j] = filedir[j] + @"\PositionData_" + timeStamp + "_" + positionDatas[0][j].Type + "_" + positionDatas[0][j].ObjectID + ".csv";
                file[j] = new StreamWriter(filedir[j]);

                for (int i = 0; i < iLength; i++)
                {
                    file[j].Write(positionDatas[i][j].Type + "," + "\t" + positionDatas[i][j].ObjectID + "," + positionDatas[i][j].TimeStamp + "," + positionDatas[i][j].Coordinates.X + "," + positionDatas[i][j].Coordinates.Y);
                    file[j].Write("\n");
                }
                file[j].Close();
            }
            return;
        }


        private void BtnGetID_Click(object sender, RoutedEventArgs e)
        {
            string result_IDs = "";

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

            string agvAddress = "http://192.168.128.36/api/v2.0.0/status";
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

            // Show origin point on canvas
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

                
                // Get coordinates of AGV
                string[] tempReuslt_AGV = new string[3];
                var agvTask = Task.Run(() => RequestServer_AGV(agvURI));
                tempReuslt_AGV = await agvTask;
                

                // Create list type of RTLS PositionData for several TAGs
                List<PositionData> subResult = new List<PositionData>();

                for (int j = 0; j < tempReuslt_RTLS_list.Count(); j++)
                {
                    subResult.Add(new PositionData());

                    if (tempReuslt_RTLS_list[j][0] == "") subResult[j].TimeStamp = null;
                    else subResult[j].TimeStamp = Convert.ToDateTime(tempReuslt_RTLS_list[j][2]);

                    if (tempReuslt_RTLS_list[j][1] != "" && tempReuslt_RTLS_list[j][2] != "")
                    {
                        subResult[j].Coordinates = new Point(-Convert.ToDouble(tempReuslt_RTLS_list[j][1]), Convert.ToDouble(tempReuslt_RTLS_list[j][0]));
                    }
                    else
                    {
                        subResult[j].Coordinates = new Point(double.NaN, double.NaN);
                    }

                    subResult[j].ObjectID = tempReuslt_RTLS_list[j][3]; // TAG ID
                    subResult[j].Type = "RTLS";
                }

                
                // Create list type of AGV PositionData to append "subResult" list
                List<PositionData> subResult_AGV = new List<PositionData>()
                {
                    new PositionData()
                    {
                        TimeStamp = Convert.ToDateTime(timeStamp),
                        Coordinates = new Point(Convert.ToDouble(tempReuslt_AGV[1]), Convert.ToDouble(tempReuslt_AGV[2])),
                        ObjectID = "12345678", // AGV object ID?
                        Type = "AGV"
                    }
                };
                

                
                // Append PositionData of AGV into "subReslut" list
                subResult.AddRange(subResult_AGV);
                

                // Store PositionData of RTLS tag and AGV into "reslut" list
                result.Add(subResult);


                // Show coordinates of RTLS tag and AGV
                string result_RTLS = "";
                string result_AGV = "";
                for (int j = 0; j < result[i].Count(); j++)
                {
                    if (result[i][j].Type == "RTLS")
                    {
                        if (result[i][j].Coordinates.X != System.Double.NaN)
                        {
                            result_RTLS = result_RTLS + "Coordinates of RTLS tag {Time: " + result[i][j].TimeStamp + ", X: " + result[i][j].Coordinates.X + ", Y: " + result[i][j].Coordinates.Y + ", objectID: " + result[i][j].ObjectID + "}" + "\r\n";
                        }
                        else
                        {
                            result_RTLS = result_RTLS + "Coordinates of RTLS tag {Time: " + result[i][j].TimeStamp + ", X: ---, Y: ---, objectID: " + result[i][j].ObjectID + " }" + "\r\n";
                        }
                    }
                    else if (result[i][j].Type == "AGV")
                    {
                        result_AGV = "Coordinates of MirAGV {Time: " + result[i][j].TimeStamp + ", X: " + result[i][j].Coordinates.X + ", Y: " + result[i][j].Coordinates.Y + "}" + "\r\n";
                    }
                }
                txtResponse.Text = txtResponse.Text + (i + 1).ToString() + ", " + timeStamp + "\r\n" + result_RTLS + result_AGV + "\r\n";
                txtResponse.ScrollToEnd();


                // Visualize where the RTLS tag and AGV have been
                for (int j = 0; j < result[i].Count(); j++)
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

                    if (result[i][j].Type == "RTLS")
                    {
                        if (Double.IsNaN(result[i][j].Coordinates.X))
                        {
                            continue;
                        }
                        else
                        {
                            currentDotRTLS.Margin = new Thickness(result[i][j].Coordinates.X * -0.5, result[i][j].Coordinates.Y * 0.5, 0, 0); // Set the position
                            myCanvas.Children.Add(currentDotRTLS);
                        }
                    }
                    else if (result[i][j].Type == "AGV")
                    {
                        currentDotAGV.Margin = new Thickness(result[i][j].Coordinates.X * 15.0, result[i][j].Coordinates.Y * 15.0, 0, 0); // Set the position
                        myCanvas.Children.Add(currentDotAGV);
                    }
                }

                await Task.Delay(TimeSpan.FromMilliseconds(intervalTime * 1000));
            }

            string message = "Finish acquiring position data!";
            string caption = "Acquire module";
            System.Windows.MessageBox.Show(message, caption);
        }


        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            WriteCSVfile(result);

            string message = "Finish saving position data!";
            string caption = "Acquire module";
            System.Windows.MessageBox.Show(message, caption);
        }


        private void BtnReset_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.Application.Restart();
            System.Windows.Application.Current.Shutdown();
        }


        // #######################################################################################
        // ############################### 2. Post-processing ####################################
        // #######################################################################################

        public class ParetoFreqTable
        {
            public double RangeValue { get; set; }
            public int Freq { get; set; }
            public int CumulFreq{ get; set; }
            public double CumulPercent { get; set; }
        }

        public class distancePoint
        {
            public DateTime TimeStamp { get; set; }
            public Point Coordinates { get; set; }
            public string ObjectID { get; set; }
            public double Distance { get; set; }
        }

        public class CandidatePoint
        {
            public DateTime TimeStamp { get; set; }
            public Point Coordinates { get; set; }
            public string ObjectID { get; set; }
        }

        /// <summary>
        /// Read CSV rawdata file
        /// </summary>
        /// <param name="CSVdir"></param>
        /// <returns></returns>
        public static string[,] ReadCSVfile(string CSVdir)
        {
            string whole_file = System.IO.File.ReadAllText(CSVdir);

            whole_file = whole_file.Replace('\n', '\r');

            string[] lines = whole_file.Split(new char[] { '\r' }, StringSplitOptions.RemoveEmptyEntries);

            int numRows = lines.Length;

            int numCols = lines[0].Split(',').Length;

            string[,] tempValues = new string[numRows, numCols];

            for (int r = 0; r < numRows; r++)
            {
                string[] line_r = lines[r].Split(',');

                for (int c = 0; c < numCols; c++)
                {
                    tempValues[r, c] = line_r[c];
                }
            }
            return tempValues;
        }

        /// <summary>
        /// Remove row which has a missing value, *** Need to adjust into more efficient way to find missing value ***
        /// </summary>
        /// <param name="array"></param>
        /// <returns></returns>
        public static string[,] RemoveEmptyRows(string[,] array)
        {
            // Find how many rows have an " NaN" value
            int rowsToRemove = 0;
            for (int i = 0; i <= array.GetUpperBound(0); i++)
            {
                if (array[i, 2] == " NaN") // NaN value of X_coordinate
                {
                    rowsToRemove++;
                }
            }

            // Reinitialize an array minus the number of empty rows
            string[,] results = new string[array.GetUpperBound(0) + 1 - rowsToRemove, array.GetUpperBound(1) + 1];

            int row = 0;
            for (int i = 0; i <= array.GetUpperBound(0); i++)
            {
                int col = 0;
                if (array[i, 2] != " NaN") // NaN value of X_coordinate
                {
                    for (int j = 0; j <= array.GetUpperBound(1); j++)
                    {
                        results[row, col] = array[i, j];
                        col++;
                    }
                    row++;
                }
            }
            return results;
        }

        /// <summary>
        /// Remove duplicate row
        /// </summary>
        /// <param name="array"></param>
        /// <returns></returns>
        public string[,] RemoveDuplicateRows(string[,] array)
        {
            // Find how many rows have an " NaN" value
            int rowsToRemove = 0;
            for (int i = 0; i <= array.GetUpperBound(0) - 1; i++)
            {
                if (array[i, 1] == array[i + 1, 1]) // compare time with previous row
                {
                    rowsToRemove++;
                }
            }

            // Reinitialize an array minus the number of duplicate rows
            string[,] results = new string[array.GetUpperBound(0) - rowsToRemove, array.GetUpperBound(1) + 1];

            int row = 0;
            for (int i = 0; i <= array.GetUpperBound(0) - 1; i++)
            {
                int col = 0;
                if (array[i, 1] != array[i + 1, 1]) // compare time with previous row
                {
                    for (int j = 0; j <= array.GetUpperBound(1); j++)
                    {
                        results[row, col] = array[i, j];
                        col++;
                    }
                    row++;
                }
            }

            string[] added = new string[array.GetUpperBound(1) + 1];

            if (array[array.GetUpperBound(0) - 1, 1] != array[array.GetUpperBound(0), 1])
            {
                for (int i = 0; i <= array.GetUpperBound(1); i++)
                {
                    added[i] = array[array.GetUpperBound(0), i];
                }
                results = AddRow(results, added);
            }

            return results;
        }

        /// <summary>
        /// Add 1-D array into 2-D array as a row to reinitialize 2-D array
        /// </summary>
        /// <param name="original"></param>
        /// <param name="added"></param>
        /// <returns></returns>
        static string[,] AddRow(string[,] original, string[] added)
        {
            int lastRow = original.GetUpperBound(0);
            int lastColumn = original.GetUpperBound(1);
            // Create new array.
            string[,] result = new string[lastRow + 2, lastColumn + 1];
            // Copy existing array into the new array.
            for (int i = 0; i <= lastRow; i++)
            {
                for (int x = 0; x <= lastColumn; x++)
                {
                    result[i, x] = original[i, x];
                }
            }
            // Add the new row.
            for (int i = 0; i < added.Length; i++)
            {
                result[lastRow + 1, i] = added[i];
            }
            return result;
        }

        /// <summary>
        /// Calculate euclidean distance between two points
        /// </summary>
        /// <param name="P1"></param>
        /// <param name="P2"></param>
        /// <returns></returns>
        public double GetDistance(Point P1, Point P2) // string
        {
            double dist = 0.0;

            double dist1 = P1.X - P2.X;
            double dist2 = P1.Y - P2.Y;
            dist = Math.Sqrt(dist1 * dist1 + dist2 * dist2);

            return dist;
        }

        /// <summary>
        /// Calculate percentile of a sorted data set (Q1, Q3, etc.)
        /// </summary>
        /// <param name="sortedData"></param>
        /// <param name="p"></param>
        /// <returns></returns>
        internal static double Percentile(double[] sortedData, double p)
        {
            // algo derived from Aczel pg 15 bottom
            if (p >= 100.0d) return sortedData[sortedData.Length - 1];

            double position = (sortedData.Length + 1) * p / 100.0;
            double leftNumber = 0.0d, rightNumber = 0.0d;

            double n = p / 100.0d * (sortedData.Length - 1) + 1.0d;

            if (position >= 1)
            {
                leftNumber = sortedData[(int)Math.Floor(n) - 1];
                rightNumber = sortedData[(int)Math.Floor(n)];
            }
            else
            {
                leftNumber = sortedData[0]; // first data
                rightNumber = sortedData[1]; // first data
            }

            //if (leftNumber == rightNumber)
            if (Equals(leftNumber, rightNumber))
                return leftNumber;
            double part = n - Math.Floor(n);
            return leftNumber + part * (rightNumber - leftNumber);
        } // end of internal function percentile
        
        /// <summary>
        /// Create 1-D distance array to manipulate other function more easily
        /// </summary>
        /// <param name="array"></param>
        /// <returns></returns>
        public double[] CreateDistArray(string[,] array)
        {
            Point[] coordinates = new Point[array.GetUpperBound(0) + 1]; // extract coordinates from rawdata

            for (int i = 0; i < coordinates.Length; i++)
            {
                coordinates[i] = new Point(Convert.ToDouble(array[i, 2]), Convert.ToDouble(array[i, 3]));
            }

            double[] distArray = new double[array.GetUpperBound(0) + 1]; // calculate distance between current and previous points 

            distArray[0] = 0;
            for (int i = 1; i < coordinates.Length; i++)
            {
                distArray[i] = GetDistance(coordinates[i - 1], coordinates[i]);
            }

            return distArray;
        }
        
        /// <summary>
        /// Create list data having "ObjectID", "TimeStamp", "Coordinates", "Distance" attributes
        /// </summary>
        /// <param name="array"></param>
        /// <returns></returns>
        public IList<distancePoint> CreateDistPointsList (string[,] array)
        {
            IList<distancePoint> distancePointsList = new List<distancePoint>();

            double[] distArray = CreateDistArray(array);

            for (int i = 0; i < distArray.Length; i++)
            {
                distancePointsList.Add(new distancePoint());
            }

            for (int i = 0; i < distArray.Length; i++)
            {
                distancePointsList[i].ObjectID = array[i, 0];
                distancePointsList[i].TimeStamp = Convert.ToDateTime(array[i, 1]);
                distancePointsList[i].Coordinates = new Point(Convert.ToDouble(array[i, 2]), Convert.ToDouble(array[i, 3]));
                distancePointsList[i].Distance = distArray[i];
            }

            return distancePointsList;
        }

        /// <summary>
        /// Calculate distance filter value in order to find destination candidates points
        /// </summary>
        /// <param name="array"></param>
        /// <param name="p"></param>
        /// <returns></returns>
        public double FindFilter(string[,] array, double p)
        {
            double filter = 0;

            double[] distArray = CreateDistArray(array);
            
            Array.Sort(distArray);
            double q1 = Percentile(distArray, 25.0);
            double q3 = Percentile(distArray, 75.0);
            double IQR = q3 - q1;
            double binWidth = 2 * IQR / Math.Pow(distArray.Length, 1.0 / 3.0); // Freedman–Diaconis rule
            int binCount = Convert.ToInt32(Math.Ceiling((distArray.Max() - distArray.Min()) / binWidth));

            // Create frequency distribution table, (binWidth, frequency, cumulative frequency, cumulative percentage)
            IList<ParetoFreqTable> freqTable = new List<ParetoFreqTable>();
            
            for (int i = 0; i < binCount; i++)
            {
                freqTable.Add(new ParetoFreqTable());
            }
    
            // set binWidth
            for (int i = 0; i < freqTable.Count(); i++)
            {
                freqTable[i].RangeValue = binWidth * (i + 1);
            }
   
            // set frequency
            for (int i = 0; i < distArray.Length; i++)
            {
                for(int j = 1; j < freqTable.Count(); j++)
                {
                    if (0 <= distArray[i] && distArray[i] < freqTable[j - 1].RangeValue)
                    {
                        freqTable[0].Freq = freqTable[0].Freq + 1;
                        break;
                    }
                    else if (freqTable[j-1].RangeValue <= distArray[i] && distArray[i] < freqTable[j].RangeValue)
                    {
                        freqTable[j].Freq = freqTable[j].Freq + 1;
                        break;
                    }
                }
            }
         
            // calculate cumulative frequency
            freqTable[0].CumulFreq = freqTable[0].Freq;
            for (int i = 1; i < freqTable.Count(); i++)
            {
                freqTable[i].CumulFreq = freqTable[i - 1].CumulFreq + freqTable[i].Freq;
            }
           
            // calculate cumulative percentage
            for (int i = 0; i < freqTable.Count(); i++)
            {
                freqTable[i].CumulPercent = freqTable[i].CumulFreq / Convert.ToDouble(distArray.Length);
            }
           
            // find filter value according to TOP percengtage
            for (int i = 0; i < freqTable.Count(); i++)
            {
                if (freqTable[i].CumulPercent < (p / 100.0) && (p / 100.0) < freqTable[i + 1].CumulPercent)
                {
                    filter = freqTable[i + 1].RangeValue;
                }
            }
            
            return filter;
        }

        /// <summary>
        /// Find destination candidates points
        /// </summary>
        /// <param name="distancePointsList"></param>
        /// <param name="filter"></param>
        /// <returns></returns>
        public IList<CandidatePoint> FindCandidatesPoints(IList<distancePoint> distancePointsList, double filter)
        {
            IList<CandidatePoint> candidatesPointsList = new List<CandidatePoint>();
            
            for (int i = 0; i < distancePointsList.Count(); i++)
            {
                if (distancePointsList[i].Distance < filter)
                {
                    candidatesPointsList.Add(new CandidatePoint()
                    {
                        ObjectID = distancePointsList[i].ObjectID,
                        TimeStamp = distancePointsList[i].TimeStamp,
                        Coordinates = distancePointsList[i].Coordinates
                    });
                    
                }
            }

            return candidatesPointsList;
        }


        // Need to change for selecting input file by user
        private void BtnSelectFile_Click(object sender, RoutedEventArgs e)
        {
            string fileDir = Directory.GetCurrentDirectory();
            txtInputPath.Text = fileDir + @"\experiment\PositionData_RTLS_2020-05-19 14-50-25_00000013.csv"; // need to change in the code
        }


        private void BtnPareto_Click(object sender, RoutedEventArgs e)
        {
            string[,] rawData = ReadCSVfile(txtInputPath.Text); // Read input CSV data
            string[,] rawData_v2 = RemoveEmptyRows(rawData); // Remove missing value rows
            string[,] rawData_v3 = RemoveDuplicateRows(rawData_v2); // Remove duplicate rows

            IList<distancePoint> distancePointsList = CreateDistPointsList(rawData_v3); // Create coordinates data with distance 

            ParetoCanvas pareto = new ParetoCanvas(distancePointsList);
            pareto.Show();

            //double filterDistance = FindFilter(rawData_v3, 60.0); // Find distance filter value for setting cadidates of destination
            //IList<CandidatePoint> candidatePointsList = FindCandidatesPoints(distancePointsList, filterDistance);

        }
        
        private void BtnCandidates_Click(object sender, RoutedEventArgs e)
        {
            
        }
        
    }
}