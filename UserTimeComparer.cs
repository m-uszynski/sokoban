using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SokobanGraph
{
    class UserTimeComparer : IComparer<User>
    {

        public int Compare(User x, User y)
        {
            if (x == null) return (y == null) ? 0 : 1;
            if (y == null) return -1;
            return (x.times["level1"] - y.times["level1"]);
        }
    }
}
