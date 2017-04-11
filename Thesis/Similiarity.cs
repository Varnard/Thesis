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
            double[] nv1, nv2;

            if (v1[1]>0.01 && v2[1]>0.01)
            {
                 nv1 = v1.Divide(v1[1]).Skip(1).ToArray();
                 nv2 = v2.Divide(v2[1]).Skip(1).ToArray();
            } else
            {
                 nv1 = v1.Divide(v1[2]).Skip(1).ToArray();
                 nv2 = v2.Divide(v2[2]).Skip(1).ToArray();
            }
            

            double dot = nv1.Dot(nv2);
            double l1 = Math.Sqrt(nv1.Pow(2).Sum());
            double l2 = Math.Sqrt(nv2.Pow(2).Sum());

            return (Math.Acos(dot / (l1 * l2))) * 180 / Math.PI;

        }

        public static double calculateDistance(double[] v1, double[] v2)
        {
            double nv1, nv2;

            if (v1[1] > 0.01 && v2[1] > 0.01)
            {
                nv1 = v1.Divide(v1[1])[0];
                nv2 = v2.Divide(v2[1])[0];
            }
            else
            {
                nv1 = v1.Divide(v1[2])[0];
                nv2 = v2.Divide(v2[2])[0];
            }

            return Math.Abs(nv1-nv2);
        }

    }
}
