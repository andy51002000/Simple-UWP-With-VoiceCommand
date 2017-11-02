using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VCDUWP.Models
{
    public class VoiceCommand
    {
        public string Name;
        public string Price;

        public VoiceCommand(string name, string price)
        {
            Name = name;
            Price = price;
        }
    }
}
