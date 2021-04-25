using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Assignment4.Models
{
    public class Charts
    {
        public List<string> oris;
        public List<int> crimes;
        public Charts()
        {

        }

        public Charts(List<string> oris, List<int> crimes)
        {
            this.oris = oris;
            this.crimes = crimes;
        }
    }
}
