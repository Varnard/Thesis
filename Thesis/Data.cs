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

        public bool[] getYAsBool(int trueValue=1) 
        {
            int size = Y.Length;
            bool[] result = new bool[size];

            for (int i = 0; i < size; i++)
            {
                result[i] = (Y[i] == trueValue);
            }
            return result;
        }
    }
}
