﻿using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace NewSignalR
{
    public enum httpVerb
    {
        GET,
        POST,
        PUT,
        DELETE
    }

    class RESTfulClient
    {
        public httpVerb httpMethod { get; set; }
        public string uriAddress { get; set; }
        public string userName { get; set; }
        public string userPassword { get; set; }

        public RESTfulClient()
        {
            httpMethod = httpVerb.GET;
        }

        public string[] GetIDs()
        {
            string strResponseValue = string.Empty;
            string[] tempResult = new string[0];

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uriAddress);

            request.Accept = "application/json";
            request.Headers.Add("X-Authenticate-User", userName);
            request.Headers.Add("X-Authenticate-Password", userPassword);
            request.Method = httpMethod.ToString();

            HttpWebResponse response = null;

            try
            {
                response = (HttpWebResponse)request.GetResponse();
                var statuscode = response.StatusCode;
                using (Stream responseStream = response.GetResponseStream())
                {
                    if (responseStream != null)
                    {
                        using (StreamReader reader = new StreamReader(responseStream))
                        {
                            strResponseValue = reader.ReadToEnd();
                            tempResult = ReadObjectIDJson(strResponseValue, "RootObject");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                strResponseValue = "{\"errorMessages\":[\"" + ex.Message.ToString() + "\"],\"errors\":{}}";
            }
            finally
            {
                if (response != null)
                {
                    ((IDisposable)response).Dispose();
                }
            }
            return tempResult;
        }

        public string[] ReadObjectIDJson(string jsonStr, string keyNameParent)
        {
            var jarray = JsonConvert.DeserializeObject<JArray>(jsonStr);
            string[] returnValue = new string[jarray.Count()];
            for (int i = 0; i < jarray.Count(); i++)
            {
                returnValue[i] = jarray[i].ToString();
            }
            return returnValue;
        }


        public List<string[]> GetDistance()
        {
            string strResponseValue = string.Empty;
            List<string[]> tempResult = new List<string[]>();

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uriAddress);

            request.Accept = "application/json";
            request.Headers.Add("X-Authenticate-User", userName);
            request.Headers.Add("X-Authenticate-Password", userPassword);
            request.Method = httpMethod.ToString();

            HttpWebResponse response = null;

            try
            {
                response = (HttpWebResponse)request.GetResponse();
                var statuscode = response.StatusCode;
                using (Stream responseStream = response.GetResponseStream())
                {
                    if (responseStream != null)
                    {
                        using (StreamReader reader = new StreamReader(responseStream))
                        {
                            strResponseValue = reader.ReadToEnd();
                            tempResult = ReadDistanceJson(strResponseValue, "RootObject");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                strResponseValue = "{\"errorMessages\":[\"" + ex.Message.ToString() + "\"],\"errors\":{}}";
            }
            finally
            {
                if (response != null)
                {
                    ((IDisposable)response).Dispose();
                }
            }
            return tempResult;
        }

        public List<string[]> ReadDistanceJson(string jsonStr, string keyNameParent)
        {
            JArray jarray = JArray.Parse(jsonStr);

            List<string[]> result = new List<string[]>(0);
            string Timestamp = "";
            string Value = "";
            string[] returnValue = new string[2];

            for (int i = 0; i < jarray.Count; i++)
            {
                var json = jarray[i];

                Timestamp = (string)json.SelectToken("Timestamp");
                Value = (string)json.SelectToken("Value");
                
                result.Add(new string[2] {Timestamp, Value});
            }
            return result;
        }


        public List<string[]> GetZoneInfo()
        {
            string strResponseValue = string.Empty;
            List<string[]> tempResult = new List<string[]>();

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uriAddress);

            request.Accept = "application/json";
            request.Headers.Add("X-Authenticate-User", userName);
            request.Headers.Add("X-Authenticate-Password", userPassword);
            request.Method = httpMethod.ToString();

            HttpWebResponse response = null;

            try
            {
                response = (HttpWebResponse)request.GetResponse();
                var statuscode = response.StatusCode;
                using (Stream responseStream = response.GetResponseStream())
                {
                    if (responseStream != null)
                    {
                        using (StreamReader reader = new StreamReader(responseStream))
                        {
                            strResponseValue = reader.ReadToEnd();
                            tempResult = ReadZoneInfoJson(strResponseValue, "RootObject");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                strResponseValue = "{\"errorMessages\":[\"" + ex.Message.ToString() + "\"],\"errors\":{}}";
            }
            finally
            {
                if (response != null)
                {
                    ((IDisposable)response).Dispose();
                }
            }
            return tempResult;
        }

        public List<string[]> ReadZoneInfoJson(string jsonStr, string keyNameParent)
        {
            JArray jarray = JArray.Parse(jsonStr);

            List<string[]> result = new List<string[]>(0);
            string zoneID = "";
            string zoneName = "";
            string[] returnValue = new string[2];

            for (int i = 0; i < jarray.Count; i++)
            {
                var json = jarray[i];

                zoneID = (string)json.SelectToken("Id");
                zoneName = (string)json.SelectToken("Name");

                result.Add(new string[2] { zoneID, zoneName });
            }
            return result;
        }
    }
}
