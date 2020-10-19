﻿using System;
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
using System.Globalization;

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

        public static ObservableCollection<DistanceClass> distancesR_idx1;
        public static ObservableCollection<DistanceClass> distancesR_idx2;
        public static ObservableCollection<DistanceClass> distancesR_idx3;

        public List<Distance> distances1;
        public List<Distance> distances2;
        public List<Distance> distances3;

        public static ObservableCollection<MovementClass> Movements;

        public static ViewModel vm;

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

            distancesR_idx1 = new ObservableCollection<DistanceClass>();
            distancesR_idx2 = new ObservableCollection<DistanceClass>();
            distancesR_idx3 = new ObservableCollection<DistanceClass>();
            listboxDistanceR_idx1.ItemsSource = distancesR_idx1;
            listboxDistanceR_idx2.ItemsSource = distancesR_idx2;
            listboxDistanceR_idx3.ItemsSource = distancesR_idx3;
            
            distances1 = new List<Distance>();
            distances2 = new List<Distance>();
            distances3 = new List<Distance>();

            Movements = new ObservableCollection<MovementClass>();
            MovementListBox1.ItemsSource = Movements;

            vm = new ViewModel();
            DataContext = vm;
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

            int dotsize = 3;
            int valueAdjustConstant = 100; // need to modify FillColor in Dispatcher statements
            
            if (inputforlist.ObjectId.ToString() == id4require[0])
            {
                Application.Current.Dispatcher.Invoke(new Action(delegate 
                {
                    inputforlist.Index = positionList1.Count();
                    positionList1.Add(inputforlist);
                    if (positionList1.Count > 1)
                    {
                        double dist = Controller.CalculateDistances(id4require[0], positionList1);
                        DistanceClass inputfordistlist = new DistanceClass { ObjectId = id4require[0], Timestamp = positionList1[positionList1.Count - 1].Timestamp, Distance = dist };
                        distancesR_idx1.Add(inputfordistlist);
                    }
                    if (positionList1.Count > 0)
                    {
                        SolidColorBrush FillColor1 = new SolidColorBrush(Colors.Red); //FillColor1
                        vm.EllipseNodes.Add(new EllipseNode1
                        {
                            Left = positionList1[positionList1.Count - 1].X / valueAdjustConstant,
                            Top = positionList1[positionList1.Count - 1].Y / valueAdjustConstant,
                            FillColor = FillColor1,
                            Height = dotsize,
                            Width = dotsize
                        });
                    }
                    if (positionList1.Count == 1)
                    {
                        if (Controller.CheckMovementType(positionList1) == "Move")
                        {
                            Movements.Add(new MovementClass
                            {
                                ObjectId = positionList1[0].ObjectId,
                                //Type = Controller.CheckMovementType(positionList1), 
                                Zone = positionList1[0].Zone,
                                StartTime = positionList1[0].Timestamp
                            });
                        }
                        else
                        {
                            Movements.Add(new MovementClass
                            {
                                ObjectId = positionList1[0].ObjectId,
                                //Type = Controller.CheckMovementType(positionList1), 
                                Zone = positionList1[0].Zone,
                                StartTime = positionList1[0].Timestamp
                            });
                        }
                    }
                    if (positionList1.Count > 1)
                    {
                        if (positionList1[positionList1.Count - 1].Zone != positionList1[positionList1.Count - 2].Zone)
                        {
                            Movements.Add(new MovementClass
                            {
                                ObjectId = positionList1[positionList1.Count - 1].ObjectId,
                                //Type = Controller.CheckMovementType(positionList1),
                                Zone = positionList1[positionList1.Count - 1].Zone,
                                StartTime = positionList1[positionList1.Count - 1].Timestamp
                            });
                        }
                    }
                }));
            }
            else if (inputforlist.ObjectId.ToString() == id4require[1])
            {
                Application.Current.Dispatcher.Invoke(new Action(delegate
                {
                    inputforlist.Index = positionList2.Count();
                    positionList2.Add(inputforlist);
                    if (positionList2.Count > 1)
                    {
                        double dist = Controller.CalculateDistances(id4require[1], positionList2);
                        DistanceClass inputfordistlist = new DistanceClass { ObjectId = id4require[1], Timestamp = positionList2[positionList2.Count - 1].Timestamp, Distance = dist };
                        distancesR_idx2.Add(inputfordistlist);
                    }
                    if (positionList2.Count > 0)
                    {
                        SolidColorBrush FillColor2 = new SolidColorBrush(Colors.Blue); //FillColor2
                        vm.EllipseNodes.Add(new EllipseNode2
                        {
                            Left = positionList2[positionList2.Count - 1].X / valueAdjustConstant,
                            Top = positionList2[positionList2.Count - 1].Y / valueAdjustConstant,
                            FillColor = FillColor2,
                            Height = dotsize,
                            Width = dotsize
                        });
                    }
                }));
            }
            else if (inputforlist.ObjectId.ToString() == id4require[2])
            {
                Application.Current.Dispatcher.Invoke(new Action(delegate
                {
                    inputforlist.Index = positionList3.Count();
                    positionList3.Add(inputforlist);
                    if (positionList3.Count > 1)
                    {
                        double dist = Controller.CalculateDistances(id4require[2], positionList3);
                        DistanceClass inputfordistlist = new DistanceClass { ObjectId = id4require[2], Timestamp = positionList3[positionList3.Count - 1].Timestamp, Distance = dist };
                        distancesR_idx3.Add(inputfordistlist);
                    }
                    if (positionList3.Count > 0)
                    {
                        SolidColorBrush FillColor3 = new SolidColorBrush(Colors.Green); //FillColor3
                        vm.EllipseNodes.Add(new EllipseNode3
                        {
                            Left = positionList3[positionList3.Count - 1].X / valueAdjustConstant,
                            Top = positionList3[positionList3.Count - 1].Y / valueAdjustConstant,
                            FillColor = FillColor3,
                            Height = dotsize,
                            Width = dotsize
                        });
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

        //-----------------------------
        // # TabSignalR
        //-----------------------------

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

        private void BtnRefresh_Click(object sender, RoutedEventArgs e)
        {
            Process.Start(Application.ResourceAssembly.Location);
            Application.Current.Shutdown();
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

        private void btnCheck_Click(object sender, RoutedEventArgs e)
        {
            string objectIDsAddress = "https://" + txtServer.Text + "/objects";
            txtTagID.Text = controller.GetID(objectIDsAddress, txtUserName.Text, txtPassword.Text);

            string[] objectIDs = controller.DivideIDs(txtTagID.Text);
            cmbTagID1.ItemsSource = objectIDs;
            cmbTagID2.ItemsSource = objectIDs;
            cmbTagID3.ItemsSource = objectIDs;

            cmbObjectForDistance1.ItemsSource = objectIDs;
            cmbObjectForDistance2.ItemsSource = objectIDs;
            cmbObjectForDistance3.ItemsSource = objectIDs;
            cmbAggregation.ItemsSource = new string[] { "None", "Sum" };
        }

        //-----------------------------
        // # RESTful 
        //-----------------------------

        private void BtnDistance_Click(object sender, RoutedEventArgs e)
        {
            string distanceAddress = "https://" + txtServer.Text + "/time-series/distance/points?order_by=Id&order=Ascending";

            int max_age = Convert.ToInt32(txtmax_age.Text);
            string aggregation = cmbAggregation.Text;

            string objectID1 = cmbObjectForDistance1.Text;
            string objectID2 = cmbObjectForDistance2.Text;
            string objectID3 = cmbObjectForDistance3.Text;
            distances1 = controller.GetDistance(distanceAddress, txtUserName.Text, txtPassword.Text, objectID1, max_age, aggregation);
            distances2 = controller.GetDistance(distanceAddress, txtUserName.Text, txtPassword.Text, objectID2, max_age, aggregation);
            distances3 = controller.GetDistance(distanceAddress, txtUserName.Text, txtPassword.Text, objectID3, max_age, aggregation);

            txtObjectForDistance1.Text = "Object, Timestamp, Distance" + "\n";
            txtObjectForDistance2.Text = "Object, Timestamp, Distance" + "\n";
            txtObjectForDistance3.Text = "Object, Timestamp, Distance" + "\n";

            for (int i = 0; i < distances1.Count(); i++)
            {
                txtObjectForDistance1.Text = txtObjectForDistance1.Text + distances1[i].ObjectId + ", " + distances1[i].Timestamp.ToString("yyyy-MM-dd HH:mm:ss") + ", " + distances1[i].Value + "\n";
                txtObjectForDistance1.ScrollToEnd();
            }
            for (int i = 0; i < distances2.Count(); i++)
            {
                txtObjectForDistance2.Text = txtObjectForDistance2.Text + distances2[i].ObjectId + ", " + distances2[i].Timestamp.ToString("yyyy-MM-dd HH:mm:ss") + ", " + distances2[i].Value + "\n";
                txtObjectForDistance2.ScrollToEnd();
            }
            for (int i = 0; i < distances3.Count(); i++)
            {
                txtObjectForDistance3.Text = txtObjectForDistance3.Text + distances3[i].ObjectId + ", " + distances3[i].Timestamp.ToString("yyyy-MM-dd HH:mm:ss") + ", " + distances3[i].Value + "\n";
                txtObjectForDistance3.ScrollToEnd();
            }
        }

        private void BtnSaveDistance_Click(object sender, RoutedEventArgs e)
        {
            controller.SaveDataToTextFile(distances1);
            controller.SaveDataToTextFile(distances2);
            controller.SaveDataToTextFile(distances3);
        }

        

        // under progressing
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

    public class EllipseNode1 : PositionClass
    {
        public double Left { get; set; }
        public double Top { get; set; }
        public SolidColorBrush FillColor { get; set; }
        public int Height { get; set; }
        public int Width { get; set; }
    }

    public class EllipseNode2 : PositionClass
    {
        public double Left { get; set; }
        public double Top { get; set; }
        public SolidColorBrush FillColor { get; set; }
        public int Height { get; set; }
        public int Width { get; set; }
    }

    public class EllipseNode3 : PositionClass
    {
        public double Left { get; set; }
        public double Top { get; set; }
        public SolidColorBrush FillColor { get; set; }
        public int Height { get; set; }
        public int Width { get; set; }
}


    public class ViewModel
    {
        public ObservableCollection<PositionClass> EllipseNodes { get; } = new ObservableCollection<PositionClass>();
    }
}
