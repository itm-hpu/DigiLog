using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewSignalR
{
    public class DistanceClass : INotifyPropertyChanged
    {

        private object objectId;
        private DateTime timestamp;
        private double distance;

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
        public DateTime Timestamp
        {
            get
            {
                return timestamp;
            }
            set
            {
                timestamp = value;
                OnPropertyChanged("Timestamp");
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

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
