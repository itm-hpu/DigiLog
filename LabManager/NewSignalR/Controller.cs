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
using System.Reflection;

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

        private object GetValueByProperty(object obj, string propertyName)
        {
            // get the type:
            var objType = obj.GetType();

            // iterate the properties
            var prop = (from property in objType.GetProperties()
                            // filter on the name
                        where property.Name == propertyName
                        // select the propertyInfo
                        select property).FirstOrDefault();

            // use the propertyinfo to get the instance->property value
            return prop?.GetValue(obj);
        }

        public void SaveDataToTextFile(ObservableCollection<ObservablePosition> positionList)
        {
            string timeStamp = System.DateTime.Now.ToString("yyyy-MM-dd_HH-mm");
            
            PropertyInfo[] propertyInfos;
            propertyInfos = typeof(ObservablePosition).GetProperties();
            string[] propertyNames = new string[typeof(ObservablePosition).GetProperties().Count()];
            int propertyIndex = 0;
            foreach (PropertyInfo p in propertyInfos)
            {
                propertyNames[propertyIndex] = p.Name;
                propertyIndex++;
            }

            int iLength = positionList.Count();
            int jLength = typeof(ObservablePosition).GetProperties().Count();

            if (iLength > 0)
            {
                string filedir = Directory.GetCurrentDirectory();
                filedir = filedir + @"\000. RTLS_PositionData_" + timeStamp + "_" + positionList[0].ObjectId.ToString() + ".txt";
                StreamWriter file = new StreamWriter(filedir);

                for (int i = 0; i < iLength; i++)
                {
                    string tempResult = string.Empty;
                    for (int j = 0; j < jLength; j++)
                    {
                        tempResult = tempResult + GetValueByProperty(positionList[i], propertyNames[j]).ToString() + ",";
                    }
                    tempResult = tempResult.Remove(tempResult.Length - 1, 1);
                    file.Write(tempResult);
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

        public void SaveDataToTextFile(ObservableCollection<ObservableDistance> distanceList)
        {
            string timeStamp = System.DateTime.Now.ToString("yyyy-MM-dd_HH-mm");

            PropertyInfo[] propertyInfos;
            propertyInfos = typeof(ObservableDistance).GetProperties();
            string[] propertyNames = new string[typeof(ObservableDistance).GetProperties().Count()];
            int propertyIndex = 0;
            foreach (PropertyInfo p in propertyInfos)
            {
                propertyNames[propertyIndex] = p.Name;
                propertyIndex++;
            }

            int iLength = distanceList.Count();
            int jLength = typeof(ObservableDistance).GetProperties().Count();

            if (iLength > 0)
            {
                string filedir = Directory.GetCurrentDirectory();
                filedir = filedir + @"\000. RTLS_DistanceDataFromSignalR_" + timeStamp + "_" + distanceList[0].ObjectId.ToString() + ".txt";
                StreamWriter file = new StreamWriter(filedir);

                for (int i = 0; i < iLength; i++)
                {
                    string tempResult = string.Empty;
                    for (int j = 0; j < jLength; j++)
                    {
                        tempResult = tempResult + GetValueByProperty(distanceList[i], propertyNames[j]).ToString() + ",";
                    }
                    tempResult = tempResult.Remove(tempResult.Length - 1, 1);
                    file.Write(tempResult);
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

            PropertyInfo[] propertyInfos;
            propertyInfos = typeof(Distance).GetProperties();
            string[] propertyNames = new string[typeof(Distance).GetProperties().Count()];
            int propertyIndex = 0;
            foreach (PropertyInfo p in propertyInfos)
            {
                propertyNames[propertyIndex] = p.Name;
                propertyIndex++;
            }

            int iLength = distanceList.Count();
            int jLength = typeof(Distance).GetProperties().Count();

            if (iLength > 0)
            {
                string filedir = Directory.GetCurrentDirectory();
                filedir = filedir + @"\000. RTLS_DistanceDataFromRESTful_" + timeStamp + "_" + distanceList[0].ObjectId.ToString() + ".txt";
                StreamWriter file = new StreamWriter(filedir);

                for (int i = 0; i < iLength; i++)
                {
                    string tempResult = string.Empty;
                    for (int j = 0; j < jLength; j++)
                    {
                        tempResult = tempResult + GetValueByProperty(distanceList[i], propertyNames[j]).ToString() + ",";
                    }
                    tempResult = tempResult.Remove(tempResult.Length - 1, 1);
                    file.Write(tempResult);
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

        public void SaveDataToTextFile(ObservableCollection<ObservableMovement> movementList)
        {
            string timeStamp = System.DateTime.Now.ToString("yyyy-MM-dd_HH-mm");

            PropertyInfo[] propertyInfos;
            propertyInfos = typeof(ObservableMovement).GetProperties();
            string[] propertyNames = new string[typeof(ObservableMovement).GetProperties().Count()];
            int propertyIndex = 0;
            foreach (PropertyInfo p in propertyInfos)
            {
                propertyNames[propertyIndex] = p.Name;
                propertyIndex++;
            }

            int iLength = movementList.Count();
            int jLength = typeof(ObservableMovement).GetProperties().Count();

            if (iLength > 0)
            {
                string filedir = Directory.GetCurrentDirectory();
                filedir = filedir + @"\000. RTLS_Movements_" + timeStamp + "_" + movementList[0].ObjectId.ToString() + ".txt";
                StreamWriter file = new StreamWriter(filedir);

                for (int i = 0; i < iLength; i++)
                {
                    string tempResult = string.Empty;
                    for (int j = 0; j < jLength; j++)
                    {
                        tempResult = tempResult + GetValueByProperty(movementList[i], propertyNames[j]).ToString() + ",";
                    }
                    tempResult = tempResult.Remove(tempResult.Length - 1, 1);
                    file.Write(tempResult);
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

        public static double CalculateDistances(string objectID, ObservableCollection<ObservablePosition> positionlist) // m
        {
            int i = positionlist.Count - 1;

            double distX = positionlist[i].X - positionlist[i - 1].X;
            double distY = positionlist[i].Y - positionlist[i - 1].Y;
            double dist = Math.Sqrt(distX * distX + distY * distY) / 100.0;

            return dist;
        }

        public static double CalculateVelocity(string objectID, ObservableCollection<ObservablePosition> positionlist) // second
        {
            int i = positionlist.Count - 1;

            DateTime startTimeSpan = positionlist[i - 1].Timestamp;
            DateTime endTimeSpan = positionlist[i].Timestamp;

            double startTimeSecond = startTimeSpan.Hour * 10000 + startTimeSpan.Minute * 100 + startTimeSpan.Second;
            double endTimeSecond = endTimeSpan.Hour * 10000 + endTimeSpan.Minute * 100 + endTimeSpan.Second;

            double timeSpan = endTimeSecond - startTimeSecond;

            double dist = CalculateDistances(objectID, positionlist);

            double velocity = dist / timeSpan;

            return velocity;
        }

        public static string CheckMovementType(string objectID, ObservableCollection<ObservablePosition> positionlist, double velocityValue) // m/s
        {
            double velocity = CalculateVelocity(objectID, positionlist);

            if (velocity < velocityValue) // Velocity criteria (m/s)
            {
                return "Stop";
            }
            else
            {
                return "Move";
            }
        }
    }
}
