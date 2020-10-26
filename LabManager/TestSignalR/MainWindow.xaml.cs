using Microsoft.AspNetCore.SignalR.Client;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
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

namespace TestSignalR
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static System.Net.Http.HttpClient client;
        public HubConnection connection;
        public MainWindow()
        {
            this.InitializeComponent();
            korigang();
        }

        public async void korigang()
        {


            string Token = await login("p186-geps-production-api.hd-rtls.com", "KTH", "!Test4KTH");


            connection = new HubConnectionBuilder()
               .WithUrl("https://p186-geps-production-api.hd-rtls.com/signalr/position", options =>
               {

                   options.Headers.Add("X-Authenticate-Token", Token);

               })
               .Build();

            connection.On<pos>("onPosition", Data =>
            {


                Poskommer("kkK", Data);
            });

            await connection.StartAsync();
            await connection.InvokeAsync("subscribe");

        }

        public static void Poskommer(string server, pos p)
        {

            int olle = 0;
            Debug.WriteLine(p.Latitude.ToString());
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
    }

    public class login_cred
    {
        public string ID { get; set; }
        public string AuthenticateToken { get; set; }
        public bool Isadmin { get; set; }
    }

    public class pos
    {
        public string Beacon { get; set; }
        public string Box { get; set; }
        public string Object { get; set; }
        public string Device { get; set; }
        public float Longitude { get; set; }
        public float Latitude { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public int Z { get; set; }
        public int Map { get; set; }
        public int Building { get; set; }
        public int Zone { get; set; }
        public int Site { get; set; }
        public string Timestamp { get; set; }
        public string ServerTimestamp { get; set; }
        public int Flags { get; set; }
        public List<object> Frames { get; set; }
        public string Type { get; set; }
        public string Radio { get; set; }
        public string Container { get; set; }
        public Data Data { get; set; }
        public int State { get; set; }
        public Object Events { get; set; }
        public Velocity Velocity { get; set; }
        public float Distance { get; set; }
        public int Quality { get; set; }



    }

    public class Data
    {

        public Acceleration Acceleration { get; set; }
        public Temperature Temperature { get; set; }
        public Battery Battery { get; set; }
        public Compass Compass { get; set; }
    }
    public class Velocity
    {
        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }
        public float Value { get; set; }
    }

    public class Acceleration
    {
        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }
        public string Timestamp { get; set; }
    }
    public class Temperature
    {
        public float Celsius { get; set; }
        public string Timestamp { get; set; }
    }
    public class Battery
    {
        public float level { get; set; }
        public float Voltage { get; set; }
        public string Timestamp { get; set; }
    }
    public class Compass
    {
        public float Degrees { get; set; }
        public string Timestamp { get; set; }
    }
}
