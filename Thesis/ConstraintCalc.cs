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

            var nconst1 = const1.Skip(1).ToArray();
            var nconst2 = const2.Skip(1).ToArray();

            double dot = nconst1.Dot(nconst2);
            double l1 = Math.Sqrt(nconst1.Pow(2).Sum());
            double l2 = Math.Sqrt(nconst2.Pow(2).Sum());

            double angle = (Math.Acos(dot / (l1 * l2))) * 180 / Math.PI;            
            if (angle>180)
            {
                nconst1 = nconst1.Add(0);
            }

            return angle;

        }


        public static double calculateDistance(double[] const1, double[] const2)
        {
            double distance;

            var c1 = new MathModel(const1);
            var c2 = new MathModel(const2);

            var points = DataProvider.getRefinementPoints();

            int numerator = 0;
            int denominator = 0;

            foreach (var point in points.X)
            {
                if (c1.Decide(point) == c2.Decide(point)) numerator++;
                denominator++;
            }

            distance = 1-((double)numerator / denominator);

            return distance;
        }

        public static double[] meanConstraint(double[] const1, double[] const2)
        {
            double[] nconst1, nconst2;

            double[] candidates = new double[const1.Length - 1];

            for (int i = 1; i < const1.Length; i++)
            {
                candidates[i - 1] = Math.Min(Math.Abs(const1[i]), Math.Abs(const2[i]));
            }

            var maxMin = candidates.Max();

            int scalingIndex = candidates.IndexOf(maxMin)+1;

            if (maxMin > 0.01)
            {
                nconst1 = const1.Divide(Math.Abs(const1[scalingIndex]));
                nconst2 = const2.Divide(Math.Abs(const2[scalingIndex]));                
            }

            else
            {
                nconst1 = const1;
                nconst2 = const2;
            }

            var result = nconst1.Add(nconst2).Divide(2);
            return result;
        }

    }
}
