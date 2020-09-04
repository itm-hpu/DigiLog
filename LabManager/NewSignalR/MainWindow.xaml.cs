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

        public static List<string> positionList1;
        public static List<string> positionList2;
        public static List<string> positionList3;

        public MainWindow()
        {
            InitializeComponent();

            txtServer.Text = "p186-geps-production-api.hd-rtls.com";
            txtUserName.Text = "KTH";
            txtPassword.Text = "!Test4KTH";

            positionList1 = new List<string>();
            positionList2 = new List<string>();
            positionList3 = new List<string>();
        }

        public async void StartSignalR()
        {
            string server = txtServer.Text;
            string userName = txtUserName.Text;
            string password = txtPassword.Text;
            //string[] objectIDs = controller.DivideIDs(txtTagID.Text);

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



        public static void Poskommer(string server, pos p)
        {
            if (p.Object.ToString() == "00000011")
            {
                positionList1.Add(p.Object + ", " + p.Timestamp + ", " + p.X + ", " + p.Y + ", " + p.latitude + ", " + p.longitude);
            }
            else if (p.Object.ToString() == "00000012")
            {
                positionList2.Add(p.Object + ", " + p.Timestamp + ", " + p.X + ", " + p.Y + ", " + p.latitude + ", " + p.longitude);
            }
            else if (p.Object.ToString() == "00000013")
            {
                positionList3.Add(p.Object + ", " + p.Timestamp + ", " + p.X + ", " + p.Y + ", " + p.latitude + ", " + p.longitude);
            }
        }

        public static void Poskommer(string server, pos p, string[] id4require)
        {
            if (p.Object.ToString() == id4require[0])
            {
                positionList1.Add(p.Object + ", " + p.Timestamp + ", " + p.X + ", " + p.Y + ", " + p.latitude + ", " + p.longitude);
            }
            else if (p.Object.ToString() == id4require[1])
            {
                positionList2.Add(p.Object + ", " + p.Timestamp + ", " + p.X + ", " + p.Y + ", " + p.latitude + ", " + p.longitude);
            }
            else if (p.Object.ToString() == id4require[2])
            {
                positionList3.Add(p.Object + ", " + p.Timestamp + ", " + p.X + ", " + p.Y + ", " + p.latitude + ", " + p.longitude);
            }
        }
        
        public void Print()
        {
            string[] id4require = new string[3];
            id4require[0] = cmbTagID1.Text;
            id4require[1] = cmbTagID2.Text;
            id4require[2] = cmbTagID3.Text;

            string temp1 = "";
            string temp2 = "";
            string temp3 = "";

            if (cmbTagID1.Text == id4require[0])
            {
                for (int j = 0; j < positionList1.Count; j++)
                {
                    temp1 = temp1 + positionList1[j] + "\n";
                }
            }

            if (cmbTagID2.Text == id4require[1])
            {
                for (int j = 0; j < positionList2.Count; j++)
                {
                    temp2 = temp2 + positionList2[j] + "\n";
                }
            }

            if (cmbTagID3.Text == id4require[2])
            {
                for (int j = 0; j < positionList3.Count; j++)
                {
                    temp3 = temp3 + positionList3[j] + "\n";
                }
            }

            txtLog1.Text = temp1;
            txtLog2.Text = temp2;
            txtLog3.Text = temp3;

            txtLog1.ScrollToEnd();
            txtLog2.ScrollToEnd();
            txtLog3.ScrollToEnd();
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
            Print();
        }

        private void BtnStart_Click(object sender, RoutedEventArgs e)
        {
            StartSignalR();
        }

        private void btnCheck_Click(object sender, RoutedEventArgs e)
        {
            string objectIDsAddress = "https://" + txtServer.Text + "/objects";
            txtTagID.Text = controller.GetID(objectIDsAddress, txtUserName.Text, txtPassword.Text);

            string[] objectIDs = controller.DivideIDs(txtTagID.Text);
            cmbTagID1.ItemsSource = objectIDs;
            cmbTagID2.ItemsSource = objectIDs;
            cmbTagID3.ItemsSource = objectIDs;
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
