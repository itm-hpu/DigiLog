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

        public static ObservableCollection<PositionClass> positionList1;
        public static ObservableCollection<PositionClass> positionList2;
        public static ObservableCollection<PositionClass> positionList3;

        public List<Distance> distances;
        public static ObservableCollection<DistanceClass> distancesR_idx1;
        public static ObservableCollection<DistanceClass> distancesR_idx2;
        public static ObservableCollection<DistanceClass> distancesR_idx3;

        public MainWindow()
        {
            InitializeComponent();

            txtServer.Text = "p184-geps-production-api.hd-rtls.com";
            txtUserName.Text = "cpal";
            txtPassword.Text = "cpal";
            txtmax_age.Text = "1440";
            
            positionList1 = new ObservableCollection<PositionClass>();
            positionList2 = new ObservableCollection<PositionClass>();
            positionList3 = new ObservableCollection<PositionClass>();
            listbox1.ItemsSource = positionList1; //binding data + need to bind as well in xaml
            listbox2.ItemsSource = positionList2;
            listbox3.ItemsSource = positionList3;

            distances = new List<Distance>();
            distancesR_idx1 = new ObservableCollection<DistanceClass>();
            distancesR_idx2 = new ObservableCollection<DistanceClass>();
            distancesR_idx3 = new ObservableCollection<DistanceClass>();
            listboxDistanceR_idx1.ItemsSource = distancesR_idx1;
            listboxDistanceR_idx2.ItemsSource = distancesR_idx2;
            listboxDistanceR_idx3.ItemsSource = distancesR_idx3;
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
            PositionClass inputforlist = new PositionClass
            {
                ObjectId = p.Object,
                X = p.X,
                Y = p.Y,
                Latitude = p.latitude,
                Longitude = p.longitude,
                Timestamp = p.Timestamp,
                Zone = p.Zone
            };

            if (inputforlist.ObjectId.ToString() == id4require[0])
            {
                Application.Current.Dispatcher.BeginInvoke(new Action(delegate 
                {
                    positionList1.Add(inputforlist);
                    if (positionList1.Count > 1)
                    {
                        double dist = CalculateDistances(id4require[0], positionList1);
                        DistanceClass inputfordistlist = new DistanceClass { ObjectId = id4require[0], Timestamp = positionList1[positionList1.Count - 1].Timestamp, Distance = dist };
                        distancesR_idx1.Add(inputfordistlist);
                    }
                }));
            }
            else if (inputforlist.ObjectId.ToString() == id4require[1])
            {
                Application.Current.Dispatcher.BeginInvoke(new Action(delegate
                {
                    positionList2.Add(inputforlist);
                    if (positionList2.Count > 1)
                    {
                        double dist = CalculateDistances(id4require[1], positionList2);
                        DistanceClass inputfordistlist = new DistanceClass { ObjectId = id4require[1], Timestamp = positionList2[positionList2.Count - 1].Timestamp, Distance = dist };
                        distancesR_idx2.Add(inputfordistlist);
                    }
                }));
            }
            else if (inputforlist.ObjectId.ToString() == id4require[2])
            {
                Application.Current.Dispatcher.BeginInvoke(new Action(delegate
                {
                    positionList3.Add(inputforlist);
                    if (positionList3.Count > 1)
                    {
                        double dist = CalculateDistances(id4require[2], positionList3);
                        DistanceClass inputfordistlist = new DistanceClass { ObjectId = id4require[2], Timestamp = positionList3[positionList3.Count - 1].Timestamp, Distance = dist };
                        distancesR_idx3.Add(inputfordistlist);
                    }
                }));
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
        
        private void BtnStart_Click(object sender, RoutedEventArgs e)
        {
            ConnectSignalR();
        }

        private void BtnStop_Click(object sender, RoutedEventArgs e)
        {
            DisconnectSignalR();

            string message = "Stop acquiring data!";
            MessageBox.Show(message);
        }

        private void BtnDebbug_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnClear_Click(object sender, RoutedEventArgs e)
        {
            txtTagID.Clear();
            cmbTagID1.ItemsSource = null;
            cmbTagID2.ItemsSource = null;
            cmbTagID3.ItemsSource = null;
            listbox1.ItemsSource = null;
            listbox2.ItemsSource = null;
            listbox3.ItemsSource = null;
            cmbObjectForDistance.ItemsSource = null;
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
                txtObjectForDistance.ScrollToEnd();
            }
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            controller.SaveDataToTextFile(positionList1);
            controller.SaveDataToTextFile(positionList2);
            controller.SaveDataToTextFile(positionList3);
            controller.SaveDataToTextFile(distancesR_idx1);
            controller.SaveDataToTextFile(distancesR_idx2);
            controller.SaveDataToTextFile(distancesR_idx3);

            string message = "Save a acquired data!";
            MessageBox.Show(message);
        }

        // under progressing
        public static double CalculateDistances(string objectID, ObservableCollection<PositionClass> positionlist)
        {
            int i = positionlist.Count - 1;

            double distX = positionlist[i].X - positionlist[i - 1].X;
            double distY = positionlist[i].Y - positionlist[i - 1].Y;
            double dist = Math.Sqrt(distX * distX + distY * distY);

            return dist;
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
        public float Longitude { get; set; }
        public float Latitude { get; set; }
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
        public double Value { get; set; }
    }
}
