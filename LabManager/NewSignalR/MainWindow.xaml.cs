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

using Newtonsoft.Json;
using System.Net.Http;
using System.Diagnostics;
using Microsoft.AspNetCore.SignalR.Client;
using System.Threading;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace NewSignalR
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        

        public static System.Net.Http.HttpClient client;
        public HubConnection connection;
        Controller controller = new Controller();

        public static ObservableCollection<Position> positionList1;
        public static ObservableCollection<Position> positionList2;
        public static ObservableCollection<Position> positionList3; 

        public List<Distance> distances;

        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = this;

            txtServer.Text = "p186-geps-production-api.hd-rtls.com";
            txtUserName.Text = "KTH";
            txtPassword.Text = "!Test4KTH";
            txtmax_age.Text = "1440";

            positionList1 = new ObservableCollection<Position>();
            positionList2 = new ObservableCollection<Position>();
            positionList3 = new ObservableCollection<Position>();

            distances = new List<Distance>();
        }

        
        public async void ConnectSignalR()
        {
            string server = txtServer.Text;
            string userName = txtUserName.Text;
            string password = txtPassword.Text;

            string[] id4require = new string[3];
            id4require[0] = cmbTagID1.Text;
            id4require[1] = cmbTagID2.Text;
            id4require[2] = cmbTagID3.Text;

            string Token = await login(server, userName, password);

            connection = new HubConnectionBuilder()
               .WithUrl("https://" + server + "/signalr/beaconPosition", options =>
               {
                   options.Headers.Add("X-Authenticate-Token", Token);
               })
               .Build();

            connection.On<pos>("onEvent", Data =>
            {
                Poskommer("kkK", Data, id4require);
            });

            await connection.StartAsync();
            await connection.InvokeAsync("subscribe", null);
        }

        
        public async void DisconnectSignalR()
        {
            await connection.StopAsync();
        }


        public static void Poskommer(string server, pos p, string[] id4require)
        {
            Position inputforlist = new Position
            {
                Object = p.Object,
                X = p.X,
                Y = p.Y,
                latitude = p.latitude,
                longitude = p.longitude,
                Timestamp = p.Timestamp,
                Zone = p.Zone
            };

            if (inputforlist.Object.ToString() == id4require[0])
            {
                positionList1.Add(inputforlist);
            }
            else if (inputforlist.Object.ToString() == id4require[1])
            {
                positionList2.Add(inputforlist);
            }
            else if (inputforlist.Object.ToString() == id4require[2])
            {
                positionList3.Add(inputforlist);
            }
            
        }
        

        public async Task<string> login(string server, string user, string passw)
        {
            client = new System.Net.Http.HttpClient();
            client.DefaultRequestHeaders.Add("Accept", "application/json");
            client.DefaultRequestHeaders.Add("X-Authenticate-User", user);
            client.DefaultRequestHeaders.Add("X-Authenticate-Password", passw);
            StringContent content = new System.Net.Http.StringContent("{\"Id\": \"" + user + "\", \"Password\": \"" + passw + "\",\"IsAdmin\": true}", Encoding.UTF8, "text/json");
            HttpResponseMessage response = await client.PostAsync("https://" + server + "/login/", content);
            login_cred result = JsonConvert.DeserializeObject<login_cred>(await response.Content.ReadAsStringAsync());
            return result.AuthenticateToken;
        }

        /*
        public async Task Print1()
        {
            var cancellationToken = cancellationTokenSource.Token;
            await Task.Factory.StartNew(() =>
            {
                string result = "";
                for (int i = 0; i < positionList1.Count(); i++)
                {
                    Dispatcher.Invoke(() =>
                    {
                        result = controller.ExtractLastInfo(positionList1);
                        //await Task.Delay(TimeSpan.FromMilliseconds(1 * 1000));
                        txtLog1.Text = txtLog1.Text + result + "\n";
                        txtLog1.ScrollToEnd();
                    });
                }
            }, cancellationTokenSource.Token);
        }

        public async Task Print2()
        {
            var cancellationToken = cancellationTokenSource.Token;
            await Task.Factory.StartNew(() =>
            {
                string result = "";
                for (int i = 0; i < positionList2.Count(); i++)
                {
                    Dispatcher.Invoke(() =>
                    {
                        result = controller.ExtractLastInfo(positionList2);
                        //await Task.Delay(TimeSpan.FromMilliseconds(1 * 1000));
                        txtLog2.Text = txtLog2.Text + result + "\n";
                        txtLog2.ScrollToEnd();
                    });
                }
            }, cancellationTokenSource.Token);
        }

        public async Task Print3()
        {
            var cancellationToken = cancellationTokenSource.Token;
            await Task.Factory.StartNew(() =>
            {
                string result = "";
                for (int i = 0; i < positionList3.Count(); i++)
                {
                    Dispatcher.Invoke(() =>
                    {
                        result = controller.ExtractLastInfo(positionList3);
                        //await Task.Delay(TimeSpan.FromMilliseconds(1 * 1000));
                        txtLog3.Text = txtLog3.Text + result + "\n";
                        txtLog3.ScrollToEnd();
                    });
                }
            }, cancellationTokenSource.Token);
        }
        */

        /*
        public async Task Print1()
        {
            string result = "";
            for (int i = 0; i < positionList1.Count(); i++)
            {
                result = controller.ExtractLastInfo(positionList1);
                await Task.Delay(TimeSpan.FromMilliseconds(1 * 1000));
                txtLog1.Text = txtLog1.Text + result + "\n";
                txtLog1.ScrollToEnd();
            }
        }

        public async Task Print2()
        {
            string result = "";
            for (int i = 0; i < positionList2.Count(); i++)
            {
                result = controller.ExtractLastInfo(positionList2);
                await Task.Delay(TimeSpan.FromMilliseconds(1 * 1000));
                txtLog2.Text = txtLog2.Text + result + "\n";
                txtLog2.ScrollToEnd();
            }
        }

        public async Task Print3()
        {

            string result = "";
            for (int i = 0; i < positionList3.Count(); i++)
            {
                result = controller.ExtractLastInfo(positionList3);
                await Task.Delay(TimeSpan.FromMilliseconds(1 * 1000));
                txtLog3.Text = txtLog3.Text + result + "\n";
                txtLog3.ScrollToEnd();
            }
        }
        */

        private void BtnStart_Click(object sender, RoutedEventArgs e)
        {
            ConnectSignalR();

            txtLog1.Text = "Object, Timestamp, X, Y, Latitude, Longitude, Zone" + "\n";
            txtLog2.Text = "Object, Timestamp, X, Y, Latitude, Longitude, Zone" + "\n";
            txtLog3.Text = "Object, Timestamp, X, Y, Latitude, Longitude, Zone" + "\n";



            /*
            while (((positionList1.Count < 5) && (positionList1.Count < 5)) && (positionList1.Count < 5))
            {
                await Task.Delay(TimeSpan.FromMilliseconds(1 * 1000));
            }

            Task t1 = Print1();
            Task t2 = Print2();
            Task t3 = Print3();

            await t1;
            await t2;
            await t3;
            */
        }

        private void BtnStop_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnUpdate_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void BtnDisconnect_Click(object sender, RoutedEventArgs e)
        {
            //DisconnectSignalR();
        }

        private void btnCheck_Click(object sender, RoutedEventArgs e)
        {
            string objectIDsAddress = "https://" + txtServer.Text + "/objects";
            txtTagID.Text = controller.GetID(objectIDsAddress, txtUserName.Text, txtPassword.Text);

            string[] objectIDs = controller.DivideIDs(txtTagID.Text);
            cmbTagID1.ItemsSource = objectIDs;
            cmbTagID2.ItemsSource = objectIDs;
            cmbTagID3.ItemsSource = objectIDs;

            cmbObjectForDistance.ItemsSource = objectIDs;
            cmbAggregation.ItemsSource = new string[] { "None", "Sum" };
        }

        private void BtnDistance_Click(object sender, RoutedEventArgs e)
        {
            string distanceAddress = "https://" + txtServer.Text + "/time-series/distance/points?order_by=Id&order=Ascending";

            string objectID = cmbObjectForDistance.Text;
            int max_age = Convert.ToInt32(txtmax_age.Text);
            string aggregation = cmbAggregation.Text;

            distances = controller.GetDistance(distanceAddress, txtUserName.Text, txtPassword.Text, objectID, max_age, aggregation);

            txtObjectForDistance.Text = "Object, Timestamp, Distance" + "\n";

            for (int i = 0; i < distances.Count(); i++)
            {
                txtObjectForDistance.Text = txtObjectForDistance.Text + distances[i].Object + ", " + distances[i].Tiemstamp.ToString("yyyy-MM-dd HH:mm:ss") + ", " + distances[i].Value + "\n";
            }
            //txtObjectForDistance.Text = txtObjectForDistance.Text + distances[0].Object + ", " + distances[0].Tiemstamp.ToString("yyyy-MM-dd HH:mm:ss") + ", " + distances[0].Value;

        }
    }


    public class login_cred
    {
        public string ID { get; set; }
        public string AuthenticateToken { get; set; }
        public bool Isadmin { get; set; }
    }

    public class pos
    {
        public float longitude { get; set; }
        public float latitude { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public int Z { get; set; }
        public int Map { get; set; }
        public int Zone { get; set; }
        public string Beacon { get; set; }
        public object Object { get; set; }
        public DateTime Timestamp { get; set; }
        public object Data { get; set; }
        public object Frames { get; set; }
        public string Type { get; set; }
        public string Radio { get; set; }
    }
    
    public class Position
    {
        public float longitude { get; set; }
        public float latitude { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public int Zone { get; set; }
        public object Object { get; set; }
        public DateTime Timestamp { get; set; }
    }

    public class Movement
    {
        public object Object { get; set; }
        public string Type { get; set; }
        public double Distance { get; set; }
        public string Zone { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public DateTime Timespan { get; set; }
    }

    public class Distance
    {
        public object Object { get; set; }
        public DateTime Tiemstamp { get; set; }
        public string Value { get; set; }
    }
}
