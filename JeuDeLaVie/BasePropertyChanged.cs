using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace JeuDeLaVie {
    public class BasePropertyChanged : INotifyPropertyChanged {

        public event PropertyChangedEventHandler PropertyChanged;
        public void RaisePropertyChanged([CallerMemberName] string propertyname = "") {
            if (this.PropertyChanged != null) 
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyname));
            
        }
    }
}
