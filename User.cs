using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SokobanGraph
{
    public class User
    {
        public string Nick { get; set; }
        public int LastLevel { get; set; }
        public Dictionary<string, int> times = new Dictionary<string, int>();

        public User(string nick)
        {
            this.Nick = nick;
            this.LastLevel = 1;
        }
    }
}
