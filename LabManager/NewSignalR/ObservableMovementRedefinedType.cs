using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewSignalR
{
    public class ObservableMovementRedefinedType : ObservableMovementType
    {
        private string redefinedType;

        public string RedefinedType
        {
            get
            {
                return redefinedType;
            }
            set
            {
                redefinedType = value;
                OnPropertyChanged("RedefinedType");
            }
        }
    }
}
