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

    class restClient
    {
        public string AGVaddress { get; set; }
        public string RTLSaddress { get; set; }
        public httpVerb httpMethod { get; set; }
        public string userName { get; set; }
        public string userPassword { get; set; }
        public string iterationNum { get; set; }
        public string intervalTime { get; set; }

        public restClient()
        {
            AGVaddress = "";
            RTLSaddress = "";
            httpMethod = httpVerb.GET;
        }

        public double[] makeAGVRequest()
        {
            string strResponseValue = string.Empty;
            string result = string.Empty;
            double[] tempResult = new double[3];

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(AGVaddress);

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

                            tempResult = ReadAGVJson(strResponseValue, "position");
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

        public double[] ReadAGVJson(string jsonStr, string keyNameParent)
        {
            JObject json = JObject.Parse(jsonStr);
            double Orientation = 0.0;
            double XValue = 0.0;
            double YValue = 0.0;

            double[] returnValue = new double[3];

            foreach (var data in json[keyNameParent])
            {
                JProperty jProperty = data.ToObject<JProperty>();

                if (jProperty.Name == "orientation") Orientation = Convert.ToDouble(jProperty.Value);
                else if (jProperty.Name == "x") XValue = Convert.ToDouble(jProperty.Value);
                else if (jProperty.Name == "y") YValue = Convert.ToDouble(jProperty.Value);
            }

            returnValue[0] = Orientation;
            returnValue[1] = XValue;
            returnValue[2] = YValue;

            return returnValue;
        }


        public double[] makeRTLSRequest()
        {
            string strResponseValue = string.Empty;
            string result = string.Empty;
            double[] tempResult = new double[3];

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(RTLSaddress);

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
                            
                            tempResult = ReadRTLSJson(strResponseValue, "RootObject");
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

        public double[] ReadRTLSJson(string jsonStr, string keyNameParent)
        {
            //JArray jarray = JArray.Parse(jsonStr);
            //var json = jarray[0]; // first element of json array
            JObject json = JObject.Parse(jsonStr);

            double Xvalue = 0.0;
            double Yvalue = 0.0;
            double Zvalue = 0.0;

            double[] returnValue = new double[3];

            Xvalue = (double)json.SelectToken("X");
            Yvalue = (double)json.SelectToken("Y");
            Zvalue = (double)json.SelectToken("Z"); 

            returnValue[0] = Xvalue;
            returnValue[1] = Yvalue;
            returnValue[2] = Zvalue;
            
            return returnValue;
        }
    }
}
