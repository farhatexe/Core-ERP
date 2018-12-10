using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Core.Models
{
    public class BaseClass : INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler PropertyChanged;

        public void RaisePropertyChanged(string prop)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}
