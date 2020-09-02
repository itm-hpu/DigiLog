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

        public static List<string> positionList11;
        public static List<string> positionList12;
        public static List<string> positionList13;

        public MainWindow()
        {
            InitializeComponent();
            positionList11 = new List<string>();
            positionList12 = new List<string>();
            positionList13 = new List<string>();
        }

        public async void StartSignalR()
        {
            string server = "p184-geps-production-api.hd-rtls.com";
            string userName = "cpal";
            string password = "cpal";

            string objectIDsAddress = "https://" + server + "/objects";
            txtTagID.Text = controller.GetID(objectIDsAddress, userName, password);

            string[] objectIDs = controller.DivideIDs(txtTagID.Text);
            cmbTagID1.ItemsSource = objectIDs;
            cmbTagID2.ItemsSource = objectIDs;
            cmbTagID3.ItemsSource = objectIDs;

            string Token = await login(server, userName, password);

            connection = new HubConnectionBuilder()
               .WithUrl("https://" + server + "/signalr/beaconPosition", options =>
               {

                   options.Headers.Add("X-Authenticate-Token", Token);

               })
               .Build();

            connection.On<pos>("onEvent", Data =>
            {
                Poskommer("kkK", Data, objectIDs);
            });

            await connection.StartAsync();
            await connection.InvokeAsync("subscribe", null);
        }

        public static void Poskommer(string server, pos p)
        {
            if (p.Object.ToString() == "00000011")
            {
                positionList11.Add(p.Object + ", " + p.Timestamp + ", " + p.X + ", " + p.Y + ", " + p.latitude + ", " + p.longitude);
            }
            else if (p.Object.ToString() == "00000012")
            {
                positionList12.Add(p.Object + ", " + p.Timestamp + ", " + p.X + ", " + p.Y + ", " + p.latitude + ", " + p.longitude);
            }
            else if (p.Object.ToString() == "00000013")
            {
                positionList13.Add(p.Object + ", " + p.Timestamp + ", " + p.X + ", " + p.Y + ", " + p.latitude + ", " + p.longitude);
            }
        }

        public static void Poskommer(string server, pos p, string[] objectIDs)
        {
            if (p.Object.ToString() == objectIDs[0])
            {
                positionList11.Add(p.Object + ", " + p.Timestamp + ", " + p.X + ", " + p.Y + ", " + p.latitude + ", " + p.longitude);
            }
            else if (p.Object.ToString() == objectIDs[1])
            {
                positionList12.Add(p.Object + ", " + p.Timestamp + ", " + p.X + ", " + p.Y + ", " + p.latitude + ", " + p.longitude);
            }
            else if (p.Object.ToString() == objectIDs[2])
            {
                positionList13.Add(p.Object + ", " + p.Timestamp + ", " + p.X + ", " + p.Y + ", " + p.latitude + ", " + p.longitude);
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

        private void BtnUpdate_Click(object sender, RoutedEventArgs e)
        {
            string[] objectIDs = controller.DivideIDs(txtTagID.Text);

            string temp11 = "";
            string temp12 = "";
            string temp13 = "";

            if (cmbTagID1.Text == objectIDs[0])
            {
                for (int j = 0; j < positionList11.Count; j++)
                {
                    temp11 = temp11 + positionList11[j] + "\n";
                }
            }

            if (cmbTagID2.Text == objectIDs[1])
            {
                for (int j = 0; j < positionList12.Count; j++)
                {
                    temp12 = temp12 + positionList12[j] + "\n";
                }
            }

            if (cmbTagID3.Text == objectIDs[2])
            {
                for (int j = 0; j < positionList13.Count; j++)
                {
                    temp13 = temp13 + positionList13[j] + "\n";
                }
            }

            txtLog11.Text = temp11;
            txtLog12.Text = temp12;
            txtLog13.Text = temp13;

            txtLog11.ScrollToEnd();
            txtLog12.ScrollToEnd();
            txtLog13.ScrollToEnd();
        }

        private void BtnStart_Click(object sender, RoutedEventArgs e)
        {
            StartSignalR();
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
}
