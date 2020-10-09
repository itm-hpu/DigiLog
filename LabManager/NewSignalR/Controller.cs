using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json;
using System.Net.Http;
using System.Diagnostics;
using Microsoft.AspNetCore.SignalR.Client;
using System.Collections.ObjectModel;
using System.IO;
using System.Globalization;
using System.Windows.Shapes;
using System.Windows.Media;
using System.Windows.Controls;
using System.Windows;

namespace NewSignalR
{
    
    class Controller
    {
        RESTfulClient rClient = new RESTfulClient();
        public string GetID(string uriAddress, string userName, string password)
        {
            string result_IDs = "";
            //RESTfulClient rClient = new RESTfulClient();
            rClient.uriAddress = uriAddress;
            rClient.userName = userName;
            rClient.userPassword = password;
            string[] responseResult = rClient.GetIDs();
            for (int i = 0; i < responseResult.Count(); i++)
            {
                result_IDs = result_IDs + responseResult[i] + "\n";
            }
            return result_IDs;
        }

        public string[] DivideIDs(string tagIDs)
        {
            string objectIDs = tagIDs;
            string[] objectIDsArray = objectIDs.Split(new char[] { '\n' });
            Array.Resize(ref objectIDsArray, objectIDsArray.Length - 1);
            return objectIDsArray;
        }

        public void SaveDataToTextFile(ObservableCollection<PositionClass> positionList)
        {
            string timeStamp = System.DateTime.Now.ToString("yyyy-MM-dd_HH-mm");

            int iLength = positionList.Count();

            if (iLength > 0)
            {
                string filedir = Directory.GetCurrentDirectory();
                filedir = filedir + @"\000. RTLS_PositionData_" + timeStamp + "_" + positionList[0].ObjectId.ToString() + ".txt";
                StreamWriter file = new StreamWriter(filedir);

                for (int i = 0; i < iLength; i++)
                {
                    file.Write(positionList[i].ObjectId + "," +
                        positionList[i].Timestamp + "," +
                        positionList[i].X + "," +
                        positionList[i].Y + "," +
                        positionList[i].Zone + "," +
                        positionList[i].Longitude + "," +
                        positionList[i].Latitude);
                    file.Write("\n");
                }
                file.Close();
                return;
            }
            else
            {
                return;
            }
        }

        public void SaveDataToTextFile(ObservableCollection<DistanceClass> distanceList)
        {
            string timeStamp = System.DateTime.Now.ToString("yyyy-MM-dd_HH-mm");

            int iLength = distanceList.Count();

            if (iLength > 0)
            {
                string filedir = Directory.GetCurrentDirectory();
                filedir = filedir + @"\000. RTLS_DistanceDataFromSignalR_" + timeStamp + "_" + distanceList[0].ObjectId.ToString() + ".txt";
                StreamWriter file = new StreamWriter(filedir);

                for (int i = 0; i < iLength; i++)
                {
                    file.Write(distanceList[i].ObjectId + "," +
                        distanceList[i].Timestamp + "," +
                        distanceList[i].Distance);
                    file.Write("\n");
                }
                file.Close();
                return;
            }
            else
            {
                return;
            }
        }

        public void SaveDataToTextFile(List<Distance> distanceList)
        {
            string timeStamp = System.DateTime.Now.ToString("yyyy-MM-dd_HH-mm");

            int iLength = distanceList.Count();

            if (iLength > 0)
            {
                string filedir = Directory.GetCurrentDirectory();
                filedir = filedir + @"\000. RTLS_DistanceDataFromRESTful_" + timeStamp + "_" + distanceList[0].ObjectId.ToString() + ".txt";
                StreamWriter file = new StreamWriter(filedir);

                for (int i = 0; i < iLength; i++)
                {
                    file.Write(distanceList[i].ObjectId + "," +
                        distanceList[i].Timestamp + "," +
                        distanceList[i].Value);
                    file.Write("\n");
                }
                file.Close();
                return;
            }
            else
            {
                return;
            }
        }

        public List<Distance> GetDistance(string uriAddress, string userName, string password, string objectID, int max_age, string aggregation)
        {
            rClient.uriAddress = uriAddress + "&object=" + objectID + "&max_age=" + max_age.ToString() + "&aggregation=" + aggregation;
            rClient.userName = userName;
            rClient.userPassword = password;

            List<string[]> responseResult = rClient.GetDistance();

            List<Distance> distances = new List<Distance>(responseResult.Count);

            for (int i = 0; i < responseResult.Count; i++)
            {
                distances.Add
                    (
                    new Distance() {
                        ObjectId = objectID,
                        Timestamp = DateTime.ParseExact(responseResult[i][0], "yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture),
                        Value = double.Parse(responseResult[i][1], CultureInfo.InvariantCulture)
            });
            }

            return distances;
        }

        public static double CalculateDistances(string objectID, ObservableCollection<PositionClass> positionlist)
        {
            int i = positionlist.Count - 1;

            double distX = positionlist[i].X - positionlist[i - 1].X;
            double distY = positionlist[i].Y - positionlist[i - 1].Y;
            double dist = Math.Sqrt(distX * distX + distY * distY);

            return dist;
        }



        /*
        public async Task<double> CalcDist(ObservableCollection<PositionClass> positionlist)
        {
            double sum = 0;

            for (int i = 1; i < positionlist.Count(); i++)
            {
                double distX = positionlist[i].X - positionlist[i - 1].X;
                double distY = positionlist[i].Y - positionlist[i - 1].Y;
                double dist = Math.Sqrt(distX * distX + distY * distY);
                sum = sum + dist;
            }

            await Task.Delay(TimeSpan.FromMilliseconds(1 * 1000));

            return sum;
        }
        */
    }
}
