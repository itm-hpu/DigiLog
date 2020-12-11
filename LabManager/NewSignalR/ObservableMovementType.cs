﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewSignalR
{
    public class ObservableMovementType : Notifier
    {
        private int index;
        private object objectId;
        private DateTime startTime;
        private DateTime endTime;
        private string type;
        private double movementTime;
        private double distance;
        private double velocity;
        private int zone;
        private string zoneName;


        public int Index
        {
            get
            {
                return index;
            }
            set
            {
                index = value;
                OnPropertyChanged("Index");
            }
        }
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
        public double MovementTime
        {
            get
            {
                return movementTime;
            }
            set
            {
                movementTime = value;
                OnPropertyChanged("MovementTime");
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
        public double Velocity
        {
            get
            {
                return velocity;
            }
            set
            {
                velocity = value;
                OnPropertyChanged("Velocity");
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
        public string ZoneName
        {
            get
            {
                return zoneName;
            }
            set
            {
                zoneName = value;
                OnPropertyChanged("ZoneName");
            }
        }
    }
}
