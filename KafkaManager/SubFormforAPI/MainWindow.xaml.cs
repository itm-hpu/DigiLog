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
            string MiRAddress = "http://130.237.5.148/api/v2.0.0/status";

            txtRTLSAddress.Text = RTLSAddress;
            txtMiRAddress.Text = MiRAddress;

        }

        private void BtnGetData_Click(object sender, RoutedEventArgs e)
        {
            //Import data using the API

            string apiURL = "";

            if (rdbRTLS.IsChecked == true) apiURL = txtRTLSAddress.Text;
            else if (rdbMir200.IsChecked == true) apiURL = txtMiRAddress.Text;

            GetAsyncAndShow(apiURL);
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
                result = result + tempValue[0].ToString() + "," + tempValue[1].ToString() + ", " + tempValue[2].ToString() + ", " + System.DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss.fff") + "\n"; 

            }

            txtResults.Text = result;

            int dotSize = 5;

            Ellipse currentDot = new Ellipse();
            currentDot.Stroke = new SolidColorBrush(Colors.Green);
            currentDot.StrokeThickness = 3;
            Canvas.SetZIndex(currentDot, 3);
            currentDot.Height = dotSize;
            currentDot.Width = dotSize;
            currentDot.Fill = new SolidColorBrush(Colors.Green);
            currentDot.Margin = new Thickness(tempValue[1]*30.0, tempValue[2]*30.0, 0, 0); // Sets the position.
            myCanvas.Children.Add(currentDot);

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
    }
}
