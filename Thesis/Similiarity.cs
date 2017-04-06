using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Accord.Math;

namespace Thesis
{
    class Similiarity
    {
        public static double calculateAngle(double[] v1, double[] v2)
        {
            double dot = v1.Dot(v2);
            double l1 = Math.Sqrt(v1.Pow(2).Sum());
            double l2 = Math.Sqrt(v2.Pow(2).Sum());

            return (Math.Acos(dot / (l1 * l2))) * 180 / Math.PI;

        }

    }
}
