using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
