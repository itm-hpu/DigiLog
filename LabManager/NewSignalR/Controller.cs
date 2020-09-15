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

        public string ExtractLastInfo(List<Position> positionlist)
        {
            string temp = positionlist[positionlist.Count() - 1].Object + ", " +
                    positionlist[positionlist.Count() - 1].Timestamp + ", " +
                    positionlist[positionlist.Count() - 1].X + ", " +
                    positionlist[positionlist.Count() - 1].Y + ", " +
                    positionlist[positionlist.Count() - 1].latitude + ", " +
                    positionlist[positionlist.Count() - 1].longitude + ", " +
                    positionlist[positionlist.Count() - 1].Zone;
            return temp;
        }

        public string ExtractLastInfo(ObservableCollection<Position> positionlist)
        {
            string temp = positionlist[positionlist.Count() - 1].Object + ", " +
                    positionlist[positionlist.Count() - 1].Timestamp + ", " +
                    positionlist[positionlist.Count() - 1].X + ", " +
                    positionlist[positionlist.Count() - 1].Y + ", " +
                    positionlist[positionlist.Count() - 1].latitude + ", " +
                    positionlist[positionlist.Count() - 1].longitude + ", " +
                    positionlist[positionlist.Count() - 1].Zone;
            return temp;
        }

        public List<Distance> GetDistance(string uriAddress, string userName, string password, string objectID, int max_age, string aggregation)
        {
            rClient.uriAddress = uriAddress + "&object=" + objectID + "&max_age=" + max_age.ToString() + "&aggregation=" + aggregation;
            rClient.userName = userName;
            rClient.userPassword = password;

            string[] responseResult = rClient.GetDistance();

            List<Distance> distances = new List<Distance>()
            {
                new Distance()
                {
                    Object = objectID,
                    Tiemstamp = DateTime.ParseExact(responseResult[0], "yyyy-MM-dd HH:mm:ss.fff", null),
                    Value = responseResult[1]
                }
            };

            return distances;
        }
        
        public double CalcDist(List<Position> positionlist)
        {
            double sum = 0;

            for (int i = 1; i < positionlist.Count(); i++)
            {
                double distX = positionlist[i].X - positionlist[i - 1].X;
                double distY = positionlist[i].Y - positionlist[i - 1].Y;
                double dist = Math.Sqrt(distX * distX + distY * distY);
                sum = sum + dist;
            }

            return sum;
        }
        
    }
}
