using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace APIDemo.Controllers
{
    public class ValuesController : ApiController
    {
        List<string> PointListX;
        List<string> PointListY;
        List<string> PointListZ;
        static List<string> MessageStorage = new List<string>();
        static int NumberOfRequest = 0;
        
        // GET api/values
        public IEnumerable<string> Get()
        {
            if (NumberOfRequest == 0) InitializeList();

            string[] returnValues = new string[MessageStorage.Count];
            for (int i = 0; i < MessageStorage.Count; i++)
            {
                returnValues[i] = MessageStorage[i];
            }

            return returnValues;
        }

        public void InitializeList()
        {            

            MessageStorage.Add("Masoud");
            MessageStorage.Add("Jake");
            MessageStorage.Add("Shweta");
            MessageStorage.Add("Magnus");
            MessageStorage.Add("Jannicke");
            MessageStorage.Add("Hello");

            NumberOfRequest++;

        }

        public string Welcome()
        {
            return "Hej hej!";
        }

        void SetPointList()
        {
            PointListX = new List<string>();
            PointListY = new List<string>();
            PointListZ = new List<string>();

            for (int i = 0; i < 100; i++)
            {
                double x = 0.0;
                double y = 10.0;
                double z = 0.0;

                PointListX.Add((x + i * i * 0.1).ToString());
                PointListY.Add((y - i * 0.1).ToString());
                PointListZ.Add((z).ToString());
            }
        }

        // GET api/values/5
        public IEnumerable<string> Get(int id)
        {
            //id=0 -> PointListX
            //id=1 -> PointListY
            //id=2 -> PointListZ

            SetPointList();

            if (id == 0) return PointListX;
            else if (id == 1) return PointListY;
            else if (id == 2) return PointListZ;
            else return PointListX;

        }

        // POST api/values
        [HttpPost]
        [Route("PostMessage")]
        public void PostMessage(string message)
        {
            MessageStorage.Add(message);
        }

        // PUT api/values/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        public void Delete(int id)
        {
            try
            {
                MessageStorage.RemoveAt(id);
            }
            catch 
            {

            }

            
        }
    }
}
