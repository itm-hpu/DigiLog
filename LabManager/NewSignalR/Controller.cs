using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json;
using System.Net.Http;
using System.Diagnostics;
using Microsoft.AspNetCore.SignalR.Client;

namespace NewSignalR
{
    
    class Controller
    {
        public string GetID(string uriAddress, string userName, string password)
        {
            string result_IDs = "";
            RESTfulClient rClient = new RESTfulClient();
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

        public string ExtractLastInfo(List<position> positionlist)
        {
            //string temp = "Object, Timestamp, X, Y, Latitude, Longitude, Zone";
            string temp = positionlist[positionlist.Count() - 1].Object + ", " +
                    positionlist[positionlist.Count() - 1].Timestamp + ", " +
                    positionlist[positionlist.Count() - 1].X + ", " +
                    positionlist[positionlist.Count() - 1].Y + ", " +
                    positionlist[positionlist.Count() - 1].latitude + ", " +
                    positionlist[positionlist.Count() - 1].longitude + ", " +
                    positionlist[positionlist.Count() - 1].Zone;

            /*
            for (int i = 0; i < positionlist.Count; i++)
            {
                temp = temp + "\n" +
                    positionlist[i].Object + ", " +
                    positionlist[i].Timestamp + ", " +
                    positionlist[i].X + ", " +
                    positionlist[i].Y + ", " +
                    positionlist[i].latitude + ", " +
                    positionlist[i].longitude + ", " +
                    positionlist[i].Zone;
            }
            */
            return temp;
        }
        
        public double CalcDist(List<position> positionlist)
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
