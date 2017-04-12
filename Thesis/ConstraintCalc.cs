using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Accord.Math;

namespace Thesis
{
    class ConstraintCalc
    {
        public static double calculateAngle(double[] const1, double[] const2)
        {
            double[] nconst1, nconst2;

            if (const1[1]>0.01 && const2[1]>0.01)
            {
                 nconst1 = const1.Divide(const1[1]).Skip(1).ToArray();
                 nconst2 = const2.Divide(const2[1]).Skip(1).ToArray();
            } else
            {
                 nconst1 = const1.Divide(const1[2]).Skip(1).ToArray();
                 nconst2 = const2.Divide(const2[2]).Skip(1).ToArray();
            }
            

            double dot = nconst1.Dot(nconst2);
            double l1 = Math.Sqrt(nconst1.Pow(2).Sum());
            double l2 = Math.Sqrt(nconst2.Pow(2).Sum());

            return (Math.Acos(dot / (l1 * l2))) * 180 / Math.PI;

        }

        public static double calculateDistance(double[] const1, double[] const2)
        {
            double nconst1, nconst2, distance;

            if (Math.Abs(const1[1]) > 0.01 && Math.Abs(const2[1]) > 0.01)
            {
                nconst1 = const1.Divide(Math.Abs(const1[1]))[0];
                nconst2 = const2.Divide(Math.Abs(const2[1]))[0];

                distance = Math.Abs(nconst1 - nconst2);
               // distance *= (Math.Abs(const1[1]) + Math.Abs(const2[1])) / 2;
            }
            else
            {
                nconst1 = const1.Divide(Math.Abs(const1[2]))[0];
                nconst2 = const2.Divide(Math.Abs(const2[2]))[0];

                distance = Math.Abs(nconst1 - nconst2);
                //distance *= (Math.Abs(const1[2]) + Math.Abs(const2[2])) / 2;
            }

            return distance;
        }

        public static double[] meanConstraint(double[] const1, double[] const2)
        {
            double[] nconst1, nconst2;
            //double scaler;

            if (Math.Abs(const1[1]) > 0.01 && Math.Abs(const2[1]) > 0.01)
            {
                nconst1 = const1.Divide(Math.Abs(const1[1]));
                nconst2 = const2.Divide(Math.Abs(const2[1]));

                //scaler = (Math.Abs(const1[1]) + Math.Abs(const2[1])) / 2;
            }
            else
            {
                nconst1 = const1.Divide(Math.Abs(const1[2]));
                nconst2 = const2.Divide(Math.Abs(const2[2]));

                //scaler = (Math.Abs(const1[2]) + Math.Abs(const2[2])) / 2;
            }


            return const1.Add(const2).Divide(2);
        }

    }
}
