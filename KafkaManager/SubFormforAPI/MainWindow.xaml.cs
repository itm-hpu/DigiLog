using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

namespace SubFormforAPI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        string result = "";

        public MainWindow()
        {
            InitializeComponent();
        }

        private void BtnGetAddress_Click(object sender, RoutedEventArgs e)
        {
            //Initial settings

            string RTLSAddress = "https://smartfactory.hd-wireless.com/objects/00000001/pos";
            string MiRAddress = "http://130.237.2.110/api/v2.0.0/status";


            txtRTLSAddress.Text = RTLSAddress;
            txtMiRAddress.Text = MiRAddress;

        }

        private void BtnGetData_Click(object sender, RoutedEventArgs e)
        {
            //Import data using the API

            string apiURL = "";

            if (rdbRTLS.IsChecked == true) apiURL = txtRTLSAddress.Text;
            else if (rdbMir200.IsChecked == true) apiURL = txtMiRAddress.Text;

            string totalIteration = txtTotalIteration.Text;
            string interval = txtInterval.Text;
            
            GetAsyncAndShow(apiURL, totalIteration, interval);
            
            
            
        }

        public async Task GetAsyncAndShow(string uri, string iteration, string interval)
        {

            double[] tempValue = new double[3];
            int numOfIteration = Convert.ToInt32(iteration);
            double intervalTime = Convert.ToDouble(interval);

            for (int i = 0; i < numOfIteration; i++)
            {

                using (WebClient wc = new WebClient())
                {
                    string json = wc.DownloadString(uri);
                    /*JObject jobj = JObject.Parse(json);
                    txtResults.Text = "Battery percentage: " + jobj["battery_percentage"].ToString() + "\n" +
                        "Position: " + jobj["position"].ToString();*/

                    tempValue = ReadJson(json, "position");
                    result = result + i.ToString() + ", " + tempValue[0].ToString() + "," + tempValue[1].ToString() + ", " + tempValue[2].ToString() + ", " + System.DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss.fff") + "\n" ;

                }


                txtResults.Text = result;
                txtResults.ScrollToEnd();

                int dotSize = 5;

                Ellipse currentDot = new Ellipse();

                Color c = new Color();
                double tempProgress = (double)i / (double)numOfIteration;
                c = Rainbow(Convert.ToSingle(tempProgress));

                currentDot.Stroke = new SolidColorBrush(c);
                currentDot.StrokeThickness = 3;
                Canvas.SetZIndex(currentDot, 3);
                currentDot.Height = dotSize;
                currentDot.Width = dotSize;

                currentDot.Fill = new SolidColorBrush(c);
                currentDot.Margin = new Thickness(tempValue[1] * 30.0, tempValue[2] * 30.0, 0, 0); // Sets the position.
                myCanvas.Children.Add(currentDot);

                await Task.Delay(TimeSpan.FromMilliseconds(intervalTime * 1000));
            }

        }

        public async Task GetAsyncAndShow(string uri)
        {

            double[] tempValue = new double[3];

                using (WebClient wc = new WebClient())
                {
                    string json = wc.DownloadString(uri);
                    /*JObject jobj = JObject.Parse(json);
                    txtResults.Text = "Battery percentage: " + jobj["battery_percentage"].ToString() + "\n" +
                        "Position: " + jobj["position"].ToString();*/

                    tempValue = ReadJson(json, "position");
                    result = result +  "X , " + tempValue[0].ToString() + "," + tempValue[1].ToString() + ", " + tempValue[2].ToString() + ", " + System.DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss.fff") + "\n";

                }


                txtResults.Text = result;
                txtResults.ScrollToEnd();

                int dotSize = 5;

                Ellipse currentDot = new Ellipse();
            

                Color c = new Color();
                c = Rainbow(Convert.ToSingle(0.5));

                currentDot.Stroke = new SolidColorBrush(c);
                currentDot.StrokeThickness = 3;
                Canvas.SetZIndex(currentDot, 3);
                currentDot.Height = dotSize;
                currentDot.Width = dotSize;

                currentDot.Fill = new SolidColorBrush(c);
                currentDot.Margin = new Thickness(tempValue[1] * 30.0, tempValue[2] * 30.0, 0, 0); // Sets the position.
                myCanvas.Children.Add(currentDot);
            
            

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

        public double[] ReadJson(string jsonStr, string keyNameParent)
        {
            JObject json = JObject.Parse(jsonStr);
            double Orientation = 0.0;
            double XValue= 0.0;
            double YValue = 0.0;           

            double[] returnValue = new double[3];

            foreach (var data in json[keyNameParent])
            {
                JProperty jProperty = data.ToObject<JProperty>();

                if (jProperty.Name == "orientation") Orientation = Convert.ToDouble(jProperty.Value);
                else if (jProperty.Name == "x") XValue = Convert.ToDouble(jProperty.Value);
                else if (jProperty.Name == "y") YValue = Convert.ToDouble(jProperty.Value);                

            }

            returnValue[0] = Orientation;
            returnValue[1] = XValue;
            returnValue[2] = YValue;

            return returnValue;
        }

        private void BtnSingleData_Click(object sender, RoutedEventArgs e)
        {
            //Import data using the API

            string apiURL = "";

            if (rdbRTLS.IsChecked == true) apiURL = txtRTLSAddress.Text;
            else if (rdbMir200.IsChecked == true) apiURL = txtMiRAddress.Text;

            string totalIteration = txtTotalIteration.Text;
            string interval = txtInterval.Text;

            GetAsyncAndShow(apiURL);
        }

        private void BtnRefresh_Click(object sender, RoutedEventArgs e)
        {
            myCanvas.Children.Clear();
        }
    }
}
