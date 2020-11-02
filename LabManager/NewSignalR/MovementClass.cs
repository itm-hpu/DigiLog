using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewSignalR
{
    public class MovementClass : Notifier
    {
        private object objectId;
        private string type;
        private double distance;
        private int zone;
        private DateTime startTime;
        //private DateTime endTime;
        //private DateTime timespan;

        public object ObjectId
        {
            get
            {
                return objectId;
            }
            set
            {
                objectId = value;
                OnPropertyChanged("ObjectId");
            }
        }
        
        public string Type
        {
            get
            {
                return type;
            }
            set
            {
                type = value;
                OnPropertyChanged("Type");
            }
        }
        
        
        public double Distance
        {
            get
            {
                return distance;
            }
            set
            {
                distance = value;
                OnPropertyChanged("Distance");
            }
        }
        
        
        public int Zone
        {
            get
            {
                return zone;
            }
            set
            {
                zone = value;
                OnPropertyChanged("Zone");
            }
        }
        public DateTime StartTime
        {
            get
            {
                return startTime;
            }
            set
            {
                startTime = value;
                OnPropertyChanged("StartTime");
            }
        }
        /*
        public DateTime EndTime
        {
            get
            {
                return endTime;
            }
            set
            {
                endTime = value;
                OnPropertyChanged("EndTime");
            }
        }
        public DateTime Timespan
        {
            get
            {
                return timespan;
            }
            set
            {
                timespan = value;
                OnPropertyChanged("Timespan");
            }
        }
        */
    }
}
