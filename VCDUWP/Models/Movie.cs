using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VCDUWP.Models
{
    public class Movie:INotifyPropertyChanged
    {
        private string name;
        private string price; 
        
        public string Name { get => name; set => name = value; }
        public string Price { get => price; set => price = value; }

        public event PropertyChangedEventHandler PropertyChanged;
        public void NotifyPropertyChanged(string name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            handler.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
