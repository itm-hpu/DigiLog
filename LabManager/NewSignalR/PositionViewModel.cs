using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.ComponentModel;
using System.Collections.ObjectModel;

namespace NewSignalR
{
    class PositionViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<PositionClass> _positionList { get; set; }
        public ObservableCollection<PositionClass> PositionList
        {
            get { return _positionList; }
            set
            {
                _positionList = value;
                RaisePropertyChanged("PositionList");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void RaisePropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        

    }
}
