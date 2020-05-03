using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;

using Newtonsoft.Json.Linq;

namespace LabManager
{
    public enum httpVerb
    {
        GET,
        POST,
        PUT,
        DELETE
    }

    class RESTClinet
    {
        public string agvAddress { get; set; }
        public string rtlsAddress { get; set; }
        public httpVerb httpMethod { get; set; }
        public string userName { get; set; }
        public string userPassword { get; set; }
        public string iterationNum { get; set; }
        public string intervalTime { get; set; }

        public RESTClinet()
        {
            agvAddress = "";
            rtlsAddress = "";
            httpMethod = httpVerb.GET;
        }

        public string[] makeAgvRequest()
        {
            string strResponseValue = string.Empty;
            string result = string.Empty;
            string[] tempResult = new string[3];

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(agvAddress);

            request.Method = httpMethod.ToString();

            HttpWebResponse response = null;

            try
            {
                response = (HttpWebResponse)request.GetResponse();

                //Proecess the resppnse stream... (could be JSON, XML or HTML etc..._
                using (Stream responseStream = response.GetResponseStream())
                {
                    if (responseStream != null)
                    {
                        using (StreamReader reader = new StreamReader(responseStream))
                        {
                            strResponseValue = reader.ReadToEnd();

                            tempResult = readAgvJson(strResponseValue, "position");
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

            //return result;
            return tempResult;
        }

        public string[] readAgvJson(string jsonStr, string keyNameParent)
        {
            JObject json = JObject.Parse(jsonStr);
            string Orientation = "";
            string XValue = "";
            string YValue = "";

            string[] returnValue = new string[3];

            foreach (var data in json[keyNameParent])
            {
                JProperty jProperty = data.ToObject<JProperty>();

                if (jProperty.Name == "orientation") Orientation = jProperty.Value.ToString();
                else if (jProperty.Name == "x") XValue = jProperty.Value.ToString();
                else if (jProperty.Name == "y") YValue = jProperty.Value.ToString();
            }

            returnValue[0] = Orientation;
            returnValue[1] = XValue;
            returnValue[2] = YValue;

            return returnValue;
        }


        public string[] makeRtlsRequest()
        {
            string strResponseValue = string.Empty;
            string result = string.Empty;
            string[] tempResult = new string[3];
            
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(rtlsAddress);

            request.Accept = "application/json";
            request.Headers.Add("X-Authenticate-User", userName);
            request.Headers.Add("X-Authenticate-Password", userPassword);
            request.Method = httpMethod.ToString();

            HttpWebResponse response = null;

            try
            {
                response = (HttpWebResponse)request.GetResponse();

                var statuscode = response.StatusCode;

                //Proecess the resppnse stream... (could be JSON, XML or HTML etc..._
                using (Stream responseStream = response.GetResponseStream())
                {
                    if (responseStream != null)
                    {
                        using (StreamReader reader = new StreamReader(responseStream))
                        {
                            strResponseValue = reader.ReadToEnd();
                            
                            tempResult = readRtlsJson(strResponseValue, "RootObject");
                        }
                    }
                }
           }
            catch (Exception ex)
            {
                //strResponseValue = "{\"errorMessages\":[\"" + ex.Message.ToString() + "\"],\"errors\":{}}";
                tempResult[0] = string.Empty;
                tempResult[1] = string.Empty;
                tempResult[2] = string.Empty;

            }
            finally
            {
                if (response != null)
                {
                    ((IDisposable)response).Dispose();
                }
            }

            //return result;
            return tempResult;
        }

        public string[] readRtlsJson(string jsonStr, string keyNameParent)
        {
            //JArray jarray = JArray.Parse(jsonStr);
            //var json = jarray[0]; // first element of json array
            JObject json = JObject.Parse(jsonStr);

            string Xvalue = "";
            string Yvalue = "";
            string Zvalue = "";

            string[] returnValue = new string[3];

            Xvalue = (string)json.SelectToken("X");
            Yvalue = (string)json.SelectToken("Y");
            Zvalue = (string)json.SelectToken("Z"); 

            returnValue[0] = Xvalue;
            returnValue[1] = Yvalue;
            returnValue[2] = Zvalue;
            
            return returnValue;
        }
    }
}
