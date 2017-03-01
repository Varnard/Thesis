using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Thesis
{
    public class Data
    {
        public double[][] X { get; set; } 
        public int[] Y { get; set; }

        public Data(double[][] X, int[] Y) 
        {
            this.X = X;
            this.Y = Y;
        }
    }
}
