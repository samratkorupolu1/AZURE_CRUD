using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Assignment4.Models
{
    public class ChartPlot
    {
        public string Label { get; set; }
        public int Y { get; set; }

        public ChartPlot(string label, int y)
        {
            this.Label = label;
            this.Y = y;
        }
    }
}
