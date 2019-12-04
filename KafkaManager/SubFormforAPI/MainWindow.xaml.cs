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
        public MainWindow()
        {
            InitializeComponent();
        }

        private void BtnGetAddress_Click(object sender, RoutedEventArgs e)
        {
            //Initial settings

            string RTLSAddress = "https://smartfactory.hd-wireless.com/objects/00000001/pos";
            string MiRAddress = "http://130.237.5.170/api/v2.0.0/status";

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
            using (WebClient wc = new WebClient())
            {
                string json = wc.DownloadString(uri);
                JObject jobj = JObject.Parse(json);
                //object temp = jobj["position"];
                

                txtResults.Text = "Battery percentage: " + jobj["battery_percentage"].ToString() + "\n" +
                    "Position: " + jobj["position"].ToString();                   
                
            }

            int dotSize = 10;

            Ellipse currentDot = new Ellipse();
            currentDot.Stroke = new SolidColorBrush(Colors.Green);
            currentDot.StrokeThickness = 3;
            Canvas.SetZIndex(currentDot, 3);
            currentDot.Height = dotSize;
            currentDot.Width = dotSize;
            currentDot.Fill = new SolidColorBrush(Colors.Green);
            currentDot.Margin = new Thickness(100, 200, 0, 0); // Sets the position.
            myCanvas.Children.Add(currentDot);

        }
    }
}
